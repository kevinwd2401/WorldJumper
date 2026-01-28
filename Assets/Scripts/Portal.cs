using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    void OnTriggerEnter(Collider c) {
        if (c.CompareTag("Player")) {
            GameManager.Instance.PortalEntered();
        }
    }
}
