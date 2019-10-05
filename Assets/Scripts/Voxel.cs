using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teleportal;

public class Voxel : MonoBehaviour
{
    private TPObject tpo;
    private MeshRenderer renderer;

    void Awake() {
        this.renderer = this.GetComponent<MeshRenderer>();
        this.renderer.enabled = false;
    }

    void Start() {
        this.tpo = TPObject.get(this.transform.parent.gameObject);
        this.tpo.Subscribe("explored", this.OnExplored);
    }

    public void Explore() {
        // Claim as self on Realm
        this.tpo.SetState("explored", Teleportal.Teleportal.tp.GetUsername());

        // (add here)
    }

    private void OnExplored(string newValue) {
        this.renderer.enabled = true;
    }
}
