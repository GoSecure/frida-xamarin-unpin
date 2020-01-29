/* Xamarin/Android HttpClient generic certificate pinning bypass.
 *
 * @author   Alexandre "alxbl" Beaulieu <abeaulieu@gosecure.net>
 * @release  Jan 28th 2020
 *
 * @description
 *
 * This script is a generic certificate pinning bypass for Android applications
 * that use Xamarin with Mono.
 *
 * There are two methods to override the server certificate validation step in .NET
 * depending on whether the classic .NET API is being used (`ServicePointerManager`)
 * or the .NET Core APIs are being used (`HttpClient.HttpClientHandler`).
 *
 * In the .NET Core case, the HttpClient's `SendAsync` implementation is hooked to 
 * inject * a default HttpClientHandler that does not perform pinning. 
 *
 * [TODO] In the .NET Framework case, the System.Net.ServicePointerManager's is hooked
 *        to always return NULL and forcefully set to NULL in order to reset it.
 *
 * @note  Validation still happens so the certificate must be valid.
 */

import { MonoApiHelper, MonoApi } from 'frida-mono-api'
const mono = MonoApi.module

// Locate System.Net.Http.dll
let status = Memory.alloc(0x1000);
let http = MonoApi.mono_assembly_load_with_partial_name(Memory.allocUtf8String('System.Net.Http'), status);
let img = MonoApi.mono_assembly_get_image(http);

// Construct a default HttpClientHandler to inject in HttpMessageInvoker instances.
let kHandler = MonoApi.mono_class_from_name(img, Memory.allocUtf8String('System.Net.Http'), Memory.allocUtf8String('HttpClientHandler'));
let ctor = MonoApiHelper.ClassGetMethodFromName(kHandler, 'CreateDefaultHandler');
let pClientHandler = MonoApiHelper.RuntimeInvoke(ctor, NULL); // Static method -> instance = NULL.
console.log(`[+] Created Default HttpClientHandler @ ${pClientHandler}`);

// Hook HttpMessageInvoker.SendAsync
let kInvoker = MonoApi.mono_class_from_name(img, Memory.allocUtf8String('System.Net.Http'), Memory.allocUtf8String('HttpMessageInvoker'));

console.log('[+] Hooking HttpMessageInvoker.SendAsync');
MonoApiHelper.Intercept(kInvoker, 'SendAsync', {
  onEnter: (args) => {
    console.log(`[*] HttpClientHandler.SendAsync called`);

    let self = args[0];
    let handler = MonoApiHelper.ClassGetFieldFromName(kInvoker, '_handler');
    let cur = MonoApiHelper.FieldGetValueObject(handler, self);

    if (cur.equals(pClientHandler)) return; // Already bypassed.

    MonoApi.mono_field_set_value(self, handler, pClientHandler);
    console.log(`[+]   Replaced with default handler @ ${pClientHandler}`);
  }
});

console.log('[+] Done!\nMake sure you have a valid MITM CA installed on the device and have fun.');
