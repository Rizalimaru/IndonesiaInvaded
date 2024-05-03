using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScreenShotHandler : MonoBehaviour
{
    private static ScreenShotHandler instance;

    private Camera myCamera;
    private bool takeScreenShotOnNextFrame;

    private void Awake(){
        instance = this;
        myCamera = gameObject.GetComponent<Camera>();
    }

    private void OnPostRender()
    {
        if(takeScreenShotOnNextFrame){
            takeScreenShotOnNextFrame = false;
            RenderTexture renderTexture = myCamera.targetTexture;   

            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            renderResult.ReadPixels(rect, 0, 0);

            byte[] byteArray = renderResult.EncodeToPNG();
            File.WriteAllBytes(Application.dataPath + "/Screenshot.png", byteArray);
            Debug.Log("Screenshot saved");

            RenderTexture.ReleaseTemporary(renderTexture);
            myCamera.targetTexture = null;

        }
    }

    private void TakeScreenShot(int width, int height){
        myCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
        takeScreenShotOnNextFrame = true;
    }

    public static void TakeScreenShot_Static(int width, int height){
        instance.TakeScreenShot(width, height);
    }
}
