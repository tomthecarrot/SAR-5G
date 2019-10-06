using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using Teleportal;

[RequireComponent(typeof(RawImage))]
public class SlimeScreenRenderer : MonoBehaviour {
    private TPObject tpo;
    private RawImage viz;
    private float NetworkInterval = 1f;
    public bool IsStandalone = false;
    public string StandaloneName = "";

    void Awake() {
        this.viz = this.gameObject.GetComponent<RawImage>();
    }

    void Start() {
        if (this.IsStandalone) {
            SlimeManager.Shared.AddScreen(this.StandaloneName, this);
        } else {
            this.DelayThenAdd();
        }
    }

    private async void DelayThenAdd() {
        TPUser tpu = this.transform.parent.parent.gameObject.GetComponent<TPUser>();

        if (tpu.isSelfUser) {
            Destroy(this.gameObject);
            return;
        }

        await Task.Delay(2000);

        string username = tpu.GetUsername();
        SlimeManager.Shared.AddScreen(username, this);
    }

    public void SetTexture(Texture2D newTexture) {
        this.viz.texture = newTexture;
    }

}