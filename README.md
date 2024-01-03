# Ready Player Me Unity SDK - WebView Module

[![GitHub release (latest SemVer)](https://img.shields.io/github/v/release/readyplayerme/rpm-unity-sdk-webview)](https://github.com/readyplayerme/rpm-unity-sdk-webview/releases/latest) [![GitHub Discussions](https://img.shields.io/github/discussions/readyplayerme/rpm-unity-sdk-webview)](https://github.com/readyplayerme/rpm-unity-sdk-webview/discussions)

**Ready Player Me WebView** is an extension to www.readyplayer.me avatar platform and is an optional part of the Ready Player Me Unity SDK, which helps you load and display avatars created on the website.

Please visit the online documentation and join our public `discord` community.

![](https://i.imgur.com/zGamwPM.png) **[Online Documentation]( https://readyplayer.me/docs )**

![](https://i.imgur.com/FgbNsPN.png) **[Discord Channel]( https://discord.gg/9veRUu2 )**

:octocat: **[GitHub Discussions]( https://github.com/readyplayerme/rpm-unity-sdk-webview/discussions )**

## How to install

The WebView module is managed and installed automatically with the [Unity Core](https://github.com/readyplayerme/Unity-Core) module.
Please refer to the **[Quick Start guide]( https://github.com/readyplayerme/Unity-Core#readme )** for instructions on how to install the Unity Core module.


Users can create Ready Player Me avatars seamlessly in a WebView displayed within a Unity application.

## Prerequisites

**Ready Player Me Core:** You need the Unity Ready Player Me Core for Unity installed in your project to retrieve avatars. See the [Quickstart guide](https://docs.readyplayer.me/ready-player-me/integration-guides/unity/quickstart)
for instructions.

**Deploying the app.** In order to test your WebView app, you have to deploy it to a physical or virtual device. See the Unity documentation on how to do that.

**Required permissions.** The Ready Player Me WebView module requires the following permissions to enable all features:
- Camera access
- Microphone access
- Local storage access
- Photo Gallery access
- Hardware acceleration
- AllowBackup

We provide a script that will automatically add these permissions called WebViewBuildPostprocessor.cs which is is enabled by default. 
To disable this just add the following define to your project via the player settings for Android and iOS build targets: `RPM_DISABLE_WEBVIEW_PERMISSIONS`

## Project setup (Android and iOS)

Creating a Scene with a WebView in your Unity project is the same for Android and iOS.

1. Create or open your Unity project.
2. Import the Ready Player Me Core package into your project, if you haven't done so already.
3. Navigate to the Unity package manager by going to Window > Package Manager.
4. Find the Ready Player Me WebView package under Packages - Ready Player Me.
5. Find and import the WebView Sample by clicking the Import button.
6. Now open the WebView example scene by finding it in the Projects tab under `Samples/Ready Player Me WebView/\[VERSION_NUMBER\]/WebView`.
7. In the Hierarchy tab, you will find a WebView Canvas object with a child object called WebView Panel, select the WebViewPanel.
8. In the inspector you will find a WebViewPanel script with some adjustable settings.
   - **Screen padding:** For adjusting the padding of the Panel UI. 
   - **Url Config:** By adjusting this you can control the behaviour of the RPM Avatar creator. 
   - **On Avatar Created:** This Event can be used to retrieve the avatar URL after finishing the avatar creation process. 
9. Open the Build Settings to set up deployment for your chosen platform. 
   - don't forget to add the scene to the build settings.

_NOTE: The WebView example scene will not load an avatar into the unity scene, it will just return and display the avatar URL on the screen. To load an avatar in the screen you can create an avatar loader script make use of the OnAvatarCreated event on the WebViewPanel._

## Deploy on Android 

1. In Build Settings, set the Platform to Android. 
2. Check Development Build. 
3. Click Player Settings.... 
4. Find `Player > Other Settings > Identification`.
   - Check Override Default Package Name.
   - Set a unique Package Name in the format com.YourCompanyName.YourProductName.
5. Find `Player > Other Settings > Under Configuration > Camera Usage Description` and put some descriptive text in this mandatory field.
6. Find `Player > Other Settings > Under Configuration > Microphone Usage Description` and put some descriptive text in this mandatory field. 
7. Close the Project Settings. 
8. On your device, turn on USB debugging in your Developer Options settings. 
9. Connect your device to your computer. 
10. Click Build and Run.
11. Once the app opens on your device, click the button. Give permissions, and off you go. 

**_Alternatively, you can build the APK and deploy it on your own.
For release builds, see the Unity and Android documentation._**

## Deploy on iOS
1. In Build Settings, set the Platform to iOS.
2. Select Debug and check Development build.
3. Find `Player > Other Settings > Identification`.
   - Check Override Default Package Name. 
   - Before you build your Project for iOS, make sure that you set the Bundle Identifier.
   - Set a Package Name in the format `com.YourCompanyName.YourProductName`.
   - Fill in the `Signing Team ID` (not required for Debug builds to complete).
   - You can also choose whether your app targets the simulator or an actual device. To do this, change the SDK version** >> Target SDK to Simulate SDK or Device SDK.
4. Find `Player > Other Settings > Under Configuration > Camera Usage Description` and put some descriptive text in this mandatory field.
5. Find `Player > Other Settings > Under Configuration > Microphone Usage Description` and put some descriptive text in this mandatory field.
6. Close Project Settings.
7. Click Build.
8. In the file explorer, find your Builds folder and in it the `Unity-iPhone.xcodeproj`.


## WebView 2.0 update

As of WebView 2.0 a number of changes have been made to isolate the WebView module from the rest of the Ready Player Me Unity SDK.

This means that the classes and logic required to work with an iFrame (AKA a WebView), that is running Web Avatar Creator have now been moved to the Ready Player Me Core module. 
This change enables developers to utilize API's for working with any iFrame, without requiring the installation of our WebView package.

As such the following classes are now located in the Ready Player Me Core module:
- WebViewMessageHelper
- UrlConfig
- WebMessage
- MessagePanel
- WebMessageHelper
- WebViewEvents

If you have any scripts in your Unity project that reference these classes you will need to update the namespace to use ReadyPlayerMe.Core instead of ReadyPlayerMe.WebView, be sure to update the import statements in these scripts.
EG: `using ReadyPlayerMe.WebView;` should be changed to `using ReadyPlayerMe.Core;`