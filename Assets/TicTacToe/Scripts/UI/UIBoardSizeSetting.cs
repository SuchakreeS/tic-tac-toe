using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Miximum;
using UnityEngine.UI;

namespace TicTacToe
{
    public class UIBoardSizeSetting : MonoBehaviour
    {
        // -------------------------------------------------------------------------------------
        [Serializable]
        public struct ToggleInfo
        {
            public BoardSize BoardSize;
            public Toggle Toggle;
        }
        // -------------------------------------------------------------------------------------

        [SerializeField] private ToggleInfo[] ToggleInfos;
        // -------------------------------------------------------------------------------------
        private BoardSize _BoardSize;
        private bool _IsSetup;
        private Dictionary<BoardSize, Toggle> _ToggleDict = new Dictionary<BoardSize, Toggle>();
        
        // -------------------------------------------------------------------------------------
        // Unity Funtion
        private void Start()
        {
            foreach (var info in ToggleInfos)
            {
                _ToggleDict[info.BoardSize] = info.Toggle;
            }
            _IsSetup = true;
            _BoardSize = DataManager.BoardSize;
            // Toggle
            foreach (var boardSize in _ToggleDict.Keys)
            {
                _ToggleDict[boardSize].OnValueChangedAsObservable().Where(_value => _value && !_IsSetup).Subscribe(_value => 
                {
                    _BoardSize = boardSize;
                    UpdateData();
                }).AddTo(this);
            }
            _IsSetup = false;
        }

        // -------------------------------------------------------------------------------------
        // Public Funtion
        // -------------------------------------------------------------------------------------
        // Private Funtion
        private void UpdateData()
        {
            DataManager.Instance.SetBoardSize(_BoardSize);
        }
        // -------------------------------------------------------------------------------------
    }
}
