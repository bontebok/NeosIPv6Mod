## NeosIPv6Mod

[Neos VR](https://neos.com/) 用の [NeosModLoader](https://github.com/zkxs/NeosModLoader) modで、セッションに透過的な IPv6接続を提供します。このMODはサードパーティのIPv6 LNLサーバーを利用してIPv6のUDPパンチスルーを容易にします。このMODはすべてのユーザーに対して透過的であるべきで、IPv6パンチスルーを試みた後にIPv4にフォールバックします。

NeosIPv6Mod には、[ModSettings](https://github.com/badhaloninja/NeosModSettings) modを利用して変更できる3つの設定が含まれています。 設定はすぐに適用されますが、現在のセッション接続には影響せず、パンチスルー プロセスを通じて確立された接続にのみ影響します。

NeosでIPv6を使用するには、両者（ホストとクライアント）がIPv6のIPアドレスを持ち、MODをインストールしている必要があります。IPv6がないと、このMODは正しく機能しません。問題がある場合は、まずIPv6 IPアドレスを持っていることを確認してください。[Test-IPv6.com](https://test-ipv6.com) を使うと確認することができます。 IPv6 IPアドレスをお持ちでない場合は、インターネットサービスプロバイダにお問い合わせください。

このModはNeos Headlessサーバーでも動作するはずですが、この機能はテストされていません。


# インストール

1. [NeosModLoader](https://github.com/zkxs/NeosModLoader)をインストールします。
1. [NeosIPv6Mod.dll](https://github.com/bontebok/NeosIPv6Mod/releases) を「nml_mods」フォルダーに配置します。 デフォルトのインストールでは、このフォルダーは `C:\Program Files (x86)\Steam\steamapps\common\NeosVR\nml_mods` にある必要があります。 見つからない場合は作成できます。または、NeosModLoader がインストールされた状態でゲームを一度起動すると、フォルダーが作成されます。
1. ゲームを開始します。 MOD が動作していることを確認したい場合は、Neos ログを確認してください。


# 設定オプション

|構成オプション    |デフォルト            |説明                                                                                                               |
|----------------|---------------------|-------------------------------------------------------------------------------------------------------------------|
|`ipv6LnlServer` |`lnl6.razortune.com` |IPv6 UDP パンチ スルーの実行に使用される IPv6 LNL サーバーのホスト名。                                                  |
|`ipv6Only`      |`false`              |パンチスルーには IPv6 のみを使用し、IPv4 は完全に無視します。 これにより、LNL リレー接続が妨げられることに注意してください。  |
|`disableMod`    |`false`              |IPv6 の試行を実行せず、標準の Neos ネットワークにフォールバックします。                                                  |


# 謝辞

※このModは、Strict-Type NATやCGNAT（Carrier Grade NAT）を含むIPv4の各種制限によりNeosの利用が制限されているユーザー向けです。
* この MOD の開発を支援してくれた Neos Modding Community に感謝します。
* テストとコードレビューに対する [Stiefel Jackal](https://github.com/stiefeljackal) に感謝します。


# 既知の問題

* 単一のNatPunchModuleがIPv4とIPv6の両方で共有される方法の性質により、一部のIPログが正しくない可能性があります。これは将来のリリースで解決されることを期待しています。
* 問題が見つかった場合は、解決されるよう上記の問題を使用して報告してください。 プルリクエストは大歓迎です!
