using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GodModeUI : MonoBehaviour
{
    public static GodModeUI Shared;
    private GameObject cameraObj;

    void Awake() {
        GodModeUI.Shared = this;

        this.cameraObj = Camera.main.gameObject; // get current camera (TP)
        this.gameObject.SetActive(false);
    }

    void OnEnable() {
        this.cameraObj.active = false;
    }

    void OnDisable() {
        this.cameraObj.active = true;
    }
}
