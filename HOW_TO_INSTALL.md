# Quick Start

### Requirements
- Unity Version 2020.3 or higher
- [Git](https://git-scm.com) needs to be installed to fetch the Unity package. [Download here](https://git-scm.com/downloads)

**1.** To add the new Ready Player Me Unity Webview to your project you can use the Unity Package Manager to import the package directly from the Git URL.

**2.** With your Unity Project open, open up the Package Manager window by going to `Window > Package Manager`.

![open-package-manager](https://user-images.githubusercontent.com/7085672/206432665-da233187-06ad-40b5-a25e-660c97d6726f.png)

**3.** In the **Package Manager** window click on the + icon in the top left corner and select Add Package From Git URL.

![add-package-from-ur;](https://user-images.githubusercontent.com/7085672/206432698-8ecde741-4259-486f-9c77-d63fbc9a6cde.png)

**4.** Paste in this url

`https://github.com/readyplayerme/rpm-unity-sdk-webview.git`

![paste-git-url](https://user-images.githubusercontent.com/7085672/206432731-f9e0d161-7843-4d6e-8851-47b1f3bfb3bc.png)

**5.** Click add and wait for the import process to finish.

After the process is complete you project will have imported these packages:

- **Ready Player Me WebView**

![image](https://github.com/readyplayerme/rpm-unity-sdk-webview/assets/107070960/0bb85ced-2b32-462e-bdee-b53a84f31a9d)

## Alternate Installation

### Using Git URL

1. Navigate to your project's Packages folder and open the manifest.json file.
2. Add this line below the `"dependencies": {` line
    - ```json title="Packages/manifest.json"
      "com.readyplayerme.core": "https://github.com/readyplayerme/rpm-unity-sdk-webview.git",
      ```
3. UPM should now install the package.
