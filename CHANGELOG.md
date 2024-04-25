# Changelog

All notable changes to this project will be documented in this file.
This project adheres to [Semantic Versioning](http://semver.org/).

## [2.2.1] - 2024.25.04

### Fixed
- feat: update install description by @rk132 in [#37](https://github.com/readyplayerme/rpm-unity-sdk-webview/pull/37)

## [2.2.0] - 2024.24.01

### Updated
- feat: update install description by @rk132 in [#33](https://github.com/readyplayerme/rpm-unity-sdk-webview/pull/33)

## [2.1.3] - 2024.17.01

### Fixed
- fix for deprecated PBXProject function

## [2.1.2] - 2024.11.01

### Fixed
- an issue causing errors when targeting iOS

## [2.1.1] - 2024.09.01

### Fixed
- fixed a flaw in the logic for disabling android build processor
- an error causing android builds to fail

## [2.1.0] - 2024.08.01

### Added
- scripting define symbol `RPM_DISABLE_WEBVIEW_PERMISSIONS` can now be used to disable Android permissions override @harrisonhough in [#26](https://github.com/readyplayerme/rpm-unity-sdk-webview/pull/26)

## [2.0.0] - 2023.20.04

### Updated
- moved core iframe and url logic to Core package @harrisonhough in [#21](https://github.com/readyplayerme/rpm-unity-sdk-webview/pull/21)

## [1.2.1] - 2023.08.14

### Fixed
- fix for missing prefab reference [#18](https://github.com/readyplayerme/rpm-unity-sdk-webview/pull/18)

### Updated
- README.md updated with new guides [#18](https://github.com/readyplayerme/rpm-unity-sdk-webview/pull/18)

## [1.2.0] - 2023.05.29
- added support for account linking for auto login [#14](https://github.com/readyplayerme/rpm-unity-sdk-webview/pull/14) 
- added support for asset unlock events [#15](https://github.com/readyplayerme/rpm-unity-sdk-webview/pull/15)

## [1.1.1] - 2023.04.20

### Updated
- exposed WebViewPanel loaded field to check when canvas is ready [#13](https://github.com/readyplayerme/rpm-unity-sdk-webview/pull/13)

## [1.1.0] - 2023.03.21

### Fixed
- permission popup now shows even if app loses focus

## [1.0.0] - 2023.02.20

### Added
- optional sdk logging
- release git actions

### Updated
- Partner subdomain now comes from CoreSettings object

### Fixed
- Various other bug fixes and improvements

## [0.2.0] - 2023.02.08

### Added
- optional sdk logging
- release git actions

### Updated
- Partner subdomain now comes from CoreSettings object

### Fixed
- Various other bug fixes and improvements

## [0.1.0] - 2023.01.10

### Added
- WebView check to detect outdated browsers
- inline code documentation
- Contribution guide, code of conduct and 3rd party notices
- Added sample from core module

### Updated
- A big refactor of code and classes

### Fixed
- Various bug fixes and improvements
