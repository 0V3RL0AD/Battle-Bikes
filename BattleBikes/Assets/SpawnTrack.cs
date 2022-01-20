using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrack : MonoBehaviour
{
    public static SpawnTrack instance;

    private float spawnTime = 0.02f, beamTimer = 0;
    public bool dead;
    public GameObject player, beam;

    // Start is called before the first frame update
    void Start()
    {
        dead = true;
        instance = this;
    }
    // Update is called once per frame
    private void Update()
    {
        if (dead == false)
        {
            beamTimer += Time.deltaTime;
            if (beamTimer >= spawnTime)
            {
                SpawnBeam();
                beamTimer = 0;
            }
        }
    }

    void SpawnBeam()
    {
            Vector3 pos = player.transform.position;
            Quaternion rot = player.transform.rotation;
            GameObject clone = Instantiate(beam, pos, rot);

            Destroy(clone, 8.0f);
    }
}