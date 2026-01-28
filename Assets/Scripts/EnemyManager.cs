using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public Transform lastSpotted;
    [SerializeField] Guard[] guards;
    [SerializeField] Target target;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Guard g in guards) {
            g.SetEnemyManager(this);
        }
        target.SetEnemyManager(this);

        StartCoroutine(UpdateVariables());
    }
    private void AlertAllGuards() {
        foreach (Guard g in guards) {
            g.AlertGuard();
        }
    }

    private IEnumerator UpdateVariables() {
        while (true) {
            yield return new WaitForSeconds(2.0f);
            bool playerSeen = false;
            foreach (Guard g in guards) {
                if (g.PlayerSpotted) {
                    playerSeen = true;
                    break;
                }
            }

            if (playerSeen) {
                AlertAllGuards();
                lastSpotted.position = PlayerManager.Instance.GetPlayerPos();
            }
        }
    }
}
