using System;

namespace TicTacToe
{
    public abstract class Player
    {
        // -------------------------------------------------------------------------------------
        public int Score;
        public PlayerName PlayerName;
        public SymbolType Symbol;
        // -------------------------------------------------------------------------------------
        public Player(PlayerName playerType, SymbolType _symbol)
        {
            PlayerName = playerType;
            Symbol = _symbol;
        }
        // -------------------------------------------------------------------------------------
        // Public Funtion
        public abstract IObservable<Position> MakeDicision(GameStage _stage, GameTime _gameTime);
        // -------------------------------------------------------------------------------------
    }
}
