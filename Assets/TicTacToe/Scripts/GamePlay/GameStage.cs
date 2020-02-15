using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Miximum;
using System.Diagnostics;

namespace TicTacToe
{
    public class GameStage
    {
        public enum StageStatus
        {
            Waiting,
            Selected,
            MatchOver
        }
        // -------------------------------------------------------------------------------------
        private int[,] _BoardData;
        private Position _Selected;
        private BoardSize _BoardSize;
        private PlayerName[] _Players;
        private PlayerName _CurrentTurn;
        private StageStatus _Status;

        public GameStage(BoardSize _boardSize, int[,] _boardData, PlayerName[] _players, PlayerName _currentTurn)
        {
            _BoardSize = _boardSize;
            _Players = _players;
            _CurrentTurn = _currentTurn;
            _BoardData = _boardData;
            _Status = StageStatus.Waiting;
        }
        // -------------------------------------------------------------------------------------
        public void SelectPosition()
        {

        }
        // -------------------------------------------------------------------------------------
        // Public Funtion

        // -------------------------------------------------------------------------------------
        // Private Funtion
        private void CheckStatus()
        {
            
        }
        // -------------------------------------------------------------------------------------
    }
}
