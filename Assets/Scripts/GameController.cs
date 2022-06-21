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
[System.Serializable]

public class GameController : MonoBehaviour {
    [SerializeField] private DartArrow          m_dartArrow           = default;
    [SerializeField] private CameraController   m_cameraController    = default;
    [SerializeField] private TextMeshProUGUI    m_debugNumberText     = default;
    [SerializeField] private TextMeshProUGUI    m_debugMemberListText = default;
    [SerializeField] private CSVLoader          m_csvLoader           = default;
    [SerializeField] private RouletteController m_rouletteController  = default;
    [SerializeField] private string             m_fileName            = default;
    [SerializeField] private string             m_targetName          = default;

    private ReactiveProperty<GameStatuses> m_gameStatuses;
    public GameStatuses GameStatuses {
        get => m_gameStatuses.Value;
        set {
            m_gameStatuses.Value = value;
            m_cameraController.SetCamera(value);
        }
    }

    public IObservable<GameStatuses> OnChangedStatus {
        get => m_gameStatuses;
    }

    void Start() {
        m_gameStatuses = new ReactiveProperty<GameStatuses>();
        GameStatuses   = GameStatuses.MAIN;

        m_csvLoader.Initialize(m_fileName);
        m_rouletteController.Initialize(m_csvLoader.CurrentMemberCount);
        m_dartArrow.Initialize(m_csvLoader.CurrentMemberCount);

        // 各State処理
        this.UpdateAsObservable()
            .Where(_ => GameStatuses == GameStatuses.GAME
                        && !m_dartArrow.IsShot)
            .Subscribe(_ => m_dartArrow.Move())
            .AddTo(this);
        this.UpdateAsObservable()
            .Where(_ => GameStatuses == GameStatuses.GAME 
                        && !m_dartArrow.IsShot 
                        && Input.GetMouseButtonDown(0))
            .Subscribe(_ => m_dartArrow.Shot())
            .AddTo(this);
        this.UpdateAsObservable()
            .Where(_ => GameStatuses != GameStatuses.MAIN 
                        && Input.GetKeyDown(KeyCode.Alpha1))
            .Subscribe(_ => GameStatuses = GameStatuses.MAIN)
            .AddTo(this);
        this.UpdateAsObservable()
            .Where(_ => GameStatuses != GameStatuses.GAME 
                        && Input.GetKeyDown(KeyCode.Alpha2))
            .Subscribe(_ => {
                m_dartArrow.Initialize(m_csvLoader.CurrentMemberCount);
                GameStatuses = GameStatuses.GAME;
                Observable.Timer(TimeSpan.FromSeconds(1.5f)).Subscribe(_ =>{
                    m_dartArrow.Ready();
                }).AddTo(this);
            })
            .AddTo(this);
        this.UpdateAsObservable()
            .Where(_ => GameStatuses != GameStatuses.RESULT 
                        && Input.GetKeyDown(KeyCode.Alpha3))
            .Subscribe(_ => GameStatuses = GameStatuses.RESULT)
            .AddTo(this);

        // Dart State
        m_dartArrow.OnShot.Subscribe(x => {
            if (x.Item1) {
                m_cameraController.SetTarget(x.Item2);
            }
        }).AddTo(m_dartArrow.gameObject);

        m_dartArrow.OnLanded.Subscribe(x => {
            m_debugMemberListText.text = "";
            m_debugNumberText.text = "今回の" + m_targetName.ToString() + "は...No." + x.ToString() + " : " + m_csvLoader.GetMamber(x - 1) + "さんです！";
            for (int i = 1; i <= m_csvLoader.CurrentMemberCount; i++) {
                m_debugMemberListText.text += i.ToString("00") + ":" + m_csvLoader.GetMamber(i - 1) + "\n";
            }
            GameStatuses = GameStatuses.RESULT;
        }).AddTo(m_dartArrow.gameObject);
    }
}
