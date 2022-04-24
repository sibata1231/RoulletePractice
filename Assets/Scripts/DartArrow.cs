using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DartArrow : MonoBehaviour {
    [SerializeField] private GameObject m_dartArrowPrefab               = default;
    [SerializeField] private Transform  m_dartArrowParentTransform = default;

    void Start() {
        
    }

    void Update() {
        var mousePos = Input.mousePosition;
        var worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 1f));
        transform.localPosition = worldPos;

        if (Input.GetMouseButtonDown(0)) {
            var obj = Instantiate(m_dartArrowPrefab, transform.position, transform.rotation, m_dartArrowParentTransform);
            Vector3[] path = {
                new Vector3(0.0f, 0, 0),
                new Vector3(0.0f, 0.25f, 1.0f),
                new Vector3(0.0f, 0, 1.8f),
            };

            obj.transform.DOLocalPath(path, 0.5f, PathType.CatmullRom)
                         .SetRelative()
                         .SetEase(Ease.Linear)
                         .SetLookAt(0.01f, -Vector3.forward);
        }
    }
}
