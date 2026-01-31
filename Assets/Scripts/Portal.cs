using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] Transform mesh;

    void Start() {
        StartCoroutine(Oscillate());
    }
    private IEnumerator Oscillate() {
        float startVal = mesh.localScale.x;
        float currVal;
        float t = 0;
        while (true) {
            currVal = startVal + 0.4f * Mathf.Sin(t) + 0.4f;
            mesh.localScale = currVal * Vector3.one;
            t += Time.deltaTime;
            yield return null;
        }
    }

    void OnTriggerEnter(Collider c) {
        if (c.CompareTag("Player")) {
            GameManager.Instance.PortalEntered();
        }
    }

}
