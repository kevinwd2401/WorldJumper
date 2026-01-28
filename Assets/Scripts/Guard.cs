using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    private EnemyManager em;
    [SerializeField] float speed;
    [SerializeField] Transform gunTransform;

    [SerializeField] Transform[] patrolPoints;

    private bool playerSpotted;
    public bool PlayerSpotted => playerSpotted;

    private EnemyState state;

    //pathfinding
    public Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {
        playerSpotted = false;
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
        ToggleGun(true);
    }
    public void PlayerSighted() {
        playerSpotted = true;
    }

    public void ToggleGun(bool alert) {
        if (alert) {
            gunTransform.rotation = Quaternion.identity;
        } else {
            gunTransform.eulerAngles = new Vector3(46.3846626f,304.399933f,313.403015f);
        }
    }
    
}

public enum EnemyState
{
    Patrol,
    Alert
}
