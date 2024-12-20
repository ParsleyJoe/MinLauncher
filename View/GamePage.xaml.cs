using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace gameLauncher.View
{
    public partial class GamePage : UserControl
    {
        bool running = false;
        string selectedGame = "Game";
        string pathToGame = "";
        public GamePage()
        {
            InitializeComponent();
        }

        private void RunGame_Click(object sender, RoutedEventArgs e)
        {
            if (!running)
            {
                RunBtn.Content = "Running";
                if (Path.Exists(pathToGame))
                {
                    Process.Start(pathToGame);
                    running = true;
                }
                else
                {
                    // Does not Exist
                }
            }
            else if (running)
            {
                RunBtn.Content = "Run";
                running = false;
            }
        }

        void GameSelectionChange()
        {
            GameList gameList = new GameList();
            gameList.GameSelected += ChangeSelectedGame;
        }

        void ChangeSelectedGame(Game game)
        {
            selectedGame = game.name;
        }
       
    }
}
