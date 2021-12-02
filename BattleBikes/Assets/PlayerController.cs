using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    private float turnSpeed = 150;
    private float tiltSpeed = 150;
    private float walkSpeed = 20;

    [SerializeField]
    private Transform tpcam; //third person camera

    [SerializeField]
    TextMesh nick;

    public PhotonMessageInfo info;

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Confined;

        ChangeColorTo(new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));

        if (photonView.IsMine && tpcam != null)
        {
            tpcam.GetComponent<Camera>().enabled = true;
            nick.text = "";
        }
        else
        {
            tpcam.GetComponent<Camera>().enabled = false;
            nick.text = photonView.Owner.NickName;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Track")
        {
            IsDead();
        }
    }

    void IsDead()
    {
        walkSpeed = 0;
        tiltSpeed = 0;
        turnSpeed = 0;
        transform.Translate(0, -100, 0);
        tpcam.GetComponent<Camera>().enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            float turn = Input.GetAxis("Horizontal");
            transform.Translate(new Vector3(0, 0, -1 * walkSpeed * Time.deltaTime));
            transform.Rotate(new Vector3(0, turn * turnSpeed * Time.deltaTime, 0));
        }

        if (Camera.current != null)
        {
            nick.transform.LookAt(Camera.current.transform);
            nick.transform.Rotate(0, 180, 0);
        }

    }

    [PunRPC] void ChangeColorTo(Vector3 color)
    {
        GetComponent<Renderer>().material.color = new Color(color.x, color.y, color.z);
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material.color = new Color(color.x, color.y, color.z);
            r.material.EnableKeyword("_EMISSION");
            r.material.SetColor("_EmissionColor", new Color(color.x, color.y, color.z));
        }
            if (photonView.IsMine)
        {
            photonView.RPC("ChangeColorTo", RpcTarget.OthersBuffered, color);
        }
    }
}
