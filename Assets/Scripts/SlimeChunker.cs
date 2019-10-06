using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teleportal;

public class SlimeChunker : MonoBehaviour
{
    public static SlimeChunker singleton;

    public GameObject voxelPrefab;
    public GameObject environment;
    public int resolution;

    private Bounds bound;
    private float edge_len;
    private List<Vector3> spawnPositions = new List<Vector3>();

    private Color debug_color;
    private Vector3 debug_pos;

    void Awake() {
        singleton = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (environment.GetComponent<Renderer>() != null) {
            bound = environment.GetComponent<Renderer>().bounds;
        } else {
            print("no renderer o.0");
        }
        chunk();
    }

    public void OnDrawGizmos() 
    {
        Gizmos.color = debug_color;
        Gizmos.DrawSphere(debug_pos, 1);
    }

    void chunk()
    {
        float maxZ = bound.max.z;
        float maxX = bound.max.x; 

        Vector3 size = bound.size;
        edge_len = Mathf.Min(size.x, size.z) / (float)resolution;
        print("\nEdge length for the voxels:");
        print(edge_len);
        Vector3 start = bound.min;
        print("\nSTART");
        print(start);

        float currX = start.x;
        float currZ = start.z;
        float fulltimeY = start.y + bound.extents.y;

        //TODO find out why it is only making an L smh
        while (currZ < maxZ) {
            print("\nX..");
            while (currX < maxX) {
                spawnPositions.Add(new Vector3(currX, fulltimeY, currZ));
                currX += edge_len;
                print("CURR X:");
                print(currX);
            }
            // reset x:
            currX = start.x;

            print("Z..");
            spawnPositions.Add(new Vector3(currX, fulltimeY, currZ));
            currZ += edge_len;
            print("\nCURR Z:");
            print(currZ);
        }
        print("\nEND- Bound max: ");
        print(bound.max);

        print("\nLength of spawns: ");
        print(spawnPositions.Count);
        print(spawnPositions);

        foreach (Vector3 pos in spawnPositions) {
            Instantiate(this.voxelPrefab, pos, Quaternion.identity);
        }
    }

    public List<Vector3> getSpawnPositions(){
        return spawnPositions;
    }

    public Vector3 getBoxColliderSize(){
        return new Vector3(edge_len, bound.size.y, edge_len);
    }

}
