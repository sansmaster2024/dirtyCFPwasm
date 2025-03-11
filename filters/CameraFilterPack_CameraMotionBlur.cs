///////////////////////////////////////////
//  CameraFilterPack - by VETASOFT 2020 ///
///////////////////////////////////////////

using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class CameraFilterPack_CameraMotionBlur : MonoBehaviour {
    void Start () 
    {
    }
    void OnRenderImage (RenderTexture sourceTexture, RenderTexture destTexture)
    {
        UnoWasm.JSInterop.drawCameraMotionBlur(sourceTexture.fbiID, destTexture.fbiID);
    }
}