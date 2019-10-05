using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;
using Teleportal;

public class SlimeScreenRenderer : MonoBehaviour {
    private RenderTexture renderTexture;
    private float NetworkInterval = 1f;

    void Start() {
        TPObject tpo = TPObject.get(this.transform.parent.gameObject);
        tpo.Subscribe("cam", delegate (string byteStr) {
            byte[] bytes = Encoding.ASCII.GetBytes(byteStr);

        });
    }

}