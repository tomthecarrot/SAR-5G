using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GodIndicator : MonoBehaviour
{
    public static GodIndicator Shared;

    void Awake() {
        GodIndicator.Shared = this;
        this.gameObject.SetActive(false);
    }
}
