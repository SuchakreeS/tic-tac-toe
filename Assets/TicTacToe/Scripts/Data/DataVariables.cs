using System;

namespace TicTacToe
{
    public enum PlayerName
    {
        None = -1,
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
        Heart,
        None = -1
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
        public static Position None => new Position(-1, -1);
        public override string ToString() => $"[{Row}, {Column}]";
        public static bool operator==(Position _x, Position _y) => _x.Row == _y.Row && _x.Column == _y.Column;
        public static bool operator!=(Position _x, Position _y) => _x.Row != _y.Row || _x.Column != _y.Column;

        public override int GetHashCode() => base.GetHashCode();
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            return base.Equals (obj);
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


