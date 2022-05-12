using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using TMPro;

public enum GameStatuses { 
    MAIN = 0, //!< メイン
    GAME,     //!< ゲーム
    RESULT,   //!< リザルト
}

public class GameController : MonoBehaviour {
    [SerializeField] private DartArrow        m_dartArrow        = default;
    [SerializeField] private CameraController m_cameraController = default;
    [SerializeField] private TextMeshProUGUI  m_debugNumberText  = default;

    private GameStatuses m_gameStatuses;
    public GameStatuses GameStatuses {
        get => m_gameStatuses;
        set {
            m_gameStatuses = value;
            m_cameraController.SetCamera(value);
        }
    }

    void Start() {
        m_gameStatuses = GameStatuses.GAME;
        this.UpdateAsObservable()
            .Where(_ => m_gameStatuses == GameStatuses.GAME)
            .Subscribe(_ => m_dartArrow.Move());
        this.UpdateAsObservable()
            .Where(_ => m_gameStatuses == GameStatuses.GAME && 
                        Input.GetMouseButtonDown(0))
            .Subscribe(_ => m_dartArrow.Shot());
        this.UpdateAsObservable()
            .Where(_ => m_gameStatuses != GameStatuses.MAIN &&
                        Input.GetKeyDown(KeyCode.Alpha1))
            .Subscribe(_ => GameStatuses = GameStatuses.MAIN);
        this.UpdateAsObservable()
            .Where(_ => m_gameStatuses != GameStatuses.GAME &&
                        Input.GetKeyDown(KeyCode.Alpha2))
            .Subscribe(_ => GameStatuses = GameStatuses.GAME);
        this.UpdateAsObservable()
            .Where(_ => m_gameStatuses != GameStatuses.RESULT &&
                        Input.GetKeyDown(KeyCode.Alpha3))
            .Subscribe(_ => GameStatuses = GameStatuses.RESULT);

        m_dartArrow.OnLanded.Subscribe(x => {
            m_debugNumberText.text = x.ToString();
        });
    }
}
