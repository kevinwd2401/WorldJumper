using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private EnemyManager em;
    [SerializeField] float speed;
    [SerializeField] Transform enemyModel;
    [SerializeField] GameObject explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckPlayerVisibleCor());
    }

    void OnCollisionEnter(Collision c) {
        if (c.gameObject.CompareTag("Player")) {
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
                    Debug.Log("Alert");
                }
            }
            yield return new WaitForSeconds(1.0f);
        }
        
    }
}
