using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teleportal;

public class SlimeRaycaster : MonoBehaviour
{
    void Start() {
        
    }

    void Update() {
        if (SlimeManager.Shared.iAmGod) {
            return; // skip
        }

        RaycastHit hit;
        Transform t = TPUser.SelfUser.transform;
        if (Physics.Raycast(t.position, transform.TransformDirection(t.forward), out hit, Mathf.Infinity)) {
            Debug.Log(hit.transform.gameObject.name);
            if (hit.transform.gameObject.name == "Voxel Cube") {
                Voxel v = hit.transform.gameObject.GetComponent<Voxel>();
                v.Explore();
            }
        }
    }
}
