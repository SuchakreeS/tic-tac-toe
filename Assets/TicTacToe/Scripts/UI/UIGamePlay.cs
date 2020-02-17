using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;
using Miximum;
using TMPro;

namespace TicTacToe
{
    public class UIGamePlay : UICanvas
    {
        
        // -------------------------------------------------------------------------------------
        [Header("Button")]
        [SerializeField] private Button m_BackButton;
        [SerializeField] private Button m_UndoButton;
        [SerializeField] private Button m_ReplayButton;
        [Header("Text")]
        [SerializeField] private TextMeshProUGUI m_Player1Score;
        [SerializeField] private TextMeshProUGUI m_Player2Score;

        [Header("Canvas")]
        [SerializeField] private CanvasGroup m_WaitForNextGameCanvas;

        [Header("Controller")]
        [SerializeField] private UIBoard m_UIBoard;
        [SerializeField] private GameController m_GameController;
        // -------------------------------------------------------------------------------------
        // Unity Funtion
        private void Start()
        {
            m_WaitForNextGameCanvas.SetAlpha(0);
            m_BackButton.OnClickAsObservable().Subscribe(_ => OnBackButton()).AddTo(this);
            m_UndoButton.OnClickAsObservable().Subscribe(_ => GameController.Instance.Undo()).AddTo(this);
            m_ReplayButton.OnClickAsObservable().Subscribe(_ => 
            {
                m_UIBoard.Refresh(DataManager.BoardSize.GetHashCode());
                GameController.Instance.SwitchPlayers();
                GameController.Instance.GameReset(true);
                GameController.Instance.GameStart();
                m_WaitForNextGameCanvas.SetAlpha(0);
            }).AddTo(this);

            GameController.Instance.OnGameCompletedAsObservable().Subscribe(_ => 
            {
                m_WaitForNextGameCanvas.SetAlpha(1);
                m_Player1Score.text = GameController.Instance.GetPlayer(PlayerName.Player1).Score.ToString();
                m_Player2Score.text = GameController.Instance.GetPlayer(PlayerName.Player2).Score.ToString();
            }).AddTo(this);
        }


        // -------------------------------------------------------------------------------------
        // Public Funtion
        public override void InitCanvas()
        {
            m_UIBoard.Refresh(DataManager.BoardSize.GetHashCode());
            m_GameController.GameReset();
            m_GameController.GameStart();
        }
        // -------------------------------------------------------------------------------------
        // Private Funtion
        private void OnBackButton()
        {
            UIController.Instance.ChangeUI(UIName.Menu);
        }

        // -------------------------------------------------------------------------------------
        
    }
}
