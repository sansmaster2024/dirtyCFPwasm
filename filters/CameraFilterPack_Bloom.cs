///////////////////////////////////////////
//  CameraFilterPack - by VETASOFT 2020 ///
///////////////////////////////////////////

using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class CameraFilterPack_Bloom : MonoBehaviour {
    void Start () 
    {
    }
    void OnRenderImage (RenderTexture sourceTexture, RenderTexture destTexture)
    {
        UnoWasm.JSInterop.drawBloom(sourceTexture.fbiID, destTexture.fbiID);
    }
}