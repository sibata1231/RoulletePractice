using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;

public class DartArrow : MonoBehaviour {
    [SerializeField] private GameObject m_dartArrowPrefab          = default;
    [SerializeField] private Transform  m_dartArrowParentTransform = default;

    private Subject<int> onLandingSubject = new Subject<int>();
    public IObservable<int> OnLanded {
        get { return onLandingSubject; }
    }

    public float Remap(float value, float from1 = 0, float to1 = 10, float from2 = -0.1f, float to2 = 0.1f) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public void Move() {
        var mousePos = Input.mousePosition;
        var worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 1f));
        transform.localPosition = worldPos;
    }

    public void Shot() {
        var obj = Instantiate(m_dartArrowPrefab, transform.position, transform.rotation, m_dartArrowParentTransform);
        Vector3[] path = {
                new Vector3(0.0f, 0, 0),
                new Vector3(0.0f, 0.25f, 1.0f),
                new Vector3(0.0f, 0, 1.95f),
        };
        obj.transform.DOLocalPath(path, 0.5f, PathType.CatmullRom)
                     .SetRelative()
                     .SetEase(Ease.Linear)
                     .SetLookAt(0.01f, -Vector3.forward)
                     .OnComplete(() => {
                         var rad = Mathf.Atan2(obj.transform.localPosition.y,
                                               obj.transform.localPosition.x);
                         var deg = rad * 180.0f / Mathf.PI + 90.0f;
                         if (deg >= 180.0f) {
                             deg -= 360.0f;
                         }
                         var value = Remap(deg, -180.0f, 180.0f, 0, 16);
                         onLandingSubject.OnNext(Mathf.CeilToInt(value));
                     });
    }

}
