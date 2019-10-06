using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstResponderIndicator : MonoBehaviour
{
    public static FirstResponderIndicator Shared;

    void Awake() {
        FirstResponderIndicator.Shared = this;
        this.gameObject.SetActive(false);
    }
}
