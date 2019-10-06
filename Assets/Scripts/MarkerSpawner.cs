using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teleportal;

public class MarkerSpawner : MonoBehaviour {

    public GameObject TPOprefab;

    public void ButtonSelected(int index) {
        string prefabName;

        switch (index) {
            case 0: // victim
                prefabName = "victim_marker";
                break;
            case 1: // hazard
                prefabName = "hazard_marker";
                break;
            case 2: // landmark
                prefabName = "caution_marker";
                break;
            default:
                prefabName = "victim_marker";
                break;
        }

        GameObject markerGO = Instantiate(this.TPOprefab);
        TPObject markerTPO = TPObject.get(markerGO);
        markerTPO.OnInit += delegate {
            Transform t = TPUser.SelfUser.transform;
            Vector3 pos = t.position;
            Vector3 ang = t.eulerAngles;
            // pos.z += 1;
            markerTPO.transform.position = pos;
            markerTPO.transform.eulerAngles = ang;

            // set marker prefab renderer
            markerTPO.SetState("prefab", prefabName);
        };
    }

}