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
        [SerializeField] private Toggle[] m_SymbolToggles;
        // -------------------------------------------------------------------------------------
        private PlayerInfo _Player;
        // -------------------------------------------------------------------------------------
        // Unity Funtion
        private void Start()
        {
            var isSetup = true;
            // Dropdown
            m_ControllerDropdown.OnValueChangedAsObservable().Where(_ => !isSetup).Subscribe(_value => 
            {
                _Player.ControlerType = (ControllerType)_value;
                UpdateData();
            }).AddTo(this);

            // Toggle
            for (int i = 0; i < m_SymbolToggles.Length; i++)
            {
                var index = i;
                m_SymbolToggles[i].OnValueChangedAsObservable().Where(_value => _value && !isSetup).Subscribe(_value => 
                {
                    Debug.Log((SymbolType)index);
                }).AddTo(this);
            }
            isSetup = false;
        }
        // -------------------------------------------------------------------------------------
        // Public Funtion
        public void UpdateData()
        {
            DataController.Instance.UpdatePlayerInfo(m_PlayerType, _Player);
        }
        // -------------------------------------------------------------------------------------
        // Private Funtion

        // -------------------------------------------------------------------------------------
    }
}
