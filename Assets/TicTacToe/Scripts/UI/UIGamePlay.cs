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
            // Hide Wait For Next Game Canvas
            m_WaitForNextGameCanvas.SetAlpha(0);

            // Subscribe Button
            m_BackButton.OnClickAsObservable().Subscribe(_ => OnBackButton()).AddTo(this);
            m_ReplayButton.OnClickAsObservable().Subscribe(_ => ReGame()).AddTo(this);
            m_UndoButton.OnClickAsObservable().Subscribe(_ => 
            {
                GameController.Instance.Undo();
                if(GameController.Instance.TurnCount > 1)
                    m_UndoButton.gameObject.SetActive(true);
                else
                    m_UndoButton.gameObject.SetActive(false);
            }).AddTo(this);

            // Enable Undo button as observable
            GameController.Instance.OnGameSelectedAsObservable().Subscribe(_ => 
            {
                if(GameController.Instance.TurnCount >= 1)
                    m_UndoButton.gameObject.SetActive(true);
                else
                    m_UndoButton.gameObject.SetActive(false);
            }).AddTo(this);

            // Update player scores
            GameController.Instance.OnGameCompletedAsObservable().Subscribe(_ => 
            {
                m_WaitForNextGameCanvas.SetAlpha(1);
                m_Player1Score.text = GameController.Instance.GetPlayer(PlayerName.Player1).Score.ToString();
                m_Player2Score.text = GameController.Instance.GetPlayer(PlayerName.Player2).Score.ToString();
            }).AddTo(this);
            
            // Update current turn
            GameController.Instance.OnBeginTurnAsObservable().Subscribe(_playerName => 
            {
                m_UIBoard.UpdatePlayerSymbol(_playerName);
            }).AddTo(this);
        }

        // -------------------------------------------------------------------------------------
        // Public Funtion
        public override void InitCanvas()
        {
            m_Player1Score.text = "0";
            m_Player2Score.text = "0";
            m_UIBoard.Refresh(DataManager.BoardSize.GetHashCode());
            m_UndoButton.gameObject.SetActive(false);
            m_GameController.InitGame();
        }
        // -------------------------------------------------------------------------------------
        // Private Funtion
        private void OnBackButton()
        {
            UIController.Instance.ChangeUI(UIName.Menu);
        }
        private void ReGame()
        {
            m_UIBoard.Refresh(DataManager.BoardSize.GetHashCode());
            m_UndoButton.gameObject.SetActive(false);
            GameController.Instance.ReGame();
            m_WaitForNextGameCanvas.SetAlpha(0);
        }
        // -------------------------------------------------------------------------------------
        
    }
}
