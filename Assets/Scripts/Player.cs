using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool worldActive;
    public Camera cam;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TogglePlayer(bool active) {
        worldActive = active;
        GetComponent<Collider>().enabled = worldActive;
    }

    public void Movement(Vector3 velocity) {
        rb.AddForce(velocity, ForceMode.Acceleration);
    }

    public bool CheckCollision() {
        // return true if there is collision
        return Physics.CheckSphere(transform.position, 0.52f, 1 << 6, QueryTriggerInteraction.Ignore);
    }

    public void DeathAnimation() {

    }
}
