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
        [SerializeField] private UIPlayerSetting m_OpponentSetting;
        [SerializeField] private Dropdown m_ControllerDropdown;
        [SerializeField] private ToggleGroup m_ToggleGroup;
        [SerializeField] private Toggle[] m_SymbolToggles;
        // -------------------------------------------------------------------------------------
        private PlayerInfo _Player;
        private bool _IsSetup;
        // -------------------------------------------------------------------------------------
        public PlayerInfo Player => _Player;
        // -------------------------------------------------------------------------------------
        // Unity Funtion
        private void Start()
        {
            _IsSetup = true;

            // Update Data
            _Player = DataManager.Instance.GetPlayerInfo(m_PlayerType);

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
                    _Player.SymbolType = (SymbolType)index;
                    RefreshToggle();
                    m_OpponentSetting.RefreshToggle();
                    UpdateData();
                }).AddTo(this);
            }
            _IsSetup = false;
        }
        // -------------------------------------------------------------------------------------
        // Public Funtion
        public void RefreshToggle()
        {
            for (int i = 0; i < m_SymbolToggles.Length; i++)
            {
                if(i != SymbolValue)
                {
                    if(i == m_OpponentSetting.SymbolValue)
                    {
                        m_SymbolToggles[i].interactable = false;
                    }
                    else
                    {
                        m_SymbolToggles[i].interactable = true;
                    }
                }
            }
        }
        public void RefreshSetting()
        {
            _IsSetup = true;
            
            // Set Controller Dropdown
            _Player = DataManager.Instance.GetPlayerInfo(m_PlayerType);
            m_ControllerDropdown.value = _Player.ControlerType.GetHashCode();
            
            // Set Symbol Toggles
            m_ToggleGroup.allowSwitchOff = true;
            foreach (var toggle in m_SymbolToggles)
            {
                toggle.interactable = true;
                toggle.isOn = false;
            }
            m_SymbolToggles[_Player.SymbolType.GetHashCode()].isOn = true;
            m_SymbolToggles[m_OpponentSetting.SymbolValue].interactable = false;
            
            m_ToggleGroup.allowSwitchOff = false;
            _IsSetup = false;
        }
        public void UpdateData()
        {
            DataManager.Instance.SetPlayerInfo(m_PlayerType, _Player);
        }
        public int SymbolValue => Player.SymbolType.GetHashCode();
        // -------------------------------------------------------------------------------------
        // Private Funtion

        // -------------------------------------------------------------------------------------
    }
}
