using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teleportal;

public class SlimeRaycaster : MonoBehaviour
{
    public float RaycastDistance = 3f; // meters

    void Update() {
        if (SlimeManager.Shared.iAmGod) {
            return; // skip
        }

        RaycastHit hit;
        Transform t = TPUser.SelfUser.transform;
        if (Physics.Raycast(t.position, transform.TransformDirection(t.forward), out hit, this.RaycastDistance)) {
            Debug.Log(hit.transform.gameObject.name);
            if (hit.transform.gameObject.name == "TPOccluder") {
                Voxel v = hit.transform.gameObject.GetComponent<Voxel>();
                v.Explore();
            }
        }
    }
}
