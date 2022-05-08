using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameTextGenerator : MonoBehaviour {
    [SerializeField] private GameObject m_namePrefabs     = default;
    [SerializeField] private Transform  m_parentTransform = default;

    public void CreateMemeber(int maxMember) {
        if (maxMember != 0) {
            for (int i = 0; i < maxMember; i++) {
                float angle = 360.0f / maxMember;
                CreateName((i+1).ToString(), angle * i + angle * 0.5f);
            }
        }
    }

    private void CreateName(string name,float angle) {
        var nameObject = Instantiate(m_namePrefabs, m_parentTransform) as GameObject;
        var nameText   = nameObject.GetComponent<NameText>();
        nameText.Name  = name;
        nameText.Angle = angle;
        var rad = angle / -180.0f * Mathf.PI;
        nameText.Position = new Vector3(0.18f * Mathf.Sin(rad), 0.18f * Mathf.Cos(rad), 0.0f);
    }
}
