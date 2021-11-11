/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    public Material customMat;
    public float spawnTime = 5, monoSpawnDelay = 5, width = 1;
    public GameObject player;
    public bool firstTime = true, isEven;
    public List<Vector3> verticesDef;
    public List<int> trianglesDef;

    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    private void Update()
    {
            if (Time.time > spawnTime)
            {
                spawnTime = Time.time + monoSpawnDelay;
                MonoLine();
            }
    }

    void MonoLine()
    {
        Vector3[] vertices = null;
        int[] triangles = null;

        var backward = player.transform.position - (player.transform.forward);

        if (firstTime)
        {
            vertices = new Vector3[]
            {
                backward + (player.transform.right * -width),
                backward - (player.transform.right * -width),
                backward - (player.transform.right * -width) + player.transform.up * width,
                backward + (player.transform.right * -width) + player.transform.up * width
            };

            triangles = new int[]
            {
                0, 2, 1, //front face
                0, 3, 2
            };

            GameObject line = new GameObject();
            line.tag = "Laser";

            MeshFilter meshFilter = line.AddComponent<MeshFilter>();
            line.AddComponent<MeshRenderer>().material = customMat;

            MeshCollider meshCollider = line.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = meshFilter.mesh;

            meshFilter.mesh.vertices = vertices;
            meshFilter.mesh.triangles = triangles;

            verticesDef = new List<Vector3>();
            trianglesDef = new List<int>();
            foreach (var v in vertices)
                verticesDef.Add(v);
            foreach (var t in triangles)
                trianglesDef.Add(t);

            isEven = false;
            firstTime = false;

            int x = 4;
            return;
        }

        if (isEven)
        {
            verticesDef.Add(backward + (player.transform.right * -width));
            verticesDef.Add(backward - (player.transform.right * -width));
            verticesDef.Add(backward - (player.transform.right * -width) + player.transform.up * width);
            verticesDef.Add(backward + (player.transform.right * -width) + player.transform.up * width);

            //left face
            trianglesDef.Add(x - 4);
            trianglesDef.Add(x - 1);
            trianglesDef.Add(x);

            trianglesDef.Add(x - 4);
            trianglesDef.Add(x);
            trianglesDef.Add(x - 3);

            //top face
            trianglesDef.Add(x - 4);
            trianglesDef.Add(x + 3);
            trianglesDef.Add(x + 2);

            trianglesDef.Add(x - 4);
            trianglesDef.Add(x + 2);
            trianglesDef.Add(x - 3);

            //right face
            trianglesDef.Add(x - 3);
            trianglesDef.Add(x + 2);
            trianglesDef.Add(x + 1);

            trianglesDef.Add(x - 3);
            trianglesDef.Add(x + 1);
            trianglesDef.Add(x - 2);

            //bottom and back face not needed

            isEven = false;
        }
        else
        {
            verticesDef.Add(backward + (player.transform.right * -width) + player.transform.up * width);
            verticesDef.Add(backward - (player.transform.right * -width) + player.transform.up * width);
            verticesDef.Add(backward - (player.transform.right * -width));
            verticesDef.Add(backward + (player.transform.right * -width));

            //left face
            trianglesDef.Add(x - 4);
            trianglesDef.Add(x + 3);
            trianglesDef.Add(x);

            trianglesDef.Add(x - 4);
            trianglesDef.Add(x);
            trianglesDef.Add(x - 1);

            //top face
            trianglesDef.Add(x - 2);
            trianglesDef.Add(x - 1);
            trianglesDef.Add(x);

            trianglesDef.Add(x - 2);
            trianglesDef.Add(x);
            trianglesDef.Add(x + 1);

            //right face
            trianglesDef.Add(x - 3);
            trianglesDef.Add(x - 2);
            trianglesDef.Add(x + 1);

            trianglesDef.Add(x - 3);
            trianglesDef.Add(x + 1);
            trianglesDef.Add(x + 2);

            //bottom and back face not needed

            isEven = true;
        }
        x += 4;

        meshFilter.mesh.vertices = verticesDef.ToArray();
        meshFilter.mesh.triangles = trianglesDef.ToArray();

        meshCollider.sharedMesh = meshFilter.mesh;
    }
}*/