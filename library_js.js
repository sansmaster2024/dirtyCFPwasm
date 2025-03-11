
mergeInto(LibraryManager.library, {
  sans: function(sans) {
   console.log(UTF8ToString(sans));
  },
  createFBI: function(width, height){
    return globalThis.advancedFilter.createFBINative(width,height);
  },
  deleteFBI: function(fbiID){
    return globalThis.advancedFilter.deleteFBI(fbiID);
  },
  bindFramebufferInfo: function(fbi){
    return globalThis.advancedFilter.bindFramebufferInfo(fbi);
  },
  drawTexture: function(tex, shader, uniformKeys, uniformValues, uniformValueCounts, uniformLen, pass){
    return globalThis.advancedFilter.drawTexture(tex, shader, uniformKeys, uniformValues, uniformValueCounts, uniformLen, pass);
  },
  drawTextureBasic: function(tex){
    return globalThis.advancedFilter.drawTextureBasic(tex);
  },
  findShader: function(name){
    return globalThis.advancedFilter.findShader(UTF8ToString(name));
  },
  findImage: function(name){
    return globalThis.advancedFilter.findImage(UTF8ToString(name));
  },
  clearFBI(fbiID){
    globalThis.advancedFilter.clearFBI(fbiID);
  },
  drawBloom: function(src, dst){
    globalThis.advancedFilter.drawBloom(src, dst);
  },
  drawCameraMotionBlur: function(src, dst){
    globalThis.advancedFilter.drawCameraMotionBlur(src, dst);
  },
  drawPenTile: function(src, dst){
    globalThis.advancedFilter.drawPenTile(src, dst);
  },
 
});
