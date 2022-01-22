using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;

public class ColourPicker : MonoBehaviourPun
{
    public static ColourPicker instance;
    public Vector3 bikeColor;

    void Start()
    {
        instance = this;
        bikeColor = new Vector3(0f, 0f, 1f);
    }

    public void Red()
    {
        bikeColor = new Vector3(1f, 0f, 0f);
        GetComponent<Renderer>().material.color = new Color(bikeColor.x, bikeColor.y, bikeColor.z);
    }
    public void Ora()
    {
        bikeColor = new Vector3(1f, 0.5f, 0f);
        GetComponent<Renderer>().material.color = new Color(bikeColor.x, bikeColor.y, bikeColor.z);
    }
    public void Yel()
    {
        bikeColor = new Vector3(1f, 1f, 0f);
        GetComponent<Renderer>().material.color = new Color(bikeColor.x, bikeColor.y, bikeColor.z);
    }
    public void Lim()
    {
        bikeColor = new Vector3(0.5f, 1f, 0f);
        GetComponent<Renderer>().material.color = new Color(bikeColor.x, bikeColor.y, bikeColor.z);
    }
    public void Gre()
    {
        bikeColor = new Vector3(0f, 1f, 0f);
        GetComponent<Renderer>().material.color = new Color(bikeColor.x, bikeColor.y, bikeColor.z);
    }
    public void Cya()
    {
        bikeColor = new Vector3(0f, 1f, 0.5f);
        GetComponent<Renderer>().material.color = new Color(bikeColor.x, bikeColor.y, bikeColor.z);
    }
    public void Tur()
    {
        bikeColor = new Vector3(0f, 1f, 1f);
        GetComponent<Renderer>().material.color = new Color(bikeColor.x, bikeColor.y, bikeColor.z);
    }
    public void LiB()
    {
        bikeColor = new Vector3(0f, 0.5f, 1f);
        GetComponent<Renderer>().material.color = new Color(bikeColor.x, bikeColor.y, bikeColor.z);
    }
    public void Blu()
    {
        bikeColor = new Vector3(0f, 0f, 1f);
        GetComponent<Renderer>().material.color = new Color(bikeColor.x, bikeColor.y, bikeColor.z);
    }
    public void Pur()
    {
        bikeColor = new Vector3(0.5f, 0f, 1f);
        GetComponent<Renderer>().material.color = new Color(bikeColor.x, bikeColor.y, bikeColor.z);
    }
    public void Pin()
    {
        bikeColor = new Vector3(1f, 0f, 1f);
        GetComponent<Renderer>().material.color = new Color(bikeColor.x, bikeColor.y, bikeColor.z);
    }
    public void Mag()
    {
        bikeColor = new Vector3(1f, 0f, 0.5f);
        GetComponent<Renderer>().material.color = new Color(bikeColor.x, bikeColor.y, bikeColor.z);
    }

}
