using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] Transform mesh;

    void Start() {
        StartCoroutine(Grow());
    }
    private IEnumerator Oscillate() {
        float startVal = mesh.localScale.x - 0.4f;
        float currVal;
        float t = 0;
        while (true) {
            currVal = startVal + 0.4f * Mathf.Sin(t) + 0.4f;
            mesh.localScale = currVal * Vector3.one;
            t += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator Grow() {
        float startVal = 0.01f;
        float endVal = mesh.localScale.x + 0.4f;
        float t = 0;
        while (t < 1) {
            mesh.localScale = Mathf.Lerp(startVal, endVal, t) * Vector3.one;
            t += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(Oscillate());
    }

    void OnTriggerEnter(Collider c) {
        if (c.CompareTag("Player")) {
            GameManager.Instance.PortalEntered();
        }
    }

}
