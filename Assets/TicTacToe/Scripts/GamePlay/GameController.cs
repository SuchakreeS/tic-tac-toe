using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Miximum;

namespace TicTacToe
{
    public enum GameTime
    {
        Infinity = 999999,
        FiveSeconds = 5,
        TenSeconds = 10,
        ThirtySeconds = 30,
    }
    public class GameController : MonoBehaviour
    {
        // -------------------------------------------------------------------------------------
        [SerializeField] private UIGamePlay m_UIGamePlay;
        [SerializeField] private UIBoard m_UIBoard;
        [SerializeField] private GameTime m_GameTime;
        // -------------------------------------------------------------------------------------
        private List<GameStage> m_GameStageList;
        private Player[] _Players;
        private bool _Status;
        private int _TurnCount;
        private PlayerName _CurrentTurn;
        private Position _Selected;
        private BoardSize _BoardSize;
        private PlayerName[] _PlayersName;
        // -------------------------------------------------------------------------------------
        // Unity Funtion
        private void Start()
        {
        }
        // -------------------------------------------------------------------------------------
        // Public Funtion
        // Call this function for 
        public void GameReset()
        {
            // Load Data
            var playersInfo = DataManager.PlayersInfo;

            // Init Variable
            m_GameStageList = new List<GameStage>();
            _PlayersName = new PlayerName[playersInfo.Length];
            _Players = new Player[playersInfo.Length];
            _BoardSize = DataManager.BoardSize;
            _TurnCount = 0;

            for (int i = 0; i < playersInfo.Length; i++)
            {
                if(playersInfo[i].Controller == ControllerType.Human)
                {
                    _Players[i] = new HumanPlayer(playersInfo[i].Name, playersInfo[i].Symbol, m_UIBoard);
                }
                else if(playersInfo[i].Controller == ControllerType.AI)
                {
                    _Players[i] = new AIPlayer(playersInfo[i].Name, playersInfo[i].Symbol);
                }
                _PlayersName[i] = playersInfo[i].Name;
            }
            var boardData = new int[(int)_BoardSize, (int)_BoardSize];

            // Create Stage
            var stage = new GameStage(_BoardSize, boardData, _PlayersName, _Players[_TurnCount%_PlayersName.Length].PlayerName);
            m_GameStageList.Add(stage);
        }
        public void GameStart()
        {
            
        }
        public void GamePause()
        {

        }
        public void GameForceEnd()
        {
            
        }
        // -------------------------------------------------------------------------------------
        // Private Funtion

        // -------------------------------------------------------------------------------------
    }
}
