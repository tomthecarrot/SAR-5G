using System.Collections;
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
            if (user == Teleportal.Teleportal.tp.GetUsername()) {
                return;
            }
            string byteStr = components[1];
            
            StartCoroutine(ShowC(user, byteStr));
        });
    }

    private IEnumerator ShowC(string user, string byteStr) {
        byte[] bytes = SlimeScreenCapture.StringToByteArray(byteStr);

        yield return new WaitForEndOfFrame();
        Texture2D tex = new Texture2D(SlimeScreenCapture.Shared.width, SlimeScreenCapture.Shared.height, TextureFormat.ARGB32, false);
        tex.LoadRawTextureData(bytes);
        tex.Apply();
        if (screens.ContainsKey(user)) {
            screens[user].SetTexture(tex);
        } else {
            Debug.Log("key lost!");
        }
        // viz.texture = tex;
    }

    public void AddScreen(string username, SlimeScreenRenderer screen) {
        if (this.screens.ContainsKey(username)) {
            this.screens[username] = screen;
        } else {
            this.screens.Add(username, screen);
        }
    }

    private void OnSetGod(string newGod) {
        // Set global variable
        this.GodUser = newGod;

        // Update UI
        this.iAmGod = (newGod == Teleportal.Teleportal.tp.GetUsername());
        GodModeUI.Shared.gameObject.SetActive(this.iAmGod);

        if (GodIndicator.Shared != null) {
            GodIndicator.Shared.gameObject.SetActive(this.iAmGod);
        }
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
