using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices.JavaScript;

namespace WebAssembly
{
    public static class Runtime
    {
        public static int tt()
        {
            return 33;
        }
        public static string InvokeJS(string str)
        {
            return Interop.InvokeJS(str);
        }
    }

    static partial class Interop
    {
        [JSImport("globalThis.invokeJS")]
        public static partial string InvokeJS(string value);
    }
}