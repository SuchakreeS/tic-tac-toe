using System;

namespace TicTacToe
{
    public enum PlayerName
    {
        Player1 = 1,
        Player2 = 2
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
    public enum BoardSize
    {
        SIZE_3x3 = 3,
        SIZE_4x4 = 4
    }
    public struct Position
    {
        public int Row;
        public int Column;

        public Position(int _row, int _column)
        {
            Row = _row;
            Column = _column;
        }
    }
    [Serializable]
    public struct PlayerInfo
    {
        public PlayerName Name;
        public SymbolType Symbol;
        public ControllerType Controller;
    }
}


