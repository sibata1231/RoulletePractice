using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameTextGenerator : MonoBehaviour {
    [SerializeField] private GameObject m_namePrefabs           = default;
    [SerializeField] private Transform  m_parentTransform       = default;

    public void CreateMemeber(int maxMember) {
        if (maxMember != 0) {
            for (int i = 0; i < maxMember; i++) {
                float angle = 360.0f / maxMember;
                CreateName("test1", angle * i + angle * 0.5f);
            }
        }
    }
    void CreateName(string name,float angle) {
        var nameObject = Instantiate(m_namePrefabs, m_parentTransform) as GameObject;
        var nameText   = nameObject.GetComponent<NameText>();
        nameText.Name  = name;
        nameText.Angle = angle;
    }
}
