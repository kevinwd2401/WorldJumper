using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    int totalTargets, targets;
    public GameObject portal;

    public Slider healthbar;
    public TextMeshProUGUI objective;
    public TextMeshProUGUI timer;
    public TextMeshProUGUI loseText;
    public GameObject loseScreen, winScreen;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        totalTargets = 1;
        targets = totalTargets;

        StartCoroutine(TimerCor());
    }

    public void UpdateHealthUI(int health) {
        healthbar.value = health;
    }

    public void LossScreen() {
        Time.timeScale = 0;
        loseScreen.SetActive(true);
    }
    public void PortalEntered() {
        Time.timeScale = 0;
        winScreen.SetActive(true);
    }

    public void TargetDeath() {
        targets--;
        if (targets == 0) {
            objective.text = "Escape through the portal!";
            portal.SetActive(true);
        } else {
            objective.text = "Targets Eliminated: " + (totalTargets - targets) + "/" + totalTargets;
        }
    }

    private IEnumerator TimerCor() {
        int totalSeconds = 5 * 60;

        while (totalSeconds >= 0)
        {
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;

            timer.text = $"{minutes:00}:{seconds:00}";

            yield return new WaitForSeconds(1f);
            totalSeconds--;
        }

        loseText.text = "You Lose";
        LossScreen();
    }
}
