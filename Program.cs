
using System; // This is a test and another

namespace SuperTicTacToe
{
    public enum Player
    {
        None,
        X,
        O
    }

    public enum GameMode
    {
        OfficialRules,
        AlternativeRules
    }

    public class Game
    {
        private Player[,] _boards;
        private Player[,] _superBoard;
        private Player _currentPlayer;
        private int _nextSuperRow;
        private int _nextSuperCol;
        private GameMode _gameMode;

        public Game(GameMode gameMode)
        {
            _boards = new Player[9, 9];
            _superBoard = new Player[3, 3];
            _currentPlayer = Player.X;
            _nextSuperRow = -1; // Indicates any super board can be played on the first move
            _nextSuperCol = -1;
            _gameMode = gameMode;
        }

        public Player CurrentPlayer => _currentPlayer;

        public int NextSuperRow => _nextSuperRow;

        public int NextSuperCol => _nextSuperCol;

        public Player[,] Boards => _boards;

        public Player[,] SuperBoard => _superBoard;

        public GameMode Mode => _gameMode;

        public bool IsSubBoardFull(int superRow, int superCol)
        {
            if (superRow < 0 || superRow >= 3 || superCol < 0 || superCol >= 3)
            {
                return false;
            }

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (_boards[superRow * 3 + row, superCol * 3 + col] == Player.None)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public Player GetSuperBoardStatus(int superRow, int superCol)
        {
            if (superRow < 0 || superRow >= 3 || superCol < 0 || superCol >= 3)
            {
                return Player.None;
            }

            return _superBoard[superRow, superCol];
        }

        public bool MakeMove(int superRow, int superCol, int row, int col)
        {
            if (_nextSuperRow != -1 && (superRow != _nextSuperRow || superCol != _nextSuperCol))
            {
                throw new ArgumentException("You must play in the designated super board.");
            }

            if (superRow < 0 || superRow >= 3 || superCol < 0 || superCol >= 3)
            {
                throw new ArgumentException("Invalid super board position");
            }

            if (row < 0 || row >= 3 || col < 0 || col >= 3)
            {
                throw new ArgumentException("Invalid board position");
            }

            if (_superBoard[superRow, superCol] != Player.None && (_gameMode == GameMode.OfficialRules || !IsSubBoardFull(superRow, superCol)))
            {
                throw new ArgumentException("Super board position is already occupied");
            }

            if (_boards[superRow * 3 + row, superCol * 3 + col] != Player.None)
            {
                throw new ArgumentException("Board position is already occupied");
            }

            _boards[superRow * 3 + row, superCol * 3 + col] = _currentPlayer;

            // Check if the current player has won the super board
            if (CheckSuperBoardWin(superRow, superCol))
            {
                _superBoard[superRow, superCol] = _currentPlayer;
            }

            // Update next super board position
            _nextSuperRow = row;
            _nextSuperCol = col;

            _currentPlayer = _currentPlayer == Player.X ? Player.O : Player.X;

            return true;
        }

        public bool CheckSuperBoardWin(int superRow, int superCol)
        {
            // Check rows
            for (int i = 0; i < 3; i++)
            {
                if (_boards[superRow * 3 + i, superCol * 3] == _boards[superRow * 3 + i, superCol * 3 + 1] &&
                    _boards[superRow * 3 + i, superCol * 3 + 1] == _boards[superRow * 3 + i, superCol * 3 + 2] &&
                    _boards[superRow * 3 + i, superCol * 3] != Player.None)
                {
                    return true;
                }
            }

            // Check columns
            for (int i = 0; i < 3; i++)
            {
                if (_boards[superRow * 3, superCol * 3 + i] == _boards[superRow * 3 + 1, superCol * 3 + i] &&
                    _boards[superRow * 3 + 1, superCol * 3 + i] == _boards[superRow * 3 + 2, superCol * 3 + i] &&
                    _boards[superRow * 3, superCol * 3 + i] != Player.None)
                {
                    return true;
                }
            }

            // Check diagonals
            if (_boards[superRow * 3, superCol * 3] == _boards[superRow * 3 + 1, superCol * 3 + 1] &&
                _boards[superRow * 3 + 1, superCol * 3 + 1] == _boards[superRow * 3 + 2, superCol * 3 + 2] &&
                _boards[superRow * 3, superCol * 3] != Player.None)
            {
                return true;
            }

            if (_boards[superRow * 3, superCol * 3 + 2] == _boards[superRow * 3 + 1, superCol * 3 + 1] &&
                _boards[superRow * 3 + 1, superCol * 3 + 1] == _boards[superRow * 3 + 2, superCol * 3] &&
                _boards[superRow * 3, superCol * 3 + 2] != Player.None)
            {
                return true;
            }

            return false;
        }
    }
}