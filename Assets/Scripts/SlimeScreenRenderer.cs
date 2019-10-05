using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using Teleportal;

[RequireComponent(typeof(RawImage))]
public class SlimeScreenRenderer : MonoBehaviour {
    private TPObject tpo;
    private RawImage viz;
    private float NetworkInterval = 1f;

    void Awake() {
        this.viz = this.gameObject.GetComponent<RawImage>();
    }

    void Start() {
        TPUser tpu = this.transform.parent.parent.gameObject.GetComponent<TPUser>();

        if (tpu.isSelfUser) {
            Destroy(this.gameObject);
            return;
        }

        this.tpo = TPObject.get(tpu);
        this.tpo.Subscribe("cam", delegate (string byteStr) {
            byte[] bytes = SlimeScreenCapture.StringToByteArray(byteStr);
            Texture2D tex = new Texture2D(100, 100, TextureFormat.ARGB32, false);
            tex.LoadRawTextureData(bytes);
            tex.Apply();
            viz.texture = tex;
        });
    }

}