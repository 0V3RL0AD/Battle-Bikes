using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ScrollTexture : MonoBehaviourPun
{
    public float ScrollX = 0.5f;
    public float ScrollY = 0.5f;

    private void Update()
    {
        float OffsetX = Time.time * ScrollX;
        float OffsetY = Time.time * ScrollY;
        GetComponent<Renderer>().material.mainTextureOffset = new Vector2(OffsetX, OffsetY);
    }
}
