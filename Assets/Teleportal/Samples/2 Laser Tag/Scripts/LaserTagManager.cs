using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teleportal;

public class LaserTagManager : MonoBehaviour
{
    public GameObject LaserPrefab;

    void Update() {
        // If the player taps the screen (or left-clicks the mouse)
        // and is already authenticated with this realm in Teleportal...
        if (Input.GetMouseButtonDown(0) && Teleportal.AuthModule.Shared.IsAuthed()) {
            // Get the Transform of this player's camera.
            Transform t = Camera.main.transform;

            // Apply that transform to a new laser object.
            // (see the Laser Prefab TPObject in the Unity inspector)
            GameObject laser = Instantiate(this.LaserPrefab, t.position, t.rotation);

            // Wait for the TPObject to initialize across the network...
            TPObject tpo = TPObject.get(laser);
            tpo.OnInit += delegate {
                // Then force update the transform.
                // This is necessary when instantiating a TPObject
                // with a specific position and rotation in the world.
                tpo.UpdateRemoteTransform();
            };
        }
    }

}
