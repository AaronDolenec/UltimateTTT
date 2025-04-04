using System;

namespace SuperTicTacToe
{
    public class ServerController
    {
        List<Player> players = new List<Player>();
        public void AddPlayer(string pName)
        {
            Player player = new Player(pName);
            players.Add(player);
            if (players.Count == 2)
            {
                new Server(players[0], players[1]);
                // Start the game
            }
        }
    }

    public class Player(string pName)
    {
        public string Name { get; set; }
        public Player(string pName)
        {
            Name = pName;
        }

    }

    public class Server(Player pPlayer1, Player pPlayer2)
    {
        Player player1 = pPlayer1;
        Player player2 = pPlayer2;
    }
}