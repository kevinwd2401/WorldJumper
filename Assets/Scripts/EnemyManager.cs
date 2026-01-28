using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public Transform lastSpotted;
    [SerializeField] Guard[] guards;
    [SerializeField] Target target;

    private bool targetAlert;

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

    public void TargetAlert() {
        targetAlert = true;
    }

    private IEnumerator UpdateVariables() {
        while (true) {
            yield return new WaitForSeconds(0.33f);
            bool playerSeen = false;
            foreach (Guard g in guards) {
                if (g.PlayerSpotted) {
                    playerSeen = true;
                    break;
                }
            }

            if (playerSeen || targetAlert) {
                AlertAllGuards();
                lastSpotted.position = PlayerManager.Instance.GetPlayerPos();
                targetAlert = false;
            }
        }
    }
}
