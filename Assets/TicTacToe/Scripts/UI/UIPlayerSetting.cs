using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
using System;
using Miximum;

namespace TicTacToe
{
    public class UIPlayerSetting : MonoBehaviour
    {
        // -------------------------------------------------------------------------------------
        [SerializeField] private PlayerType m_PlayerType;
        [SerializeField] private Dropdown m_ControllerDropdown;
        [SerializeField] private ToggleGroup m_ToggleGroup;
        [SerializeField] private Toggle[] m_SymbolToggles;
        // -------------------------------------------------------------------------------------
        private PlayerInfo _Player;
        private bool _IsSetup;
        // -------------------------------------------------------------------------------------
        // Unity Funtion
        private void Start()
        {
            _IsSetup = true;
            // Dropdown
            m_ControllerDropdown.OnValueChangedAsObservable().Where(_ => !_IsSetup).Subscribe(_value => 
            {
                _Player.ControlerType = (ControllerType)_value;
                UpdateData();
            }).AddTo(this);

            // Toggle
            for (int i = 0; i < m_SymbolToggles.Length; i++)
            {
                var index = i;
                m_SymbolToggles[i].OnValueChangedAsObservable().Where(_value => _value && !_IsSetup).Subscribe(_value => 
                {
                    Debug.Log((SymbolType)index);
                }).AddTo(this);
            }
            _IsSetup = false;
        }
        // -------------------------------------------------------------------------------------
        // Public Funtion
        public void RefreshSetting()
        {
            _IsSetup = true;

            _Player = DataController.Instance.GetPlayerInfo(m_PlayerType);
            

            _IsSetup = false;
        }
        public void UpdateData()
        {
            DataController.Instance.SetPlayerInfo(m_PlayerType, _Player);
        }
        // -------------------------------------------------------------------------------------
        // Private Funtion

        // -------------------------------------------------------------------------------------
    }
}
