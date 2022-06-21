using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;

public class DartArrow : MonoBehaviour {
    [SerializeField] private GameObject m_dartArrowPrefab          = default;
    [SerializeField] private Transform  m_dartArrowParentTransform = default;
    [SerializeField] private Renderer   m_renderer                 = default;
    private const float DART_BOARD_RADUIS = 0.3f;
    private Subject<int> onLandingSubject = new Subject<int>();
    private int m_memberCount;

    public ReactiveProperty<(bool, Transform)> ShotProperty { get; private set; }
    public IObservable<int> OnLanded {
        get { return onLandingSubject; }
    }
    public IObservable<(bool,Transform)> OnShot {
        get { return ShotProperty; }
    }
    public bool IsShot {
        get => ShotProperty.Value.Item1; 
    }
    private readonly Vector3[] DEAFAULT_PASS = {
        new Vector3(0.0f, 0, 0),
        new Vector3(0.0f, 0.25f, 1.0f),
        new Vector3(0.0f, 0, 1.95f),
    };

    private Vector3[] m_path;

    public void Initialize(int memberCount) {
        ShotProperty       = ShotProperty ?? new ReactiveProperty<(bool, Transform)>();
        ShotProperty.Value = (false, null);
        m_renderer.enabled = false;
        m_path             = DEAFAULT_PASS.ToArray();
        m_memberCount      = memberCount;
    }

    public float Remap(float value, float from1 = 0, float to1 = 10, float from2 = -0.1f, float to2 = 0.1f) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public void Ready() {
        m_renderer.enabled = true;
        m_path             = DEAFAULT_PASS.ToArray();
        Debug.Log(m_path[1]);
    }

    public void Move() {
        if (!m_renderer.enabled) {
            return;
        }
        var mousePos = Input.mousePosition;
        var worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 1f));
        transform.localPosition = worldPos;
    }

    public void Shot() {
        if (!m_renderer.enabled) {
            return;
        }
        m_renderer.enabled = false;
        var obj            = Instantiate(m_dartArrowPrefab, transform.position, transform.rotation, m_dartArrowParentTransform);
        ShotProperty.Value = (true, obj.transform);
        var diff = new Vector2(DART_BOARD_RADUIS, DART_BOARD_RADUIS) - new Vector2(obj.transform.localPosition.x, obj.transform.localPosition.y);
        for (int i = 1; i < m_path.Length; i++) {
            if(diff.x <= 0.0f) {
                m_path[i].x += diff.x;
            } else if(-diff.x <= -DART_BOARD_RADUIS) {
                m_path[i].x += diff.x - DART_BOARD_RADUIS * 2.0f;
            }
            if (diff.y <= 0.0f) {
                m_path[i].y = diff.y;
            } else if (-diff.y <= -DART_BOARD_RADUIS) {
                m_path[i].y = diff.y - DART_BOARD_RADUIS * 2.0f;
            }
        }
        obj.transform.DOLocalPath(m_path, 0.5f, PathType.CatmullRom)
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
                         var value = Remap(deg, -180.0f, 180.0f, 0, m_memberCount);
                         onLandingSubject.OnNext(Mathf.CeilToInt(value));
                         ShotProperty.Value = (false, null);
                     });
    }

}
