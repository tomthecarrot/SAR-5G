using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Teleportal {
    
    [CustomEditor(typeof(TPObject))]
    public class TPObjectEditor : Editor {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            TPObject tpo = (TPObject) target;
            if (tpo != null && tpo.initialized && GUILayout.Button("Delete X")) {
                tpo.Delete();
            }
        }
    }

}