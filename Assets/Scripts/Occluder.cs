using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Occluder : MonoBehaviour
{
    private BoxCollider bc;
    private List<Vector3> positions; // where to spawn these occluders 

    void Awake(){
        bc = GetComponent<BoxCollider>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        transform.parent.localScale = SlimeChunker.singleton.getBoxColliderSize();
    }

}
