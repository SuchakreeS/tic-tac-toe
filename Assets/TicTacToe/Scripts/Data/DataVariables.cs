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
        public ControllerType ControlerType;
        public SymbolType SymbolType;
    }
}


