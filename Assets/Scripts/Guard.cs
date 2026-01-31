using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : Pathfinding
{
    private EnemyManager em;
    [SerializeField] float speed;
    [SerializeField] Transform gunTransform;
    [SerializeField] Transform enemyModel;

    [SerializeField] Transform[] patrolPoints;

    [SerializeField] private float alertTimer;
    private bool playerSpotted;
    public bool PlayerSpotted => playerSpotted;

    public EnemyState state;

    public Vector3 dir => agent.desiredVelocity;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        agentSetup();
        playerSpotted = false;
        state = EnemyState.Patrol;

        StartCoroutine(CheckPlayerVisibleCor());
        StartCoroutine(UpdateMovement());
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        alertTimer -= Time.deltaTime;
        if (state == EnemyState.Alert && alertTimer < 0) {
            state = EnemyState.Patrol;
            ToggleGun(false);
        }
    }

    void FixedUpdate()
    {
        agent.nextPosition = transform.position;
        rb.AddForce( (playerSpotted ? 2 : speed) * agent.desiredVelocity, ForceMode.Acceleration);
    }

    public void SetEnemyManager(EnemyManager em) {
        this.em = em;
    }
    public void AlertGuard() {
        state = EnemyState.Alert;
        ToggleGun(true);
        alertTimer = 15;
        speed = 7;
    }

    public void ToggleGun(bool alert) {
        if (alert) {
            gunTransform.localRotation = Quaternion.identity;
        } else {
            gunTransform.localEulerAngles = new Vector3(35.048687f,292.185242f,320.988861f);
        }
    }

    private IEnumerator UpdateMovement() {
        
        destination = getRandomNavPoint(patrolPoints[0].position, 3);
        agent.SetDestination(destination);

        yield return new WaitForSeconds(3);

        int index = 1;
        while (true) {
            if (playerSpotted) {
                Vector3 playerPos = PlayerManager.Instance.GetPlayerPos();
                if (Vector3.Distance(playerPos, transform.position) > 25) {
                    destination = getRandomNavPoint(patrolPoints[index++].position, 4);
                    agent.SetDestination(destination);
                    if (index >= patrolPoints.Length) {
                        index = 0;
                    }
                } else {
                    destination = getRandomNavPoint(playerPos, 2);
                    agent.SetDestination(destination);
                }
            } else if (state == EnemyState.Alert) {
                if (hasReachedDest()) {
                    Vector3 playerPos = PlayerManager.Instance.GetPlayerPos();
                    if (Vector3.Distance(playerPos, transform.position) > 25) {
                        playerPos = em.lastSpotted.position;
                    }
                    destination = getRandomNavPoint((Random.value > 0.3f) ? em.lastSpotted.position : playerPos, (Random.value > 0.5f) ? 5 : 30);
                    agent.SetDestination(destination);
                }
            } else if (state == EnemyState.Patrol) {
                if (hasReachedDest()) {
                    destination = getRandomNavPoint(patrolPoints[index++].position, 4);
                    agent.SetDestination(destination);
                    if (index >= patrolPoints.Length) {
                        index = 0;
                    }
                }
            }

            yield return new WaitForSeconds(state == EnemyState.Alert ? 0.6f : 2.5f);
        }
        
    }

    private IEnumerator CheckPlayerVisibleCor() {
        yield return new WaitForSeconds(Random.value);
        while (true) {
            Vector3 toPlayer = PlayerManager.Instance.GetPlayerPos() - transform.position;
            float dot = Vector3.Dot(enemyModel.forward, toPlayer.normalized);
            if (dot > 0.45f) {
                if (!Physics.Raycast(
                transform.position,
                toPlayer.normalized,
                out RaycastHit hit,
                toPlayer.magnitude,
                1 << 6,
                QueryTriggerInteraction.Ignore))
                {
                    playerSpotted = true;
                    if (state != EnemyState.Alert) {
                        AlertGuard();
                    } else {
                        alertTimer = 15;
                    }
                }
                else
                {
                    playerSpotted = false;
                }
            }
            yield return new WaitForSeconds(0.6f);
        }
        
    }
    
    void OnCollisionEnter(Collision c) {
        if (c.gameObject.CompareTag("Player")) {
            PlayerManager.Instance.TakeDamage();
            PlayerManager.Instance.TakeDamage();
            PlayerManager.Instance.TakeDamage();
        }
    }
}

public enum EnemyState
{
    Patrol,
    Idle,
    Alert
}
