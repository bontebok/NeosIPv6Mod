# NeosIPv6Mod

세션에 투명한 IPv6 연결을 제공하는 [Neos VR](https://neos.com/)용 [NeosModLoader](https://github.com/zkxs/NeosModLoader) 모드입니다. 이 모드는 타사 IPv6 LNL 서버를 활용하여 IPv6 UDP 펀치 스루를 용이하게 합니다. 이 모드는 모든 사용자에게 투명해야 하며, IPv6 펀치 스루를 시도한 후에는 IPv4로 되돌아갑니다.


NeosIPv6Mod에는 [ModSettings](https://github.com/badhaloninja/NeosModSettings) 모드를 사용하여 변경할 수 있는 세 가지 설정이 포함되어 있습니다. 설정은 즉시 적용되지만 현재 세션 연결에는 영향을 미치지 않으며, 펀치 스루 프로세스를 통해 설정된 세션 연결에만 영향을 미칩니다.

Neos에서 IPv6를 사용하려면 양쪽 당사자(호스트와 클라이언트)가 모두 IPv6 IP 주소를 가지고 있고 모드를 설치해야 합니다. IPv6가 없으면 이 모드가 제대로 작동하지 않습니다. 문제가 있는 경우 먼저 IPv6 IP 주소가 있는지 확인하려면 다음 사이트에서 확인을 받으세요. [Test-IPv6.com](https://test-ipv6.com/). IPv6 IP 주소가 없는 경우 인터넷 서비스 제공업체에 문의하여 도움을 받으세요.

NeosIPv6Mod는 Windows 및 Linux 클라이언트뿐만 아니라 Windows 및 Linux 헤드리스 클라이언트에서도 작동합니다.


## 설치

1. [NeosModLoader](https://github.com/zkxs/NeosModLoader)를 설치합니다.
1. [NeosIPv6Mod.dll](https://github.com/bontebok/NeosIPv6Mod/releases)을 `nml_mods` 폴더에 넣습니다. 이 폴더는 기본 설치의 경우 `C:\Program Files (x86)\Steam\steamapps\common\NeosVR\nml_mods`에 있어야 합니다. 이 폴더가 없는 경우 직접 만들거나 NeosModLoader를 설치한 상태에서 게임을 한 번 실행하면 폴더가 자동으로 생성됩니다.
1. 게임을 시작합니다. 모드가 작동하는지 확인하려면 네오스 로그를 확인하면 됩니다.


## 구성 옵션

|구성 옵션        |기본값               |설명                                                                                                       |
|----------------|---------------------|----------------------------------------------------------------------------------------------------------|
|`ipv6LnlServer` |`lnl6.razortune.com` |IPv6 UDP 펀치 스루를 수행하는 데 사용되는 IPv6 LNL 서버의 호스트 이름입니다.                                   |
|`ipv6Only`      |`false`              |펀치 스루에는 IPv6만 사용하고 IPv4는 완전히 무시하세요. 이렇게 하면 LNL 릴레이 연결이 차단된다는 점에 유의하세요. |
|`disableMod`    |`false`              |IPv6 시도를 수행하지 말고 표준 Neos 네트워킹으로 폴백하세요.                                                  |


# 감사합니다

* 이 모드는 엄격한 유형 NAT 또는 CGNAT (캐리어 등급 NAT)를 포함한 다양한 IPv4 제한으로 인해 Neos를 사용하는 데 제한이 있는 사용자를 위한 전용 모드입니다.
* 이 모드를 개발하는 데 도움을 주신 "Neos" 모딩 커뮤니티에 감사드립니다.
* 테스트 및 코드 검토를 도와준 [Stiefel Jackal](https://github.com/stiefeljackal)에게 감사드립니다.
* 일본어 번역을 제공해 주신 [litalita](https://github.com/litalita0)에게 감사드립니다.

# 이슈

* 단일 NatPunchModule이 IPv4와 IPv6 모두에 대해 공유되는 방식의 특성으로 인해 일부 IP 로깅이 올바르지 않을 수 있으며, 향후 릴리스에서 이 문제를 해결하고자 합니다.
* 문제가 발견되면 위의 이슈를 통해 신고하여 해결될 수 있도록 도와주세요. 풀 리퀘스트 환영합니다!