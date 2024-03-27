using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MovementManager : MonoBehaviour
{
    private PhotonView myView; // access the PhotonView script

    // movement
    private GameObject myBody;
    private Rigidbody myRB;
    private float xInput;
    private float zInput;
    private float moveSpeed = 10.0f;


    // materials
    private MeshRenderer myMesh;
    private int index = 0;
    [SerializeField] List<Material> myMaterials;

    // Start is called before the first frame update
    void Start()
    {
        myView = GetComponent<PhotonView>();
        myBody = transform.GetChild(0).gameObject;
        myRB = myBody.GetComponent<Rigidbody>();
        myMesh = myBody.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myView.IsMine)
        {
            xInput = Input.GetAxis("Horizontal");
            zInput = Input.GetAxis("Vertical");
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            myView.RPC("changeMaterial", RpcTarget.Others); // apply function to only the other player's view, and not self
            Debug.Log("Spacebar pressed");
        }
    }

    private void FixedUpdate()
    {
        myRB.AddForce(xInput * moveSpeed, 0, zInput * moveSpeed);
    }

    [PunRPC] void changeMaterial() // [PunRPC]functions that can be called over any/all instances of an instantiate object (aka, something that affects ALL players)
    {
        if (myView.IsMine)
        {
            if (index == myMaterials.Count)
            {
                index = 0;
            }
            myMesh.material = myMaterials[index];
            index++;

        }
        
    }
}
