using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Miximum;
using System;

namespace TicTacToe
{
    public enum PlayerType
    {
        Player1,
        Player2
    }
    public enum ControllerType
    {
        Human,
        AI
    }
    public enum SymbolType
    {
        Chip,
        Club,
        Diamond,
        Heart
    }
    public enum MatchSize
    {
        SIZE_3x3,
        SIZE_4x4
    }
    [Serializable]
    public struct PlayerInfo
    {
        public ControllerType ControlerType;
        public SymbolType SymbolType;
    }
    public class DataController : Singleton<DataController>
    {
        // -------------------------------------------------------------------------------------
        [Header("Default Setting")]
        [SerializeField] private PlayerInfo m_Player1Default;
        [SerializeField] private PlayerInfo m_Player2Default;
        [SerializeField] private MatchSize m_MatchSizeDefault;
        // -------------------------------------------------------------------------------------
        private PlayerInfo _Player1;
        private PlayerInfo _Player2;
        private MatchSize _MatchSize;
        // -------------------------------------------------------------------------------------
        public PlayerInfo Player1 => _Player1;
        public PlayerInfo Player2 => _Player2;
        public MatchSize MatchSize => _MatchSize;
        // -------------------------------------------------------------------------------------
        // Unity Funtion
        public override void Awake()
        {
            base.Awake();
            // Set Default 
            _Player1 = m_Player1Default;
            _Player2 = m_Player2Default;
            _MatchSize = m_MatchSizeDefault;
        }
        private void Start()
        {
            // Load Data
        }
        // -------------------------------------------------------------------------------------
        // Public Funtion
        public void LoadData()
        {
        }
        public PlayerInfo GetPlayerInfo(PlayerType _playerType)
        {
            if(_playerType == PlayerType.Player1)
            {
                return _Player1;
            }
            else if(_playerType == PlayerType.Player2)
            {
                return _Player2;
            }
            return _Player1;
        }
        public void SetPlayerInfo(PlayerType _playerType, PlayerInfo _playerInfo)
        {
            if(_playerType == PlayerType.Player1)
            {
                _Player1 = _playerInfo;
            }
            else if(_playerType == PlayerType.Player2)
            {
                _Player2 = _playerInfo;
            }
        }
        // -------------------------------------------------------------------------------------
        // Private Funtion

        // -------------------------------------------------------------------------------------
    }
}
