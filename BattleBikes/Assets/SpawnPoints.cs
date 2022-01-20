using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    public static SpawnPoints instance;

    public Transform[] spawnPoints;

    void Start()
    {
        instance = this;
    }

}
