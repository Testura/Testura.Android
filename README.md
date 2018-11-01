![Testura Logo](http://testura.net/Content/Images/logo2.png)

Testura.Android is a lightweight test automation framework for Android built in C#. It contains tools and help classes to test, validate and interact with your Android device or emulator.

## Why should I use Testura?
 
- Quick and easy to set up
- Designed to be used with the page object pattern
- Interact with multiple objects at the same time
- Makes it easy to run tests with multiple devices
- Add custom extensions to handle pop-ups, error dialogs, loading bars, etc without any overhead.
- Help tools to create your page objects ([seperate project](https://github.com/Testura/Testura.Android.PageObjectCreator))


# Install

## Prerequisites

- [Android adb.exe (download SDK platform-tools)](https://developer.android.com/studio/releases/platform-tools.html) (also recommended to add adb to environment variables)


## NuGet [![NuGet Status](https://img.shields.io/nuget/v/Testura.Android.svg?style=flat)](https://www.nuget.org/packages/Testura.Android)

[https://www.nuget.org/packages/Testura.Android](https://www.nuget.org/packages/Testura.Android)
    
    PM> Install-Package Testura.Android

## Usage

Testura.Android has been designed for the page object pattern. Here is a short example of a login view:

```c#
using Testura.Android.Device;
using Testura.Android.Device.Configurations;
using Testura.Android.Device.Ui.Objects;
using Testura.Android.Device.Ui.Search;

namespace Testura.Android.Tests.Device
{
    public class ExampleView : View
    {
	// All UiObjects with the "MapUiObject" attribute will be automatically initialized as long as your class 
	// inherit from the "View" class or if you call on "ViewFactory.MapUiNodes(..)"
	[MapUiObject(ResourceId = "usernameTextbox")]
        private readonly UiObject _usernameTextbox;
		
	[MapUiObject(ContentDesc = "passwordTextbox")]
        private readonly UiObject _passwordTextbox;
		
        private readonly UiObject _logInButton;

        public ExampleView(IAndroidDevice device) : base(device)
        {
	    // It is also possible to skip the attribute and initialize the object from the constructor (this is also required for the "lambda" mapping)
            _logInButton = device.MapUiObject(Where.Lambda(node => node.Text == "Login"));
        }

        public void Login(string username, string password)
        {
            _usernameTextbox.InputText(username);
            _passwordTextbox.InputText(password);
            _logInButton.Tap();
        }
    }
}
```

In the example we can see how we map different nodes on the screen to "UI objects".


## Map `UiObject` with `Where` 

If you have used Selenium, Appium or any other big test automation framework I'm sure you are familiar with the `By` keyword. Testura.Android has something similiar called `Where`.

When mapping up a new UI object, you first have to decide how to find it, using one or more `Where`:

- `Where.Text` - Find nodes that match the exact text
- `Where.ContainsText` - Find nodes that contain just a part of the text
- `Where.ResourceId` - Find nodes that contains the exact resource ID
- `Where.ContentDesc` - Find nodes that contains the exact content description
- `Where.Class` - Find nodes that contains the exact class
- `Where.Index` - Find nodes that have this index
- `Where.Package` - Find nodes that contains the exact package
- `Where.Lamba` - Find nodes with a lamba expression

`Where.Lamba` is a powerful method to find nodes and you can access both the node's parent and children:

```c#
Where.Lambda(n =>
    n.Text == "Some text"
    && n.Parent != null
    && n.Children.First().ContentDesc == "My child");
```

### Advanced mapping with wildcards 

In some cases we don't know the whole value when mapping so to get passed this Testura.Android provide "wildcards": 

```c#
[MapUiObject(Class = "textbox", ResourceId = Where.Wildcard)]
private readonly UiObject _usernameTextbox;
```

and later on we provide the wildcard value: 

```c#
// This will input text into textbox where class = "textbox" and resourceId = "user"
_usernameTextbox["user"].InputText(".."); 
```

## Services

The Android device class consists of multiple "services" that handle different parts of the device:

- ADB service - Send shell commands, push and pull files, install APKs etc.
- Settings service - Enable/disable different settings, for example wifi and airplane mode
- Activity service - Start activity or get the name of the current one
- Interaction service - Handle interaction with the device, for example swipe and click

All services are accessible from the device class like this:

```c#
var device = new AndroidDevice();
device.Adb.Shell("my shell commando");
device.Settings.Wifi(State.Enable);
```

## `UiObject`

`UiObject` is one of the core components of Testura.Android. It wraps a node with all the necessary functions:

```c#
var device = new AndroidDevice();
var uiObject = device.MapUiObject(Where.ContentDesc(".."));
uiObject.Tap();
uiObject.IsVisible();
uiObject.IsHidden();
uiObject.InputText("..");
uiObject.WaitUntil(node => node.Enabled);
var node = uiObject.First();
```

## Run tests in parallel with multiple devices 

In many cases you want to run your tests on multiple devices in parallel to save time and Testura.Android can help you with that. Simply use the `AndroidDeviceFactory` class. 

In your setup: 

```c#
var deviceFactory = new AndroidDeviceFactory();
var device = deviceFactory.GetDevice(new DeviceConfiguration { Serial = ".." }, new DeviceConfiguration { Serial = ".." });
```

After your test: 

```c#
deviceFactory.DisposeDevice(device);
```

Here we simply provide all possible devices to the `GetDevice` method and let Testura.Android keep track of the queue. When we finished our test we call on `DisposeDevice` so next test in the queue can run.

## Other features 

### Logcat watcher

Testura.Android provides functionally to watch the logcat for different tags. To use it you simply have to extend the `LogcatWatcher` class or use the `EventLogcatWatcher`.

#### `EventLogcatWatcher`

`EventLogcatWatcher` will fire an event every time it find something that matches the provided tag(s). 

```c#
public void StartLogcatWatcher(IAndroidDevice device)
{
	var eventLogcatWatcher = new EventLogcatWatcher(device, new[] {"myTag "}, flushLogcat: true);
	eventLogcatWatcher.NewOutputEvent += EventLogcatWatcherOnNewOutputEvent;
}

private void EventLogcatWatcherOnNewOutputEvent(object sender, string line)
{
	// Found a new line with the tag, handle it.
}
```

### UI Extensions

Ui extensions is a way to hijack the UIService so *before* we look for element x we do y. An example of this could be looking for dialogs, error message or loading bars.

#### Example when we wait for a loading bar to disappear

```c#
    public class UiLoadingExtension : IUiExtension
    {
        // A flag to temporary disable this extension 
        private bool _isWaiting;
        private readonly UiObject _loadingBar;

        private UiLoadingExtension(IAndroidUiMapper device)
        {
            _loadingBar = device.MapUiObject(With.ResourceId("loadingId"));
        }

        public bool CheckNodes(IList<Node> nodes)
        {
            if (nodes.Any(n => With.ResourceId("loadingId").NodeMatch(n, null)) && !_isWaiting)
            {
                WaitForLoading();
                // Reset the wait timer for the actual object we're looking for. 
                return true;
            }
            return false; 
        }

        private void WaitForLoading()
        {
            _isWaiting = true;
            _loadingBar.IsHidden(180);
            _isWaiting = false;
        }
    }
```

And then you add this to your device ui service

```c#
var device = new AndroidDevice();
device.Ui.Extensions.Add(new UiLoadingExtension());
```

### Record screen 

```c#

var screenRecording = device.Adb.RecordScreen();
.. Perform actions ...
screenRecording.StopRecording("path/to/save");
```

## Page object helpers

For more information about page object helpers, visit the wiki: https://github.com/Testura/Testura.Android/wiki/Page-Object

We also have a help tool to generate page objects: https://github.com/Testura/Testura.Android.PageObjectCreator

## Documentation 

- Wiki - [https://github.com/Testura/Testura.Android/wiki](https://github.com/Testura/Testura.Android/wiki)
- API [https://testura.github.io/Android/api/index.html](https://testura.github.io/Android/api/index.html)

## License

This project is licensed under the MIT License. See the [LICENSE.md](LICENSE.md) file for details.

## Contact

Visit <a href="http://www.testura.net">www.testura.net</a>, twitter at @GameSpeedCoding
