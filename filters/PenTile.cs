///////////////////////////////////////////
//  CameraFilterPack - by VETASOFT 2020 ///
///////////////////////////////////////////

using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class PenTile : MonoBehaviour {
    void Start () 
    {
    }
    void OnRenderImage (RenderTexture sourceTexture, RenderTexture destTexture)
    {
        UnoWasm.JSInterop.drawPenTile(sourceTexture.fbiID, destTexture.fbiID);
    }
}