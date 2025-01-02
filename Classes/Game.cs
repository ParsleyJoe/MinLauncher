using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameLauncher.Classes
{
    public class Game
    {
        public string name;
        public string pathToGame;

        public Game(string name, string pathToGame)
        {
            this.name = name;
            this.pathToGame = pathToGame;
        }
    }

    public static class Globals
    {
        public static List<Game> GamesData = new List<Game>();

    }
}
