using UnityEngine;

public class ProjectilePulse : MonoBehaviour
{
    [Header("Pulse")]
    public float pulseSpeed = 4f;
    public float pulseMinScale = 0.85f;
    public float pulseMaxScale = 1.15f;

    [Header("Emission")]
    public float emissionMinIntensity = 0.8f;
    public float emissionMaxIntensity = 2.0f;

    private Vector3 _baseScale;
    private Renderer _renderer;
    private Material _material;
    private Color _baseEmission;
    private bool _hasEmission;

    void Awake()
    {
        _baseScale = transform.localScale;
        _renderer = GetComponent<Renderer>();

        if (_renderer != null)
        {
            _material = _renderer.material;
            _hasEmission = _material.HasProperty("_EmissionColor");

            if (_hasEmission)
                _baseEmission = _material.GetColor("_EmissionColor");
        }
    }

    void OnEnable()
    {
        transform.localScale = _baseScale;
    }

    void Update()
    {
        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;

        float scale = Mathf.Lerp(pulseMinScale, pulseMaxScale, t);
        transform.localScale = _baseScale * scale;

        if (_hasEmission)
        {
            float intensity = Mathf.Lerp(emissionMinIntensity, emissionMaxIntensity, t);
            _material.SetColor("_EmissionColor", _baseEmission * intensity);
        }
    }

    void OnDisable()
    {
        transform.localScale = _baseScale;

        if (_hasEmission && _material != null)
            _material.SetColor("_EmissionColor", _baseEmission);
    }
}