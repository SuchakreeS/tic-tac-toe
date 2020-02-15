using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Miximum;

namespace TicTacToe
{
    public class HumanPlayer : Player
    {
        private UIBoard _UIBoard;
        // -------------------------------------------------------------------------------------
        public HumanPlayer(PlayerName _playerType, SymbolType _symbol, UIBoard _uiBoard) : base(_playerType, _symbol)
        {
            _UIBoard = _uiBoard;
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

                    var dicision = _UIBoard.OnClickBoardAsObservable().Subscribe(_position => 
                    {
                        _observer.OnNext(_position);
                        _observer.OnCompleted();
                    });
                    
                    return Disposable.Create(() => 
                    {
                        gameTime?.Dispose();
                        dicision?.Dispose();
                    });
                }
            );
        }
        // -------------------------------------------------------------------------------------
        // Private Funtion

        // -------------------------------------------------------------------------------------
    }
}
