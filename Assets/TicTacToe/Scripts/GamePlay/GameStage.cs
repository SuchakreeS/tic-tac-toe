﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Miximum;
using System.Diagnostics;
namespace TicTacToe
{
    public enum StageStatus
    {
        Waiting,
        Selected,
        MatchOver
    }
    public class GameStage
    {
        
        // -------------------------------------------------------------------------------------
        private int[,] _BoardData;
        private Position _Selected;
        private BoardSize _BoardSize;
        private PlayerName[] _Players;
        private PlayerName _CurrentTurn;
        private StageStatus _Status;
        private List<Position> _PosiblePosition;
        private PlayerName _WonPlayer;
        private int _WinAmount;
        // -------------------------------------------------------------------------------------
        public PlayerName[] Players => _Players;
        public Position Selected => _Selected;
        public PlayerName WonPlayer => _WonPlayer;
        public StageStatus Status => _Status;
        public PlayerName CurrentTurn => _CurrentTurn;
        public int[,] BoardData => _BoardData;
        public List<Position> PosiblePosition => _PosiblePosition;
        public int WinAmount => _WinAmount;
        // -------------------------------------------------------------------------------------
        public GameStage(BoardSize _boardSize, int[,] _boardData, PlayerName[] _players, PlayerName _currentTurn, int _winAmount = -1)
        {
            _BoardSize = _boardSize;
            _Players = _players;
            _CurrentTurn = _currentTurn;
            _BoardData = _boardData;
            CreatePosiblePositions();

            if(_winAmount == -1 || _winAmount > _WinAmount)
                _WinAmount = (int)_boardSize;
            else
                _WinAmount = _winAmount;
            _WonPlayer = CheckWonPlayer();
            _Status = CheckStatus();
        }
        // -------------------------------------------------------------------------------------
        // Public Funtion
        public bool SelectPosition(Position _position)
        {
            if(_position == Position.None)
                _position = RandomPosition();
            if(_BoardData[_position.Row, _position.Column] == 0 && _Status == StageStatus.Waiting)
            {
                _BoardData[_position.Row, _position.Column] = (int)_CurrentTurn;
                _Selected = _position;
                _Status = StageStatus.Selected;
                return true;
            }
            else
                return false;
        }
        public bool CheckPosibleDecision(Position _position)
        {
            if(_BoardData[_position.Row, _position.Column] == 0)
                return true;
            else
                return false;
        }
        public Position RandomPosition()
        {
            if(_PosiblePosition.Count > 0)
            {
                var random = UnityEngine.Random.Range(0, _PosiblePosition.Count);
                return _PosiblePosition[random];
            }
            return new Position(-1, -1);
        }
        public bool Undo()
        {
            if(_Status == StageStatus.Selected && _Selected != Position.None)
            {
                _BoardData[_Selected.Row, _Selected.Column] = 0;
                UnityEngine.Debug.Log($"[{_Selected.ToString()}]: {_BoardData[_Selected.Row, _Selected.Column]}");
                _Selected = Position.None;
                _Status = StageStatus.Waiting;
                return true;
            }
            return false;
        }
        public GameStage Clone()
        {
            var boardData = new int[(int)_BoardSize, (int)_BoardSize];
            for (int i = 0; i < (int)_BoardSize; i++)
            {
                for (int j = 0; j < (int)_BoardSize; j++)
                {
                    boardData[i, j] = _BoardData[i, j];
                }
            }
            return new GameStage(_BoardSize, boardData, _Players, _CurrentTurn, _WinAmount);
        }
        public void SetNextPlayer()
        {
            for (int i = 0; i < _Players.Length; i++)
            {
                if(_CurrentTurn == _Players[i])
                {
                    _CurrentTurn = _Players[(i+1)%_Players.Length];
                }
            }
        }
        public void PrintStage()
        {
            var stage = "";
            for (int i = 0; i < (int)_BoardSize; i++)
            {
                for (int j = 0; j < (int)_BoardSize; j++)
                {
                    stage += $" {_BoardData[i,j]}";
                }
                stage += "\n";
            }
            UnityEngine.Debug.Log(stage);
        }
        public StageStatus CheckStatus()
        {
            // Check Won Player
            if(_WonPlayer != PlayerName.None)
                return StageStatus.MatchOver;

            // Check Match Over
            if(_PosiblePosition.Count > 0)
                return StageStatus.Waiting;
            else
                return StageStatus.MatchOver;
        }
        public PlayerName CheckWonPlayer()
        {
            for (int i = 0; i < (int)_BoardSize; i++)
            {
                for (int j = 0; j < (int)_BoardSize; j++)
                {
                    var target = -1;
                    bool winRow = true;
                    bool winCol = true;
                    bool winRightDiagonal = true;
                    bool winLeftDiagonal = true;
                    for (int k = 0; k < _WinAmount; k++)
                    {
                        if(k == 0)
                        {
                            if(_BoardData[i,j] == 0)
                            {
                                winRow = false; 
                                winCol = false; 
                                winRightDiagonal = false; 
                                winLeftDiagonal = false;
                                break;
                            }
                            else
                            {
                                target = _BoardData[i,j];
                                continue;
                            }
                        }

                        if(j+k < (int)_BoardSize)
                        {
                            if(_BoardData[i,j+k] != target)
                            {
                                winCol = false;
                            }
                        }
                        else
                        {
                            winCol = false;
                        }

                        if(i+k < (int)_BoardSize)
                        {
                            if(_BoardData[i+k,j] != target)
                            {
                                winRow = false;
                            }
                        }
                        else
                        {
                            winRow = false;
                        }

                        if(i+k < (int)_BoardSize && j+k < (int)_BoardSize)
                        {
                            if(_BoardData[i+k,j+k] != target)
                            {
                                winRightDiagonal = false;
                            }
                        }
                        else
                        {
                            winRightDiagonal = false;
                        }

                        if(i+k < (int)_BoardSize && j-k >= 0)
                        {
                            if(_BoardData[i+k,j-k] != target)
                            {
                                winLeftDiagonal = false;
                            }
                        }
                        else
                        {
                            winLeftDiagonal = false;
                        }
                    }
                    if(winRow || winCol || winRightDiagonal || winLeftDiagonal)
                    {
                        return (PlayerName) target;
                    }
                }
                
            }
            return PlayerName.None;
        }
        // -------------------------------------------------------------------------------------
        // Private Funtion
        private void CreatePosiblePositions()
        {
            _PosiblePosition = new List<Position>();
            for (int i = 0; i < (int)_BoardSize; i++)
                for (int j = 0; j < (int)_BoardSize; j++)
                    if(_BoardData[i,j] == 0)
                        _PosiblePosition.Add(new Position(i, j));
        }
        // -------------------------------------------------------------------------------------
    }
}
