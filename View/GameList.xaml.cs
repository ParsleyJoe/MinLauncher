using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
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
            LoadData();
        }

        private void AddGame_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() ?? false)
            {
                GameAdd(dialog);
            }

            WriteData();
        }

        // Adds Game to List and XAML ListView
        public void GameAdd(OpenFileDialog dialog)
        {
            GamesData.Add(new Game(Path.GetFileNameWithoutExtension(dialog.SafeFileName), dialog.FileName));
            gameListView.Items.Add(GamesData.Last().name);
        }
        // Same Logic but takes strings as parameters
        public void GameAdd(string name, string pathToGame)
        {
            if (name == null || pathToGame == null) { return; }
            GamesData.Add(new Game(Path.GetFileNameWithoutExtension(name), pathToGame));
            gameListView.Items.Add(GamesData.Last().name);
        }

        private void GameList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedGame = GamesData[gameListView.SelectedIndex].name;
            PathToGame = GamesData[gameListView.SelectedIndex].pathToGame;
            GameName.Text = SelectedGame;
        }

        // Run Game
        private void RunBtn_Click(object sender, RoutedEventArgs e)
        {
            // If path exists run and return out of function
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

            // If already running try to kill game, return.
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

        // Write data to a File
        void WriteData()
        {
            StreamWriter sw = new StreamWriter("StoredData.txt", false);
            foreach (Game game in GamesData)
            {
                if (sw != null)
                {
                    sw.WriteLine(game.name);
                    sw.WriteLine(game.pathToGame);
                }
            }
            sw?.Close();
        }

        void LoadData()
        {
            StreamReader sr = new StreamReader("StoredData.txt");
            string name, path;
            while (sr.Peek() >= 0)
            {
                name = sr.ReadLine();
                path = sr.ReadLine();
                GameAdd(name, path);
            }

        }

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


        private void MenuItemRename_Click(object sender, RoutedEventArgs e)
        {
            RenamePopup.IsOpen = true;

        }
    }
}

