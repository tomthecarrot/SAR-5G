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
    private TPObject tpo;

    void Awake() {
        SlimeManager.Shared = this;
    }

    void Start() {
        this.tpo = TPObject.get(this.transform.parent.gameObject);
        this.tpo.Subscribe("god", this.OnSetGod);
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
