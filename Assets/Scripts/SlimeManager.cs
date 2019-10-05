using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teleportal;

public class SlimeManager : MonoBehaviour
{
    public string GodUser = "NULL";
    private TPObject tpo;

    void Start() {
        this.tpo = TPObject.get(this.transform.parent.gameObject);
        this.tpo.Subscribe("god", this.OnSetGod);
    }

    private void OnSetGod(string newGod) {
        // Set global variable
        this.GodUser = newGod;

        // Update UI
        bool isSelf = (newGod == Teleportal.Teleportal.tp.GetUsername());
        GodIndicator.Shared.gameObject.SetActive(isSelf);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.G)) {
            this.ToggleGod();
        }
    }

    public void ToggleGod() {
        string newGod = Teleportal.Teleportal.tp.GetUsername();
        if (this.GodUser == Teleportal.Teleportal.tp.GetUsername()) {
            newGod = "NULL";
        }

        this.tpo.SetState("god", newGod);
    }
}
