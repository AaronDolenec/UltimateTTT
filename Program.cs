
using System; 

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
        private Player[,] _boards; // Super board initialization
        private Player[,] _superBoard; // Super board status
        private Player _currentPlayer; // Current player
        private int _nextSuperRow; // Next super board row
        private int _nextSuperCol; // Next super board column
        private GameMode _gameMode; // Game mode (official or alternative)

        public Game(GameMode gameMode)
        {
            _boards = new Player[9, 9]; // 9x9 board for super board
            _superBoard = new Player[3, 3]; // 3x3 board for super board status
            _currentPlayer = Player.X; // Start with player X
            _nextSuperRow = -1; // Indicates any super board can be played on the first move
            _nextSuperCol = -1; // Indicates any super board can be played on the first move
            _gameMode = gameMode;
        }

        // Getter for Properties

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
                        return false; // If any square is empty, the sub-board is not full
                    }
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public Player GetSuperBoardStatus(int superRow, int superCol) // Check Winner of sub-board board[0-2,0-2]
        {
            if (superRow < 0 || superRow >= 3 || superCol < 0 || superCol >= 3)
            {
                return Player.None;
            }

            return _superBoard[superRow, superCol];
        }

        public bool MakeMove(int superRow, int superCol, int row, int col)
        {
            if (_nextSuperRow != -1 && (superRow != _nextSuperRow || superCol != _nextSuperCol)) // Check if the move is on correct sub-board
            {
                throw new ArgumentException("You must play in the designated super board.");
            }

            if (superRow < 0 || superRow >= 3 || superCol < 0 || superCol >= 3) // Check if the super board position is valid
            {
                throw new ArgumentException("Invalid super board position");
            }

            if (row < 0 || row >= 3 || col < 0 || col >= 3) // Check sub-board position
            {
                throw new ArgumentException("Invalid board position");
            }

            if (_superBoard[superRow, superCol] != Player.None && (_gameMode == GameMode.OfficialRules || !IsSubBoardFull(superRow, superCol))) // Check if sub-board is full or won in official rules
            {
                throw new ArgumentException("Super board position is already occupied");
            }

            if (_boards[superRow * 3 + row, superCol * 3 + col] != Player.None) // Check if the position is already occupied
            {
                throw new ArgumentException("Board position is already occupied");
            }

            _boards[superRow * 3 + row, superCol * 3 + col] = _currentPlayer; // Place the current player's mark on the board

            // Check if the current player has won the super board
            if (CheckSuperBoardWin(superRow, superCol))
            {
                _superBoard[superRow, superCol] = _currentPlayer;
            }

            // Update next super board position
            _nextSuperRow = row;
            _nextSuperCol = col;

            _currentPlayer = _currentPlayer == Player.X ? Player.O : Player.X; // Switch players

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