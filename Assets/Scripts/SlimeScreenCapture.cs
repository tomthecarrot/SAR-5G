using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using Teleportal;

public class SlimeScreenCapture : MonoBehaviour {

    public float NetworkInterval = 1f;

    private RenderTexture renderTexture;

    IEnumerator Start() {
        while (!Teleportal.Teleportal.tp.IsConnected()) {
            // Wait for connection
            yield return new WaitForSeconds(0.2f);
        }

        while (Teleportal.Teleportal.tp.IsConnected()) {
            yield return new WaitForEndOfFrame();

            // Read screen image into render texture
            this.renderTexture = new RenderTexture(/*Screen.width, Screen.height*/100, 100, 0, RenderTextureFormat.ARGB32);
            ScreenCapture.CaptureScreenshotIntoRenderTexture(this.renderTexture);
            
            // Async read bytes operation
            AsyncGPUReadback.Request(this.renderTexture, 0, TextureFormat.ARGB32, this.ReadbackCompleted);
            
            // Wait for the specified delay
            yield return new WaitForSeconds(this.NetworkInterval);
        }
    }

    private async void ReadbackCompleted(AsyncGPUReadbackRequest request) {
        // Free up memory
        DestroyImmediate(this.renderTexture);

        byte[] imageBytes = request.GetData<byte>().ToArray();

        Debug.Log("serializing to string...");

        Thread t = new Thread(this.ByteArrayToString);
        t.Start(imageBytes.ToArray());
        
        // string byteStr = /*await */SlimeScreenCapture.ByteArrayToString(imageBytes.ToArray());
        // Debug.Log("image byte str:");
        // Debug.Log(byteStr);
    }

    // HELPERS //

    public /*async Task<string>*/ void ByteArrayToString(object data) {
        byte[] ba = (byte[]) data;
        StringBuilder hex = new StringBuilder(ba.Length * 2);
        Debug.Log(ba.Length);
        foreach (byte b in ba) {
            hex.AppendFormat("{0:x2}", b);
            // await Task.Delay(1);
        }

        string hexStr = hex.ToString();
        UnityMainThreadDispatcher.Instance().Enqueue(this.SetStateIE(hexStr));

        // Debug.Log("image byte str:");
        // Debug.Log(hexStr);
        // return hexStr;
    }

    private IEnumerator SetStateIE(string hexStr) {
        SlimeManager.Shared.tpo.SetState("cam", Teleportal.Teleportal.tp.GetUsername() + ";" + hexStr);

        /*
        TPObject tpo = TPUser.SelfUser.GetObject();
        Debug.Log(tpo);
        if (tpo != null) {
            tpo.SetState("cam", hexStr);
        }
        */
        
        yield return null;
    }

    public static byte[] StringToByteArray(string hex) {
        return Enumerable.Range(0, hex.Length)
                        .Where(x => x % 2 == 0)
                        .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                        .ToArray();
    }

}