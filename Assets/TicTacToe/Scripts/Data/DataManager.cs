using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Miximum;

namespace TicTacToe
{
    public class DataManager : Singleton<DataManager>
    {
        // -------------------------------------------------------------------------------------
        [Header("Default Setting")]
        [SerializeField] private PlayerInfo[] m_PlayerInfoDefault;
        [SerializeField] private BoardSize m_MatchSizeDefault;
        // -------------------------------------------------------------------------------------
        private static PlayerInfo[] _PlayersInfo;
        private static BoardSize _BoardSize;
        // -------------------------------------------------------------------------------------
        public static PlayerInfo[] PlayersInfo => _PlayersInfo;
        public static BoardSize BoardSize => _BoardSize;
        // -------------------------------------------------------------------------------------
        // Unity Funtion
        public override void Awake()
        {
            base.Awake();
            // Set Default 
            _PlayersInfo = m_PlayerInfoDefault;
            _BoardSize = m_MatchSizeDefault;
        }
        // -------------------------------------------------------------------------------------
        // Public Funtion
        public static PlayerInfo GetPlayerInfo(PlayerName _playerName)
        {
            foreach(var info in _PlayersInfo)
            {
                if(_playerName == info.Name)
                {
                    return info;
                }
            }
            return _PlayersInfo[0];
        }
        public static void SetPlayerInfo(PlayerName _playerName, PlayerInfo _playerInfo)
        {
            for (int i = 0; i < _PlayersInfo.Length; i++)
            {
                if (_playerName == _PlayersInfo[i].Name)
                {
                    _PlayersInfo[i] = _playerInfo;
                }
            }
        }
        public void SetBoardSize(BoardSize _boardSize)
        {
            _BoardSize = _boardSize;
        }
        // -------------------------------------------------------------------------------------
        // Private Funtion

        // -------------------------------------------------------------------------------------
    }
}
