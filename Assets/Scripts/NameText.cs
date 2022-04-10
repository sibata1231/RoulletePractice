using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameText : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI m_nameText      = default;
    [SerializeField] private RectTransform   m_rectTransform = default;

    public string Name {
        get => m_nameText.text;
        set => m_nameText.text = value;
    }

    public float Angle {
        get => m_rectTransform.eulerAngles.z;
        set {
            if(value <= 90.0f || value >= 270.0f) {
                Debug.Log(value);
                m_rectTransform.eulerAngles = new Vector3(0, 0, value);
            } else {
                m_rectTransform.pivot         = new Vector2(0.0f, 0.5f);
                m_rectTransform.eulerAngles   = new Vector3(0, 0, value + 180.0f);
                m_rectTransform.localPosition = Vector3.zero;
            }

        }
    }
}
