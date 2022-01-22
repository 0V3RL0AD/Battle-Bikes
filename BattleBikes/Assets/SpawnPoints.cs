using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPoints : MonoBehaviourPun
{
    public static SpawnPoints instance;

    public Transform[] spawnPoints;

    void Start()
    {
        instance = this;
    }

}
