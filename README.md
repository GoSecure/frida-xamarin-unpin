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

TODO

## Limitations

- Currently it is impossible to early-instrument an application (using
  `frida -f`) because `frida-mono-api` needs the mono modules to
  already be loaded in memory.

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
many details as possible regarding the application you tried to
instrument.

Pull requests, improvements, bug fixes and additional features are
more than welcome!
