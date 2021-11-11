using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    public bool IsDead = false;

    private float turnSpeed = 150;
    private float tiltSpeed = 150;
    private float walkSpeed = 20;

    public GameObject trail;
    public GameObject cam1;
    public GameObject cam2;

    [SerializeField]
    private Transform tpcam; //third person camera
    private Camera topcam; //top view cam

    [SerializeField]
    TextMesh nick;

    public PhotonMessageInfo info;

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Confined;

        cam1.SetActive(true);
        cam2.SetActive(false);


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
            IsDead = true;

        }
    }
    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            float turn = Input.GetAxis("Horizontal");
            transform.Translate(new Vector3(0, 0, 1 * walkSpeed * Time.deltaTime));
            transform.Rotate(new Vector3(0, turn * turnSpeed * Time.deltaTime, 0));

        }

        if (Camera.current != null)
        {
            nick.transform.LookAt(Camera.current.transform);
            nick.transform.Rotate(0, 180, 0);
        }

        if (IsDead == true)
        {
            walkSpeed = 0;
            transform.position = new Vector3(0, -100, 0);



            cam1.SetActive(false);
            cam2.SetActive(true);

        }


    }
}
