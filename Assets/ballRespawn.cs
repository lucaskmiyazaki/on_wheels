using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class ballRespawn : MonoBehaviour
{

    public GameObject player;
    public float respawnTime;

    private XRGrabInteractable grabInteractable;
    private bool isInteracting;
    private bool hasCollided;
    private float timeFromCollision;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos = player.transform.position;
        transform.position = pos;

        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);

        isInteracting = false;
        hasCollided = false;
        timeFromCollision = 0;
    }

    void OnGrab(SelectEnterEventArgs args){
        isInteracting = true;
    }

    void OnCollisionEnter(Collision collision){
        hasCollided = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasCollided){
            if (timeFromCollision < respawnTime){
                timeFromCollision += Time.deltaTime;
            } else {
                timeFromCollision = 0;
                hasCollided = false;
                isInteracting = false;
            }
        } 
            //isInteracting = false;

        if (!isInteracting){
            Vector3 pos = player.transform.position;
            transform.position = pos;
        }
    }
}
