using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MainMenu : MonoBehaviour
{
    private static bool musicSpawned;
    public GameObject BGMPrefab;
    [SerializeField] private Volume volume;
    private ChromaticAberration ca;

    [SerializeField] Transform enemyMesh;

    // Start is called before the first frame update
    void Start()
    {
        if (volume.profile.TryGet(out ca))
        {
            ca.intensity.overrideState = true;
        }
        StartCoroutine(ChromaCor());
        StartCoroutine(TurnCor());

        if (!musicSpawned) {
            musicSpawned = true;
            GameObject sound = Instantiate(BGMPrefab);
            DontDestroyOnLoad(sound);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator TurnCor() {
        Vector3 currRot = enemyMesh.localEulerAngles;
        float t = 0;
        while (true) {
            t += Time.deltaTime;
            enemyMesh.localEulerAngles = currRot + 25 * Mathf.Sin(t / 3) * Vector3.up;
            yield return null;
        }
    }

    private IEnumerator ChromaCor() {
        float t = 0;
        while (true) {
            t += Time.deltaTime;
            ca.intensity.value = 0.6f + 0.4f * Mathf.Sin(t);
            yield return null;
        }
    }

    public void ExitGame() {
        Application.Quit();
    }
    public void StartGame() {
        SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
    }
}
