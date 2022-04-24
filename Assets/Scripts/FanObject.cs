using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanObject : MonoBehaviour {
    [SerializeField] private Renderer m_circleRenderer = default;

    private Material m_circleImageMaterial;
    private int      m_circleMaterialCountId;
    private int      m_circleMaterialColorId;

    public void SetPosition(float count) => m_circleImageMaterial.SetFloat(m_circleMaterialCountId, count);
    public void SetColor(Color color)    => m_circleImageMaterial.SetColor(m_circleMaterialColorId, color);

    public void Initialize() {
        m_circleImageMaterial   = new Material(m_circleRenderer.material);
        m_circleRenderer.material     = m_circleImageMaterial;
        m_circleMaterialCountId = Shader.PropertyToID("_Count");
        m_circleMaterialColorId = Shader.PropertyToID("_FanColor");
    }
}
