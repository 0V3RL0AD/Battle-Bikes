using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class PlayerController : MonoBehaviourPun
{
    private float turnSpeed = 100;
    private float walkSpeed = 30;

    private float spawnTime = 0.02f, beamTimer = 0;
    public bool dead = true;
    public GameObject player, beam, deadEffect;
    public MeshRenderer bike;

    [SerializeField]
    private Transform tpcam; //third person camera

    [SerializeField]
    TextMesh nick;

    public PhotonMessageInfo info;

    void Start()
    {
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

    void OnTriggerEnter(Collider col)
    {
        if (dead == false)
        {
            if (col.gameObject.CompareTag("Track") || col.gameObject.CompareTag("Walls"))
            {
                if (photonView.IsMine)
                {
                    photonView.RPC("LosePoints", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber - 1);
                    IsDead();
                }

            }
        }
    }

    [PunRPC] void LosePoints(int player)
    {
        if (player == 0)
        {
            NetworkManager.instance.playerScores[0]--;
        }
        if (player == 1)
        {
            NetworkManager.instance.playerScores[1]--;
        }
        if (player == 2)
        {
            NetworkManager.instance.playerScores[2]--;
        }
        if (player == 3)
        {
            NetworkManager.instance.playerScores[3]--;
        }
    }
    [PunRPC] void IsDead()
    {
        dead = true;
        turnSpeed = 0;
        walkSpeed = 0;
        bike.enabled = false;
        deadEffect.SetActive(true);

        StartCoroutine(RespawnTime());

        if (photonView.IsMine)
        {
            photonView.RPC("IsDead", RpcTarget.Others);
        }
    }

    IEnumerator RespawnTime()
    {
        yield return new WaitForSeconds(2f);

        int temp = Random.Range(0, 8);
        transform.position = SpawnPoints.instance.spawnPoints[temp].position;
        transform.rotation = SpawnPoints.instance.spawnPoints[temp].rotation;
        turnSpeed = 100;
        walkSpeed = 40;
        bike.enabled = true;

        yield return new WaitForSeconds(2.5f);

        dead = false;
        deadEffect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            float turn = Input.GetAxis("Horizontal");
            transform.Translate(new Vector3(0, 0, -1 * walkSpeed * Time.deltaTime));
            transform.Rotate(new Vector3(0, turn * turnSpeed * Time.deltaTime, 0));

            ChangeColorTo(new Vector3(ColourPicker.instance.bikeColor.x, ColourPicker.instance.bikeColor.y, ColourPicker.instance.bikeColor.z));

            if (dead == false)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    walkSpeed = 40;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    walkSpeed = 20;
                }
                else
                {
                    walkSpeed = 30;
                }
            }
        }

        if (dead == false)
        {
            beamTimer += Time.deltaTime;
             if (beamTimer >= spawnTime)
             {
                 Vector3 pos = player.transform.position;
                 Quaternion rot = player.transform.rotation;

                 GameObject clone = Instantiate(beam, pos, rot);
                 Destroy(clone, 8.0f);

                 beamTimer = 0;
             }
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
