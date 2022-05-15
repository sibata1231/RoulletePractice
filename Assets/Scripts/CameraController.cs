using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour {
    [SerializeField] private CinemachineBrain         m_brainCamera  = default;
    [SerializeField] private CinemachineVirtualCamera m_mainCamera   = default;
    [SerializeField] private CinemachineVirtualCamera m_gameCamera   = default;
    [SerializeField] private CinemachineVirtualCamera m_dartCamera   = default;
    [SerializeField] private CinemachineVirtualCamera m_resultCamera = default;

    public void SetCamera(GameStatuses gameStatuses) {
        switch (gameStatuses) {
            case GameStatuses.MAIN:   setCamera(10,  0,  0,  0); break;
            case GameStatuses.GAME:   setCamera( 0, 10,  0,  0); break;
            case GameStatuses.RESULT: setCamera( 0,  0,  0, 10); break;
            default:                  setCamera(10,  0,  0,  0); break;
        }
    }

    private void setCamera(int priority1, int priority2, int priority3, int priority4) {
        m_mainCamera.Priority   = priority1;
        m_gameCamera.Priority   = priority2;
        m_dartCamera.Priority   = priority3;
        m_resultCamera.Priority = priority4;
    }

    public void SetTarget(Transform targetTransform) {
        setCamera(0, 0, 10, 0);
        m_dartCamera.Follow = targetTransform;
        m_dartCamera.LookAt = targetTransform;
    }
}
