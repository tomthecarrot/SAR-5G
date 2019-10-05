﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teleportal;

public class SlimeManager : MonoBehaviour
{
    public static SlimeManager Shared;
    public GameObject VoxelTPO;
    public string GodUser = "NULL";
    public bool iAmGod = false;
    public TPObject tpo;

    private Dictionary<string, SlimeScreenRenderer> screens;

    void Awake() {
        SlimeManager.Shared = this;
        this.screens = new Dictionary<string, SlimeScreenRenderer>();
    }

    void Start() {
        this.tpo = TPObject.get(this.transform.parent.gameObject);
        this.tpo.Subscribe("god", this.OnSetGod);
        this.tpo.Subscribe("cam", delegate (string value) {
            string[] components = value.Split(':');
            string user = components[0];
            string byteStr = components[1];
            byte[] bytes = SlimeScreenCapture.StringToByteArray(byteStr);
            Texture2D tex = new Texture2D(100, 100, TextureFormat.ARGB32, false);
            tex.LoadRawTextureData(bytes);
            tex.Apply();
            if (screens.ContainsKey(user)) {
                screens[user].SetTexture(tex);
            }
            // viz.texture = tex;
        });
    }

    public void AddScreen(string username, SlimeScreenRenderer screen) {
        this.screens.Add(username, screen);
    }

    private void OnSetGod(string newGod) {
        // Set global variable
        this.GodUser = newGod;

        // Update UI
        this.iAmGod = (newGod == Teleportal.Teleportal.tp.GetUsername());
        GodIndicator.Shared.gameObject.SetActive(this.iAmGod);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.G)) {
            this.ToggleGod();
        }
        if (Input.GetKeyDown(KeyCode.T)) {
            GameObject voxelGO = Instantiate(this.VoxelTPO);
            TPObject voxelTPO = TPObject.get(voxelGO);
            voxelTPO.OnInit += delegate {
                Transform t = TPUser.SelfUser.transform;
                Vector3 pos = t.position;
                Vector3 ang = t.eulerAngles;
                // pos.z += 1;
                voxelTPO.transform.position = pos;
                voxelTPO.transform.eulerAngles = ang;
            };
        }
    }

    public void ToggleGod() {
        string newGod = Teleportal.Teleportal.tp.GetUsername();
        if (this.iAmGod) {
            newGod = "NULL";
        }

        this.tpo.SetState("god", newGod);
    }
}
