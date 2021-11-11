using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrack : MonoBehaviour
{
    public float spawnTime = 1, spawnDelay = 0;
    public GameObject player, beam;

    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    private void Update()
    {
        if (Time.time > spawnTime)
        {
            spawnTime = Time.time + spawnDelay;
            SpawnBeam();
        }
    }

    void SpawnBeam()
    {
        Vector3 pos = player.transform.position;
        Quaternion rot = player.transform.rotation;
        GameObject clone = Instantiate(beam, pos, rot);

        Destroy(clone, 15.0f);
    }
}