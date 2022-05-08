using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameText : MonoBehaviour {
    [SerializeField] private TMP_Text        m_nameText      = default;
    [SerializeField] private RectTransform   m_rectTransform = default;

    public string Name {
        get => m_nameText.text;
        set => m_nameText.text = value;
    }

    public float Angle {
        get => m_rectTransform.eulerAngles.z;
        set {
            m_rectTransform.eulerAngles = new Vector3(0, 0, value);
        }
    }

    public Vector3 Position {
        get => m_rectTransform.position;
        set {
            m_rectTransform.localPosition = value;
        }
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log(other.gameObject.name + ":" + Name);
    }

}
