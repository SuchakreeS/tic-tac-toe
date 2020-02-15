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
                    
                    return Disposable.Create(() => 
                    {
                        gameTime?.Dispose();
                    });
                }
            );
        }
        // -------------------------------------------------------------------------------------
        // Private Funtion

        // -------------------------------------------------------------------------------------
    }
}
