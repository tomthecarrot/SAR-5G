using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlugModeUI : MonoBehaviour
{
    public static SlugModeUI Shared;

    void Awake() {
        SlugModeUI.Shared = this;
    }
}
