using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameController : MonoBehaviour {
    [Header("< ----- Config ----- >")]
    [Range(1, 256)] [SerializeField] private int m_maxNameMember = 1;

    [Header("< ----- Preset Component ----- >")]
    [SerializeField] private NameTextGenerator m_nameTextGenerator = default;
    [SerializeField] private Image             m_circleUI = default;

    private Material m_circleImageMaterial;
    private int      m_circleMaterialCountId;
    private float    m_timeCount;

    void Start() {
        // Preset‰Šú‰»
        m_nameTextGenerator.CreateMemeber(m_maxNameMember);
        m_circleImageMaterial   = m_circleUI.material;
        m_circleMaterialCountId = Shader.PropertyToID("_Count");

        // •Ï”‰Šú‰»
        m_timeCount = 0;
    }

    private void FixedUpdate() {
        m_timeCount += 1f;
        float count = 360.0f / m_maxNameMember * Mathf.FloorToInt(m_timeCount * Time.deltaTime);
        m_circleImageMaterial.SetFloat(m_circleMaterialCountId, count);
        //Debug.Log(count);
    }
}