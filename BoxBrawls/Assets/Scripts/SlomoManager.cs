using System.Collections;
using UnityEngine;

public class SlowmoManager : MonoBehaviour
{
    // this script manages the slow-motion effect in the game.
    public static SlowmoManager Instance;

    private Coroutine activeSlowmo;
    private float slowmoDuration = 3f;
    private float slowTimeScale = 0.5f;
    private float originalFixedDelta;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            originalFixedDelta = Time.fixedDeltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TriggerSlowmo()
    {
        if (activeSlowmo != null)
        {
            StopCoroutine(activeSlowmo); // Cancel previous timer
        }
        activeSlowmo = StartCoroutine(SlowmoRoutine());
    }

    private IEnumerator SlowmoRoutine()
    {
        Time.timeScale = slowTimeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(slowmoDuration);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = originalFixedDelta;
        activeSlowmo = null;
    }

    public void ResetSlowmo()
{
    if (activeSlowmo != null)
    {
        StopCoroutine(activeSlowmo);
        activeSlowmo = null;
    }
    Time.timeScale = 1f;
    Time.fixedDeltaTime = originalFixedDelta;
}
}
