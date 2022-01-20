using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    private float turnSpeed = 100;
    private float walkSpeed = 40;
    public MeshRenderer bike;

    [SerializeField]
    private Transform tpcam, deadcam; //third person camera

    [SerializeField]
    TextMesh nick;

    public PhotonMessageInfo info;

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;

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

        StartCoroutine(SpawnInvinc(0.4f));


    }

    void OnTriggerEnter(Collider col)
    {
        if (SpawnTrack.instance.dead == false)
        { 
            if (col.gameObject.CompareTag("Track") || col.gameObject.CompareTag("Walls"))
            {
                IsDead();
            }
        }
    }

    void IsDead()
    {
        SpawnTrack.instance.dead = true;
        turnSpeed = 0;
        walkSpeed = 0;
        bike.enabled = false;
        StartCoroutine(RespawnTime(2));
        StartCoroutine(SpawnInvinc(2.4f));
    }

    IEnumerator RespawnTime(float time1)
    {
        yield return new WaitForSeconds(time1);

        int temp = Random.Range(0, 8);
        transform.position = SpawnPoints.instance.spawnPoints[temp].position;
        transform.rotation = SpawnPoints.instance.spawnPoints[temp].rotation;
        turnSpeed = 100;
        walkSpeed = 40;
        bike.enabled = true;
    }
    IEnumerator SpawnInvinc(float time2)
    {
        yield return new WaitForSeconds(time2);

        SpawnTrack.instance.dead = false;
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

        if (SpawnTrack.instance.dead == false)
        {
            if (Input.GetKey(KeyCode.W))
            {
                walkSpeed = 60;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                walkSpeed = 20;
            }
            else
            {
                walkSpeed = 40;
            }
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
