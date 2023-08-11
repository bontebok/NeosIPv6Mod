# NeosIPv6Mod

A [NeosModLoader](https://github.com/zkxs/NeosModLoader) mod for [Neos VR](https://neos.com/) to provide transparent IPv6 connectivity for sessions. The mod utilizes a third party IPv6 LNL Server to facilitate IPv6 UDP punch throughs. The mod should be transparent for all users and falls back to IPv4 after attempting an IPv6 punch through.

The NeosIPv6Mod includes three settings which utilize the [ModSettings](https://github.com/badhaloninja/NeosModSettings) mod to allow them to be changed. The settings apply immediately but do not affect any current session connections, only those established through the punch through process.

The Mod should work under Neos Headless servers as well, but this feature has not been tested.


## Installation

1. Install [NeosModLoader](https://github.com/zkxs/NeosModLoader).
1. Place [NeosIPv6Mod.dll]() into your `nml_mods` folder. This folder should be at `C:\Program Files (x86)\Steam\steamapps\common\NeosVR\nml_mods` for a default install. You can create it if it's missing, or if you launch the game once with NeosModLoader installed it will create the folder for you.
1. Make a backup copy of the LiteNetLib.dll file located in your Neos_Data\Managed folder, which should be located at `C:\Program Files (x86)\Steam\steamapps\common\NeosVR\Neos_Data\Managed`. Copy the LiteNetLib.dll file from the version contained in release for this mod into the Managed folder. As LNL does not by default include IPv6 punch through support, this is required. Source can be found here: [LiteNetLib](https://github.com/bontebok/LiteNetLib)
1. Start the game. If you want to verify that the mod is working you can check your Neos logs.


## Config Options

|Config Option   |Default              |Description                                                                                               |
|----------------|---------------------|----------------------------------------------------------------------------------------------------------|
|`ipv6LnlServer` |`lnl6.razortune.com` |The hostname of the IPv6 LNL Server used for performing IPv6 UDP punch through.                           |
|`ipv6Only`      |`false`              |Only use IPv6 for punch through and ignore IPv4 entirely. Note, this will prevent LNL Relay connectivity. |
|`disableMod`    |`false`              |Do not perform any IPv6 attempts and fallback to standard Neos networking.                                |


# Thank You

* This Mod is dedicated to the users who are limited in being able to use Neos due to various IPv4 restrictions, including Strict-Type NAT, or CGNAT (Carrier Grade NAT).


# Issues

* Some IP logging may not be correct due to the nature of how a single NatPunchModule is shared for both IPv4 and IPv6, I hope to resolve this in future releases.
* If you find any issues, please report them using the issues above so that they are addressed. Pull requests welcome!