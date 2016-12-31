![Testura Logo](http://testura.net/Content/Images/logo.png)

Testura.Android is a lightweight test automation framework for Android built in C#. It contains tools and help classes to test, validate and interact with your Android device or emulator.


# Install

## Prerequisites

- [Android SDK](https://developer.android.com/studio/index.html)
- Visual Studio


## NuGet [![NuGet Status](https://img.shields.io/nuget/v/Testura.Android.svg?style=flat)](https://www.nuget.org/packages/Testura.Android)

[https://www.nuget.org/packages/Testura.Android](https://www.nuget.org/packages/Testura.Android)
    
    PM> Install-Package Testura.Android
   
## Nuget with page object helpers (optional) [![NuGet Status](https://img.shields.io/nuget/v/Testura.Android.PageObject.svg?style=flat)]

[https://www.nuget.org/packages/Testura.Android.PageObject](https://www.nuget.org/packages/Testura.Android.PageObject)
    
    PM> Install-Package Testura.Android.PageObject

## Usage

[Introduction video here](https://www.youtube.com/watch?v=x-U2F6mzcyc)

Testura.Android has been designed for the page object pattern. Here is a short example of a login view:

```c#
using Testura.Android.Device;
using Testura.Android.Device.Configurations;
using Testura.Android.Device.Ui.Objects;
using Testura.Android.Device.Ui.Search;

namespace Testura.Android.Tests.Device
{
    public class ExampleView
    {
        private IAndroidDevice _androidDevice;

        private readonly UiObject _usernameTextbox;
        private readonly UiObject _passwordTextbox;
        private readonly UiObject _logginButton;

        public ExampleView()
        {
            _androidDevice = new AndroidDevice(new DeviceConfiguration());

            // The device won't look for the node/UI object before we interact with it,
            // so it's perfectly safe to set up everything inside the constructor.
            _usernameTextbox = _androidDevice.Ui.CreateUiObject(With.ResourceId("usernameTextbox"));
            _passwordTextbox = _androidDevice.Ui.CreateUiObject(With.ContentDesc("passwordTextbox"));
            _logginButton = _androidDevice.Ui.CreateUiObject(With.Lambda(n => n.Text == "Login"));
        }

        public void Login(string username, string password)
        {
            _usernameTextbox.SendKeys(username);
            _passwordTextbox.SendKeys(password);
            _logginButton.Click();
        }
    }
}
```

In the example we can see how we map different nodes on the screen to "UI objects". We can then interact with the nodes through the mapped UI object.

## Selecting nodes with `With`

If you have used Selenium, Appium or any other big test automation framework I'm sure you are familiar with the `By` keyword. Testura.Android has something similiar called `With`.

When mapping up a new UI object, you first have to decide how to find it, using one or more `With`:

- `With.Text` - Find nodes that match the exact text
- `With.ContainsText` - Find nodes that contain just a part of the text
- `With.ResourceId` - Find nodes that contains the exact resource ID
- `With.ContentDesc` - Find nodes that contains the exact content description
- `With.Class` - Find nodes that contains the exact class
- `With.Index` - Find nodes that have this index
- `With.Lamba` - Find nodes with a lamba expression

`With.Lamba` is a powerful method to find nodes, and you can access both the node's parent and children:

```c#
With.Lambda(n =>
    n.Text == "Some text"
    && n.Parent != null
    && n.Children.First().ContentDesc == "My child");
```

## Services

The Android device class consists of multiple "services" that handle different parts of the device:

- ADB service - Send shell commands, push and pull files, install APKs etc.
- UI service - Look for nodes or map UI objects (handles screen dumping)
- Settings service - Enable/disable different settings, for example wifi and airplane mode
- Activity service - Start activity or get the name of the current one
- Interaction service - Handle interaction with the device, for example swipe and click

All services are accessible from the device class like this:

```c#
var device = new AndroidDevice(new DeviceConfiguration());
device.Adb.Shell("my shell commando");
device.Settings.Wifi(State.Enable);
```

## `UiObject`

`UiObject` is one of the core components of Testura.Android. It wraps a node with all the necessary functions:

```c#
var device = new AndroidDevice(new DeviceConfiguration());
var uiObject = device.Ui.CreateUiObject(With.ContentDesc(".."));
uiObject.Click();
uiObject.IsVisible();
uiObject.IsHidden();
uiObject.SendKeys("..");
var node = uiObject.Values();
```

## Page object helpers

For more information about page object helpers, visit the wiki: https://github.com/Testura/Testura.Android/wiki/10.-Page-Object

## License

This project is licensed under the MIT License. See the [LICENSE.md](LICENSE.md) file for details.

## Contact

Visit <a href="http://www.testura.net">www.testura.net</a>
