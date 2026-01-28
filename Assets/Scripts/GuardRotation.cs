using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardRotation : MonoBehaviour
{
    [SerializeField] Guard guard;
    [SerializeField] Transform firepoint;

    private float turnSpeed = 160;
    private float FOV = 0.6f;

    [Header("VFX")]
    [SerializeField] private LineRenderer linePrefab;
    [SerializeField] private float tracerDuration = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShootCor());
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetDir = Vector3.zero;

        if (guard.PlayerSpotted)
        {
            Vector3 playerPos = PlayerManager.Instance.GetPlayerPos();
            targetDir = playerPos - transform.position;
        }
        else if (guard.dir != Vector3.zero)
        {
            targetDir = guard.dir;
        }

        if (targetDir == Vector3.zero)
            return;

        targetDir.y = 0f;

        Quaternion targetRot = Quaternion.LookRotation(targetDir.normalized);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRot,
            turnSpeed * Time.deltaTime
        );
    }

    private void Shoot() {
        Vector3 origin = firepoint.position;
        Vector3 dir = transform.forward;

        if (Physics.Raycast(origin, dir, out RaycastHit hit, 20, ~0, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.CompareTag("Player"))
            {
                PlayerManager.Instance.TakeDamage();
            }

            SpawnAndFadeLine(origin, hit.point, tracerDuration);
        }
    }

    private IEnumerator ShootCor() {
        while (true) {
            yield return new WaitForSeconds(0.5f);
            Vector3 playerDir =  PlayerManager.Instance.GetPlayerPos() - transform.position;
            if (guard.PlayerSpotted && Vector3.Dot(transform.forward, playerDir.normalized) > 0.96f) {
                Shoot();
                yield return new WaitForSeconds(0.25f);
                Shoot();
                yield return new WaitForSeconds(0.25f);
                Shoot();
                yield return new WaitForSeconds(0.4f);
            }
        }
    }

    private void SpawnAndFadeLine(Vector3 start, Vector3 end, float duration)
    {
        if (linePrefab == null) return;

        LineRenderer lr = Instantiate(linePrefab);
        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        StartCoroutine(FadeAndDestroyLine(lr, duration));
    }

    private IEnumerator FadeAndDestroyLine(LineRenderer lr, float duration)
    {
        if (lr == null) yield break;

        Color startColor = lr.startColor;
        Color endColor = lr.endColor;

        float t = 0f;
        while (t < duration && lr != null)
        {
            float a = 1f - (t / duration);

            lr.startColor = new Color(startColor.r, startColor.g, startColor.b, a);
            lr.endColor   = new Color(endColor.r,   endColor.g,   endColor.b,   a);

            t += Time.deltaTime;
            yield return null;
        }

        if (lr != null) Destroy(lr.gameObject);
    }
}
