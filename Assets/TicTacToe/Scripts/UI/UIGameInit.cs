using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;
using Miximum;

namespace TicTacToe
{
    public class UIGameInit : UICanvas
    {
        // -------------------------------------------------------------------------------------
        [SerializeField] private UIPlayerSetting m_Player1Setting;
        [SerializeField] private UIPlayerSetting m_Player2Setting;
        [SerializeField] private Button m_PlayButton;
        [SerializeField] private Button m_BackButton;
        // -------------------------------------------------------------------------------------
        // Unity Funtion
        private void Start()
        {
            m_PlayButton.OnClickAsObservable().Subscribe(_ => OnStartButton()).AddTo(this);
            m_BackButton.OnClickAsObservable().Subscribe(_ => OnBackButton()).AddTo(this);
        }
        // -------------------------------------------------------------------------------------
        // Public Funtion
        public override void InitCanvas()
        {
            Debug.Log("InitCamvas");
            m_Player1Setting.RefreshSetting();
            m_Player2Setting.RefreshSetting();
        }
        // -------------------------------------------------------------------------------------
        // Private Funtion
        private void OnStartButton()
        {
            UIController.Instance.ChangeUI(UIName.GamePlay);
        }
        private void OnBackButton()
        {
            UIController.Instance.ChangeUI(UIName.Menu);
        }
        // -------------------------------------------------------------------------------------
    }
}
