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

        SlimeManager.Shared.AddScreen(tpu.GetUsername(), this);
    }

    public void SetTexture(Texture2D newTexture) {
        this.viz.texture = newTexture;
    }

}