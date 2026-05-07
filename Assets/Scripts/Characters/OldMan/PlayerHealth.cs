using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Respawn")]
    public string respawnSceneName = "BeginningArea";

    [Header("Flash Effect")]
    public Renderer[] meshRenderers; // assign Scavenger_mesh_body + backpack
    public float flashDuration = 0.08f;
    public int flashCount = 4;
    public Color hitColor = Color.red;

    private Health _health;

    void Awake()
    {
        _health = GetComponent<Health>();

        if (_health == null)
        {
            Debug.LogError("[PlayerHealth] No Health component found!");
            return;
        }

        // Enable iFrames for the player
        _health.hasIFrames = true;

        // Hook up events
        _health.onDeath.AddListener(OnPlayerDeath);
        _health.onHit.AddListener(OnPlayerHit);
        _health.onHealthChanged.AddListener(OnHealthChanged);
    }

    void OnPlayerHit()
    {
        StartCoroutine(FlashEffect());
        Debug.Log("[PlayerHealth] Ouch!");
    }

    void OnHealthChanged(float healthPercent)
    {
        // Add in code here to show health bar, also effects
        Debug.Log($"[PlayerHealth] Health: {healthPercent * 100f:0}%");
    }

    void OnPlayerDeath()
    {
        Debug.Log("[PlayerHealth] Player died — loading respawn scene...");
        // Switch to respawn scene later
        SceneManager.LoadScene(respawnSceneName);
    }

    System.Collections.IEnumerator FlashEffect()
    {
        for (int i = 0; i < flashCount; i++)
        {
            SetColor(hitColor);
            yield return new WaitForSeconds(flashDuration);
            SetColor(Color.white);
            yield return new WaitForSeconds(flashDuration);
        }
    }

    void SetColor(Color color)
    {
        foreach (Renderer r in meshRenderers)
        {
            if (r != null)
                r.material.color = color;
        }
    }
}