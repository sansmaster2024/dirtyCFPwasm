typedef unsigned char byte;

void sans(byte* s);
int* createFBI(int width, int height);
void deleteFBI(int fbiID);
void bindFramebufferInfo(int fbi);
void drawTexture(int tex, int shader, byte* uniformKeys, float** uniformValues, byte* uniformValueCounts, int uniformLen, int pass);
void drawTextureBasic(int tex);
int findShader(byte* shader);
int findImage(byte* image);
void clearFBI(int fbiID);
void drawBloom(int src, int dst);
void drawCameraMotionBlur(int src, int dst);
void drawPenTile(int src, int dst);