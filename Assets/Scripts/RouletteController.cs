using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteController : MonoBehaviour {
    [Header("< ----- Config ----- >")]
    [Range(1, 256)] [SerializeField] private int m_maxNameMember = 1;

    [Header("< ----- Preset Component ----- >")]
    [SerializeField] private NameTextGenerator m_nameTextGenerator = default;
    [SerializeField] private FanObject         m_mainFanObject      = default;
    //[SerializeField] private DartAroow         m_dartArrow          = default;
    [SerializeField] private Transform         m_fanParentTransform = default;
    [SerializeField] private GameObject        m_fanObjectPrefab    = default;

    private float    m_timeCount;

    void Start() {
        // UI èâä˙âª
        m_nameTextGenerator.CreateMemeber(m_maxNameMember);

        // FanObject
        m_mainFanObject.Initialize();
        m_mainFanObject.gameObject.SetActive(false);
        for (int i= 0; i < m_maxNameMember; i++) {
            var obj = Instantiate(m_fanObjectPrefab, m_fanParentTransform.position, m_fanParentTransform.rotation, m_fanParentTransform);
            var fanObject = obj.GetComponent<FanObject>();
            fanObject.Initialize();
            fanObject.SetPosition(360.0f / m_maxNameMember * i);
            fanObject.SetColor((i % 2 == 0 ? Color.white : Color.black) * 0.5f);
        }

        // Initialize
        m_timeCount = 0;
    }

    private void FixedUpdate() {
        m_timeCount += 1f;
        float count  = 360.0f / m_maxNameMember * Mathf.FloorToInt(m_timeCount * Time.deltaTime);
        m_mainFanObject.SetPosition(count);
    }
}