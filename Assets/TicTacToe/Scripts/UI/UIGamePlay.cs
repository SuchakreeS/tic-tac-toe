using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;
using Miximum;

namespace TicTacToe
{
    public class UIGamePlay : UICanvas
    {
        
        // -------------------------------------------------------------------------------------
        [SerializeField] private Button m_BackButton;
        [SerializeField] private UIBoard m_UIBoard;
        // -------------------------------------------------------------------------------------
        // Unity Funtion
        private void Start()
        {
            m_BackButton.OnClickAsObservable().Subscribe(_ => OnBackButton()).AddTo(this);
        }


        // -------------------------------------------------------------------------------------
        // Public Funtion
        public override void InitCanvas()
        {
            m_UIBoard.Refresh(DataManager.BoardSize.GetHashCode());
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
