# Xamarin Certificate Pinning Bypass

This is a small Frida script to bypass Mono-based certificate pinning.

The repository includes a sample application with sources and a
prebuilt APK for quick testing.

The fully functional script is also available in `dist/` for quick usage

**NOTE:** Frida requires a rooted Android device.

## Using Pre-built Frida Script

Simply run `frida -U -l dist/xamarin-unpin.js com.your.app` after your
application has launched.


## Technical Details

On recent versions (Xamarin >= 10.x, Mono >= 6.0) of the Mono runtime, the script works by creating a
default `HttpClientHandler` and hooking the `HttpClient` base class'
`SendAsync` method, which is the underlying method for all HTTP
requests. When the method is called, the `HttpClientHandler` is
checked and if it isn't the script-created handler, it replaces it by
the default handler before proceeding with the `SendAsync` logic.

Using this method, it is possible to hook any HTTP request as long as
it is performed with the Xamarin System.Net.Http stack. Furthermore,
it can be performed at any point during the program's lifecycle
and does not require heap scanning.

On older versions (Xamarin < 10.x, Mono < 6.0) of the Mono runtime, it is even simpler because the
certificate validation callback is a static property of the class
`System.Net.ServicePointManager`. This property's `get` and `set`
methods are hooked to always return null and always set null,
respectively. Additionally, the setter is called with `null`
explicitly by the script to remove any handler that may already be
present.

In both cases, the hooking process works by first forcing a JIT of the
target method by the mono runtime using `mono_compile_method()`
followed by a hooking of the native method code.

The script has not yet been tested with tiered compilation, AOT
compilation or full AOT compilation for iOS. We are interested in any
feedback or sample applications to help us implement and debug these
particular scenarios.

## Limitations

- Currently it is impossible to early-instrument an application (using
  `frida -f`) because `frida-mono-api` needs the mono modules to
  already be loaded in memory.
- Full AOT with iOS is not supported yet
- Untested with Android AOT and Tiered compilation

# Developing 

If you want to build the APK or Frida script to play around, follow the instructions below.

## Building and Deploying the APK

- Make sure that the build target is `Release/AnyCPU`
- Right click on the `SampleApp.Android` project and select `Archive...`
- After archival has completed, select the archive entry and click on `Distribute...`
- Select `Ad-Hoc` distribution
- Select your signing identity (create one if needed)
- Save APK to disk
- Uninstall any existing APKs: `adb uninstall com.test.sample`
- Install the newly saved APK `adb install /path/to/com.test.sample.apk`


## Building and Running the Frida Script

- Clone the modified `frida-mono-api` in this repository's root. 
```
git clone https://github.com/GoSecure/frida-mono-api mono-api
cd mono-api && git switch extra
cd ..
```
- Run `npm i && npm run build`
- Ensure you have frida installed (`pip install frida frida-push`)
- Start the test Application on your Android device
- Run the following commands to launch the script
```
frida-push
frida -U com.test.sample -l dist/xamarin-unpin.js --no-pause
```

To test the behavior, navigate in the application to the About page
and hit the "Make HTTP Request" Button with/without the script.
  
## Contributing

If you run into issues while trying to bypass pinning in real
applications, feel free to open an issue with the Frida output and as
much detail as possible regarding the application you tried to
instrument.

Pull requests, improvements, bug fixes and additional features are
more than welcome!
