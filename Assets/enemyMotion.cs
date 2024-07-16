using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMotion : MonoBehaviour
{
    public float jumpTime;
    public float jumpForce;
    public float xSpawnPosition;
    public float zSpawnPosition;

    private bool hasCollided;
    private float timeFromCollision;
    private Rigidbody body;
    private Vector3 spawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        hasCollided = false;
        timeFromCollision = 0;
        body = GetComponent<Rigidbody>();
        spawnPosition = new Vector3(0f, 10f, 20f);
        transform.position = spawnPosition;
    }

    void OnCollisionEnter(Collision collision){
        int layer = collision.gameObject.layer;

        if (layer == LayerMask.NameToLayer("Scenario")){
            hasCollided = true;
        }
        if (layer == LayerMask.NameToLayer("Ball")){
            hasCollided = false;
            timeFromCollision = 0;
            transform.position = spawnPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hasCollided){
            if (timeFromCollision < jumpTime){
                timeFromCollision += Time.deltaTime;
            } else {
                timeFromCollision = 0;
                hasCollided = false;
                Vector3 jumpVector = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f));
                jumpVector.Normalize();
                body.AddForce(jumpForce * jumpVector, ForceMode.Impulse);
            }
        } 
    }
}
