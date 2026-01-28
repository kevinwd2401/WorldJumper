using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    private EnemyManager em;
    [SerializeField] float speed;

    [SerializeField] Transform[] patrolPoints;

    private bool playerSpotted;
    public bool PlayerSpotted => playerSpotted;

    private EnemyState state;

    //pathfinding
    public Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {
        state = EnemyState.Patrol;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetEnemyManager(EnemyManager em) {
        this.em = em;
    }
    public void AlertGuard() {
        state = EnemyState.Alert;
    }
    
}

public enum EnemyState
{
    Patrol,
    Alert
}
