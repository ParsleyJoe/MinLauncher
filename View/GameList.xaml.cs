using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;

namespace gameLauncher.View
{
    public partial class GameList : UserControl
    {
        public List<Game> GamesData = new List<Game>();
        string SelectedGame = "";
        string PathToGame = "";
        bool running = false;
        Process StartedGame = new Process();
        public GameList()
        {
            InitializeComponent();
        }

        private void AddGame_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() ?? false)
            {
                GameAdd(dialog);
            }

        }

        public void GameAdd(OpenFileDialog dialog)
        {
            GamesData.Add(new Game(dialog));
            gameListView.Items.Add(GamesData.Last().name);
        }

        private void GameList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedGame = GamesData[gameListView.SelectedIndex].name;
            PathToGame = GamesData[gameListView.SelectedIndex].pathToGame;
            Name.Text = SelectedGame;
        }

        private void RunBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Path.Exists(PathToGame) && !running)
            {
                StartedGame = Process.Start(PathToGame);
                if (StartedGame != null)
                {
                    RunBtn.Content = "Running..";
                    running = true;
                }
                return;
            }

            if (running)
            {
                if (!StartedGame.HasExited)
                {
                    StartedGame.Kill();
                }
                running = false;
                RunBtn.Content = "Run";
                return;
            }
        }

        public class Game
        {
            public string name;
            public string pathToGame;

            public Game(OpenFileDialog dialog)
            {
                name = Path.GetFileNameWithoutExtension(dialog.SafeFileName);
                pathToGame = dialog.FileName;
            }

        }
    }
}

