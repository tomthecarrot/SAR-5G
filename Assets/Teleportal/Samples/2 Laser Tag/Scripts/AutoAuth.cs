using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teleportal;

public class AutoAuth : MonoBehaviour
{
    public string TargetUsername;

    void Start() {
        #if PLATFORM_LUMIN && !UNITY_EDITOR
            StartCoroutine(this.AuthOnConnect());
        #endif
    }

    private IEnumerator AuthOnConnect() {
        while (!Teleportal.Teleportal.tp.IsConnected()) {
            yield return new WaitForSeconds(0.1f);
        }

        Teleportal.Teleportal.tp.SetUsername(this.TargetUsername);
        Teleportal.AuthModule.Shared.Authenticate();
    }
}
