using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using Teleportal;

public class SlimeScreenCapture : MonoBehaviour {

    public float NetworkInterval = 1f;
    public static SlimeScreenCapture Shared;

    public int width = 100;
    public int height = 100;
    public int divisions = 10;
    public float divisionInterval = 0.01f;
    private RenderTexture renderTexture;
    private Texture2D t2d;
    private Rect rectRead;

    void Awake() {
        SlimeScreenCapture.Shared = this;
    }

    IEnumerator Start() {
        // this.width = Screen.width;
        // this.height = Screen.height;

        int divWidth = this.width / divisions;
        int divHeight = this.height / divisions;

        while (!Teleportal.Teleportal.tp.IsConnected() || !Teleportal.AuthModule.Shared.IsAuthed()) {
            // Wait for connection
            yield return new WaitForSeconds(0.2f);
        }

        while (Teleportal.Teleportal.tp.IsConnected()) {
            // Read screen image into render texture
            this.renderTexture = new RenderTexture(this.width, this.height, 0, RenderTextureFormat.ARGB32);
            ScreenCapture.CaptureScreenshotIntoRenderTexture(this.renderTexture);
            
            // Async read bytes operation
            // AsyncGPUReadback.Request(this.renderTexture, 0, TextureFormat.ARGB32, this.ReadbackCompleted);

            RenderTexture.active = this.renderTexture;

            List<byte[]> byteArrs = new List<byte[]>();
            int byteArrLen = 0;

            for (int i = 0; i < divisions; i++) {
                yield return new WaitForEndOfFrame();

                this.t2d = new Texture2D(this.width, this.height, TextureFormat.ARGB32, false);
                this.rectRead = new Rect(divWidth * i, divHeight * i, divWidth, divHeight);
                this.t2d.ReadPixels(this.rectRead, 0, 0);
                this.t2d.Compress(false);
                this.t2d.Apply();

                byte[] tmp = t2d.GetRawTextureData<byte>().ToArray();
                byteArrs.Add(tmp);
                byteArrLen += tmp.Length;
                yield return new WaitForSeconds(this.divisionInterval);
            }

            byte[] ba = new byte[byteArrLen];
            for (int i = 0; i < byteArrs.Count; i++) {
                byte[] tmp = byteArrs[i];
                for (int j = 0; j < tmp.Length; j++) {
                    ba[i + j] = tmp[j];
                }
            }

            Thread t = new Thread(this.ByteArrayToString);
            t.Start(ba);
            
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
        // Debug.Log("image byte str:");
        // Debug.Log(hexStr);

        UnityMainThreadDispatcher.Instance().Enqueue(this.SetStateIE(hexStr));
        // return hexStr;
    }

    private IEnumerator SetStateIE(string hexStr) {
        SlimeManager.Shared.tpo.SetState("cam", Teleportal.Teleportal.tp.GetUsername() + ":" + hexStr);

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