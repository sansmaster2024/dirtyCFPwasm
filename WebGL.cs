using System.Runtime.InteropServices.JavaScript;
using System.Runtime.InteropServices;

namespace UnoWasm
{
    public static unsafe class JSInterop
    {

        [DllImport("Test")]
        public static extern void sans(byte* s);
        [DllImport("Test")]
        public static extern int* createFBI(int width, int height);
        [DllImport("Test")]
        public static extern void deleteFBI(int fbiID);
        [DllImport("Test")]
        public static extern void bindFramebufferInfo(int fbi);

        [DllImport("Test")]
        public static extern void drawTexture(int tex, int shader, byte* uniformKeys, float[]* uniformValues, byte* uniformValueCounts, int uniformLen, int pass);
        [DllImport("Test")]
        public static extern void drawTextureBasic(int tex);
        [DllImport("Test")]
        public static extern int findShader(byte* shaderName);
        [DllImport("Test")]
        public static extern int findImage(byte* ImageName);
        [DllImport("Test")]
        public static extern void drawBloom(int src, int dst);
        [DllImport("Test")]
        public static extern void drawCameraMotionBlur(int src, int dst);
        [DllImport("Test")]
        public static extern void drawPenTile(int src, int dst);
        [DllImport("Test")]
        public static extern void clearFBI(int fbiID);
        
    }
}
