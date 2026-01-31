using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostEffects : MonoBehaviour
{
    [SerializeField] private Volume volume;

    private Coroutine ChromaCor;
    private bool resetChromaFlag;
    private ChromaticAberration ca;

    private LensDistortion lens;
    private bool lensCharging;
    private Coroutine minusLensCoroutine, restoreLensCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        if (volume.profile.TryGet(out ca))
        {
            ca.intensity.overrideState = true;
        }
        if (volume.profile.TryGet(out lens))
        {
            lens.intensity.overrideState = true;
        }
    }

    public void ChangeVignetteColor(Color c) {
        if (volume.profile.TryGet(out Vignette vignette)) {
            vignette.color.overrideState = true;
            vignette.color.value = c;
        }
    }

    public void IncreaseChroma() {
        if (ChromaCor != null) return;
        ChromaCor = StartCoroutine(IncreaseChromaCor());
    }

    private IEnumerator IncreaseChromaCor() {
        ca.intensity.overrideState = true;
        float val = ca.intensity.value;
        resetChromaFlag = false;

        while (val <= 1 && !resetChromaFlag) {
            ca.intensity.value = val;
            val += Time.deltaTime * 0.7f;
            yield return null;
        }

        if (resetChromaFlag) {
            ca.intensity.value = 0.3f;
        }

        ChromaCor = null;
    }

    public void ResetChroma() {
        resetChromaFlag = true;
        ca.intensity.value = 0.3f;
    }

    public void MinusLens() {
        lensCharging = true;
        minusLensCoroutine = StartCoroutine(MinusLensCor());
    }
    private IEnumerator MinusLensCor() {
        float val = lens.intensity.value;
        float speed = Mathf.Abs(-0.8f - val);

        while (val > -0.8f) {
            lens.intensity.value = val;
            val -= Time.deltaTime * speed;
            yield return null;
        }

        lensCharging = false;
        minusLensCoroutine = null;
    }

    public void RestoreLens() {
        if (restoreLensCoroutine != null) return;
        if (minusLensCoroutine != null) {
            StopCoroutine(minusLensCoroutine);
            minusLensCoroutine = null;
        }
        lensCharging = false;
        restoreLensCoroutine = StartCoroutine(RestoreLensCor());
    }

    private IEnumerator RestoreLensCor() {
        float start = lens.intensity.value;
        float target = 0.2f;
        float duration = 0.4f;

        if (start >= 0.1f)
            yield break;

        float t = 0f;

        while (t < duration && !lensCharging)
        {
            float x = t / duration;
            float eased = easeOutElastic(x);
            lens.intensity.value = Mathf.Lerp(start, target, eased);

            t += Time.deltaTime;
            yield return null;
        }

        lens.intensity.value = target;
        restoreLensCoroutine = null;
    }

    private float easeOutElastic(float x) {
        float c4 = (2 * Mathf.PI) / 3;

        return (x == 0) ? 0 : ((x == 1) ? 1
        : (Mathf.Pow(2, -10 * x) * Mathf.Sin((x * 10 - 0.75f) * c4) + 1));
    }
}
