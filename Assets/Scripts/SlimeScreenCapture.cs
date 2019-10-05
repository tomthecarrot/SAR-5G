using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;
using Teleportal;

public class SlimeScreenCapture : MonoBehaviour {
    private RenderTexture renderTexture;
    private float NetworkInterval = 1f;

    IEnumerator Start() {
        while (!Teleportal.Teleportal.tp.IsConnected()) {
            // Wait for connection
            yield return new WaitForSeconds(0.2f);
        }

        while (Teleportal.Teleportal.tp.IsConnected()) {
            // Read screen image into render texture
            this.renderTexture = new RenderTexture(Screen.width, Screen.height, 0);
            ScreenCapture.CaptureScreenshotIntoRenderTexture(this.renderTexture);
            
            // Async read bytes operation
            AsyncGPUReadback.Request(this.renderTexture, 0, TextureFormat.RGBA32, this.ReadbackCompleted);
            
            // Wait for a new frame to be ready, then the specified delay
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(this.NetworkInterval);
        }
    }

    private void ReadbackCompleted(AsyncGPUReadbackRequest request) {
        // Free up memory
        DestroyImmediate(this.renderTexture);

        using (var imageBytes = request.GetData<byte>()) {
            string byteStr = Encoding.ASCII.GetString(imageBytes.ToArray());

            Debug.Log("image byte str:");
            Debug.Log(byteStr);
        }
    }

}