using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxer : MonoBehaviour
{
    public Camera c;
    public GameObject cube;


    // Update is called once per frame
    void Update()
    {
        // get dif in y angle
       Vector3 fwd = cube.transform.forward;
       Vector3 lookAtCube = c.transform.position - cube.transform.position;

        var cubeangle = new Vector3(cube.transform.forward.x, 0.0f, cube.transform.forward.z);
        var cameraangle = new Vector3(c.transform.forward.x, 0.0f, c.transform.forward.z);
        var horizDiffAngle = Vector3.Angle(cubeangle, cameraangle);

        horizDiffAngle = Angle2D(cube.transform.position.x, c.transform.position.x, cube.transform.position.z, c.transform.position.z);

        print(horizDiffAngle);

        

    }


    float Angle2D(float x1, float y1, float x2, float y2)
    {
        return Mathf.Atan2(y2 - y1, x2 - x1) * Mathf.Rad2Deg;
    }
}
