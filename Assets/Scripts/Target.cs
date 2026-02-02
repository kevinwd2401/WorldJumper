using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Target : Pathfinding
{
    private EnemyManager em;
    [SerializeField] float speed;
    private float turnSpeed = 120;
    [SerializeField] Transform enemyModel;
    [SerializeField] GameObject explosionPrefab;

    [SerializeField] Transform[] patrolPoints;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        agentSetup();
        StartCoroutine(CheckPlayerVisibleCor());
        StartCoroutine(UpdateMovement());
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        agent.nextPosition = transform.position;
        rb.AddForce( speed * agent.desiredVelocity, ForceMode.Acceleration);
    }

    void Update() {
        if (rb.velocity.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(rb.velocity.normalized);
            enemyModel.rotation = Quaternion.RotateTowards(
                enemyModel.rotation,
                targetRotation,
                Time.deltaTime * turnSpeed
            );
        }
    }

    void OnCollisionEnter(Collision c) {
        if (c.gameObject.CompareTag("Player")) {
            if (c.gameObject.TryGetComponent<Player>(out Player p)) {
                p.PlaySound(1);
            }
            Die();
        }
    }
    private void Die() {
        GameManager.Instance.TargetDeath();
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, 4);
        Destroy(gameObject);
    }

    public void SetEnemyManager(EnemyManager em) {
        this.em = em;
    }

    private IEnumerator CheckPlayerVisibleCor() {
        while (true) {
            Vector3 toPlayer = PlayerManager.Instance.GetPlayerPos() - transform.position;
            float dot = Vector3.Dot(enemyModel.forward, toPlayer.normalized);
            if (dot > 0.6) {
                if (!Physics.Raycast(
                transform.position,
                toPlayer.normalized,
                out RaycastHit hit,
                toPlayer.magnitude,
                1 << 6,
                QueryTriggerInteraction.Ignore))
                {
                    em.TargetAlert();
                }
            }
            yield return new WaitForSeconds(1.0f);
        }
        
    }

    private IEnumerator UpdateMovement() {
        
        destination = getRandomNavPoint(patrolPoints[0].position, 3);
        agent.SetDestination(destination);

        yield return new WaitForSeconds(3);

        int index = 1;
        while (true) {
            if (hasReachedDest()) {
                destination = getRandomNavPoint(patrolPoints[index++].position, 4);
                agent.SetDestination(destination);
                if (index >= patrolPoints.Length) {
                    index = 0;
                }
            }
            yield return new WaitForSeconds(3);
        }
        
    }

    
}
