using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Miximum;

namespace TicTacToe
{
    public class AIPlayer : Player
    {
        // -------------------------------------------------------------------------------------
        private const int MAX_VALUE = 10000;
        private const int MIN_VALUE = -10000;
        // -------------------------------------------------------------------------------------
        public AIPlayer(PlayerName _playerType, SymbolType _symbol) : base(_playerType, _symbol)
        {
        }
        // -------------------------------------------------------------------------------------
        // Public Funtion
        public override IObservable<Position> MakeDicision(GameStage _stage, GameTime _gameTime)
        {
            return Observable.Create<Position>
            (
                _observer => 
                {
                    var gameTime = Observable.Timer(TimeSpan.FromSeconds((int)_gameTime)).Subscribe(_ => 
                    {
                        _observer.OnNext(new Position(-1, -1));
                        _observer.OnCompleted();
                    });
                    var disposable = Observable.Timer(TimeSpan.FromSeconds(0.5f)).Subscribe(_ => 
                    {
                        var cloneStage = _stage.Clone();
                        _observer.OnNext(FindBestSolution(cloneStage));
                        _observer.OnCompleted();
                    });
                    return Disposable.Create(() => 
                    {
                        gameTime?.Dispose();
                        disposable?.Dispose();
                    });
                }
            );
        }
        // -------------------------------------------------------------------------------------
        // Private Funtion
        private Position FindBestSolution(GameStage _stage)
        {
            var isMinimax = _stage.Players[0] != _stage.CurrentTurn;
            var bestList = new List<Position>();
            bestList.Add(_stage.RandomPosition());
            var bestValue = isMinimax ? MAX_VALUE : MIN_VALUE;
            var bestPos = _stage.RandomPosition();
            for(int i = 0; i < _stage.PosiblePosition.Count; i++)
            {
                var nextStage = _stage.Clone();
                nextStage.SelectPosition(_stage.PosiblePosition[i]);
                nextStage.SetNextPlayer();
                
                var value  = Minimax(3, 0, false, nextStage, MIN_VALUE, MAX_VALUE, _stage.CurrentTurn);
                if(isMinimax)
                {
                    if(bestValue > value)
                    {
                        bestList = new List<Position>();
                        bestValue = value;
                        bestPos = _stage.PosiblePosition[i];
                        bestList.Add(_stage.PosiblePosition[i]);
                    }
                    else if(bestValue == value)
                    {
                        bestList.Add(_stage.PosiblePosition[i]);
                    }
                }
                else
                {
                    if(bestValue < value)
                    {
                        bestList = new List<Position>();
                        bestValue = value;
                        bestPos = _stage.PosiblePosition[i];
                        bestList.Add(_stage.PosiblePosition[i]);
                    }
                    else if(bestValue == value)
                    {
                        bestList.Add(_stage.PosiblePosition[i]);
                    }
                }
            }
            return bestList[UnityEngine.Random.Range(0, bestList.Count)];
        }
        private int Minimax(int _depth, int _value, bool _IsMainPlayer, GameStage _stage, int _alpha, int _beta, PlayerName _mainPlayer)
        {
            if(_depth <= 0) return _value;

            if(_IsMainPlayer)
            {
                int best = MIN_VALUE;
                for(int i = 0; i < _stage.PosiblePosition.Count; i++)
                {
                    var nextStage = _stage.Clone();
                    nextStage.SelectPosition(_stage.PosiblePosition[i]);
                    var wonPlayer = nextStage.CheckWonPlayer();
                    var reward = _mainPlayer == wonPlayer ? 2 : 1;
                    _value += wonPlayer != PlayerName.None ? reward : 0;
                    nextStage.SetNextPlayer();

                    int value = Minimax(_depth-1, _value, false, _stage, _alpha, _beta, _mainPlayer);
                    
                    best = Math.Max(best, value);
                    _alpha = Math.Max(_alpha, best);

                    if(_beta <= _alpha)
                        break;
                }
                return best;
            }
            else
            {
                int best = MAX_VALUE;
                for(int i = 0; i < _stage.PosiblePosition.Count; i++)
                {
                    var nextStage = _stage.Clone();
                    nextStage.SelectPosition(_stage.PosiblePosition[i]);
                    _value += nextStage.CheckWonPlayer() != PlayerName.None ? 1 : 0;

                    int value = Minimax(_depth-1, _value, true, _stage, _alpha, _beta, _mainPlayer);

                    best = Math.Min(best, value);
                    _beta = Math.Min(_beta, best);

                    if(_beta <= _alpha)
                        break;
                }
                return best;
            }
        }
        // -------------------------------------------------------------------------------------
    }
}
