using HarmonyLib;
using NeosModLoader;
using FrooxEngine;
using NetX;
using System;
using System.Net;
using System.Threading.Tasks;
using LiteNetLib;
using System.Reflection;
using System.Collections.Generic;


namespace NeosIPv6Mod
{
    public class NeosIPv6Mod : NeosMod
    {
        public override string Name => "NeosIPv6Mod";
        public override string Author => "Rucio";
        public override string Version => "1.0.0";
        public override string Link => "https://github.com/bontebok/NeosIPv6Mod";

        [AutoRegisterConfigKey]
        public static readonly ModConfigurationKey<string> MATCHMAKER_EPv6 = new("ipv6LnlServer", "IPv6 LNL Server: The hostname of the IPv6 LNL Server used for performing IPv6 UDP punch through.", () => "lnl6.razortune.com");

        [AutoRegisterConfigKey]
        public static readonly ModConfigurationKey<bool> IPV6_ONLY = new("ipv6Only", "IPv6 Only: Only use IPv6 for punch through and ignore IPv4 entirely. Note, this will prevent LNL Relay connectivity.", () => false);

        [AutoRegisterConfigKey]
        public static readonly ModConfigurationKey<bool> DISABLEMOD = new("disableMod", "Disable Mod: Do not perform any IPv6 attempts and fallback to standard Neos networking.", () => false);

        public static ModConfiguration Config;
        public override void OnEngineInit()
        {
            try
            {
                Config = GetConfiguration();
                Harmony harmony = new Harmony("com.ruciomods.neosipv6mod");
                harmony.PatchAll();
            }
            catch (Exception ex)
            {
                Error(ex);
            }
        }

        [HarmonyPatch(typeof(LNL_Listener))]
        public class NeosIPv6ModPatch
        {
            public static IPEndPoint _cachedMatchmakerEPv6;
            public static IPEndPoint _cachedMatchmakerEPv4;

            private static IPEndPoint MatchMakerEPv6
            {
                get
                {
                    if (_cachedMatchmakerEPv6 == null)
                        try
                        {
                            Msg($"Resolving LNL IPv6 Server IP: {Config.GetValue(MATCHMAKER_EPv6)}\n");
                            _cachedMatchmakerEPv6 = new IPEndPoint(Dns.GetHostEntry(Config.GetValue(MATCHMAKER_EPv6)).AddressList[0], 12501);
                        }
                        catch (Exception ex)
                        {
                            Msg($"Exception resolving LNL IPv6 Server IP:\n {ex?.ToString()}");
                            _cachedMatchmakerEPv6 = null;
                        }
                    return _cachedMatchmakerEPv6;
                }
            }

            [HarmonyPrefix]
            [HarmonyPatch(typeof(LNL_Listener), "GlobalAnnounceRefresh")]
            public static bool GlobalAnnounceRefresh(LNL_Listener __instance, ref bool ___initialized, ref World ____world, ref NetManager ____server)
            {
                bool ipv6only = Config.GetValue(IPV6_ONLY);
                bool disablemod = Config.GetValue(DISABLEMOD);
                if (disablemod)
                    return true;

                if (!___initialized || ____world?.SessionId == null)
                    return true; // Let the IPv4 deal with this

                try
                {
                    ____server.NatPunchModule.SendNatIntroduceRequest(MatchMakerEPv6, "S;" + ____world.SessionId + ";" + LNL_Implementer.SecretAnnounceKey.Value);
                }
                catch (Exception ex)
                {
                    Msg($"Exception in GlobalAnnounce: {ex?.ToString()}");
                }
                if (ipv6only)
                    return false;
                else
                    return true;
            }

            private static readonly PropertyInfo Peer = AccessTools.Property(typeof(LNL_Connection), "Peer");
            private static readonly PropertyInfo FailReason = AccessTools.Property(typeof(LNL_Connection), "FailReason");
            private static readonly PropertyInfo MATCHMAKER_EP = AccessTools.Property(typeof(LNL_Implementer), "MATCHMAKER_EP");
            private static readonly MethodInfo ConnectToRelay = AccessTools.Method(typeof(LNL_Connection), "ConnectToRelay");

            [HarmonyPrefix]
            [HarmonyPatch(typeof(LNL_Connection), "PunchthroughConnect")]
            public static bool PunchthroughConnect(LNL_Connection __instance, Action<LocaleString> statusCallback, ref NetManager ____client, ref string ____connectionToken, ref ConnectionEvent ___ConnectionFailed, ref Task __result)
            {
                bool ipv6only = Config.GetValue(IPV6_ONLY);
                bool disablemod = Config.GetValue(DISABLEMOD);
                if (disablemod)
                    return true;

                string connectionToken = ____connectionToken;
                NetManager client = ____client;
                _cachedMatchmakerEPv4 = MATCHMAKER_EP.GetValue(__instance) as IPEndPoint;
                ConnectionEvent ConnectionFailed = ___ConnectionFailed;

                __result = Task.Run(async () =>
                {
                    await new ToBackground();

                    bool flag = false;
                    foreach (string commandLineArg in Environment.GetCommandLineArgs())
                    {
                        if (commandLineArg.ToLower().EndsWith("forcerelay"))
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        for (int i = 0; i < 5; ++i) // IPv6 first
                        {
                            statusCallback("World.Connection.LNL.NATPunchthrough".AsLocaleKey((string)null, "n", (object)i));
                            Msg($"IPv6 Punchthrough attempt: {i.ToString()}");
                            client.NatPunchModule.SendNatIntroduceRequest(MatchMakerEPv6, "C;" + connectionToken);
                            await Task.Delay(TimeSpan.FromSeconds(1.0));

                            if (Peer.GetValue(__instance) != null || !client.IsRunning)
                                return false; // Connected??
                        }
                        if (ipv6only)
                        {
                            Msg($"IPv6 Punchthrough failed");
                        }
                        else
                        {
                            Msg($"IPv6 Punchthrough failed, falling back to IPv4");

                            for (int i = 0; i < 5; ++i)
                            {
                                statusCallback("World.Connection.LNL.NATPunchthrough".AsLocaleKey((string)null, "n", (object)i));
                                Msg($"IPv4 Punchthrough attempt: {i.ToString()}");
                                client.NatPunchModule.SendNatIntroduceRequest(_cachedMatchmakerEPv4, "C;" + connectionToken);
                                await Task.Delay(TimeSpan.FromSeconds(1.0));

                                if (Peer.GetValue(__instance) != null || !client.IsRunning)
                                    return false; // Connected???
                            }
                        }
                    }
                    if (!ipv6only)
                    {
                        statusCallback("World.Connection.LNL.Relay".AsLocaleKey((string)null, true, (Dictionary<string, object>)null));
                        Msg("IPv4 Punchthrough failed, Connecting to Relay");
                        AccessTools.MethodDelegate<Action>(ConnectToRelay, __instance).Invoke();
                    }
                    else
                    {
                        // Exausted all options, fail
                        FailReason.SetValue(__instance, "World.Error.FailedToConnect");
                        ConnectionFailed?.Invoke(__instance);
                    }
                    return false;

                });
                return false;
            }
        }
    }
}