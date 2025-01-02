using gameLauncher.Classes;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace gameLauncher.View
{
    public partial class GameList : UserControl
    {
        public ObservableCollection<string> GamesDataStr = new();
        string PathToSaveFile = Path.GetFullPath("StoredData.txt");
        string SelectedGame = "";
        string PathToGame = "";
        bool running = false;
        Process StartedGame = new Process();

        // Constructor 
        public GameList()
        {
            DataContext = this;
            LoadData();
            InitializeComponent();
            gameListView.ItemsSource = GamesDataStr;
        }

        #region GameAdd()
        // Adds Game to GamesDataList and GamesDataStr ObserCollec
        public void GameAdd(OpenFileDialog dialog)
        {
            Globals.GamesData.Add(new Game(Path.GetFileNameWithoutExtension(dialog.SafeFileName), dialog.FileName));
            GamesDataStr.Add(Globals.GamesData.Last().name);
        }
        // Same Logic but takes strings as parameters
        public void GameAdd(string name, string pathToGame)
        {
            if (name == null || pathToGame == null) { return; }
            Globals.GamesData.Add(new Game(Path.GetFileNameWithoutExtension(name), pathToGame));
            GamesDataStr.Add(Globals.GamesData.Last().name);
        }
        #endregion

        // Update Page for Game when selected game is changed
        private void GameList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // gameListView.SelectedIndex == -1 when GamesDataStr changed ,idk why
            if (gameListView.SelectedIndex == -1 ) { return; }
            SelectedGame = Globals.GamesData[gameListView.SelectedIndex].name;
            PathToGame = Globals.GamesData[gameListView.SelectedIndex].pathToGame;
            GameName.Text = SelectedGame;
        }

        #region ButtonClicks
        private void AddGame_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() ?? false)
            {
                GameAdd(dialog);
            }
            WriteData();
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

        private void MenuItemRename_Click(object sender, RoutedEventArgs e)
        {
            RenamePopup.IsOpen = true;
        }

        private void RenameOkBtn_Click(object sender, RoutedEventArgs e)
        {
            Globals.GamesData[gameListView.SelectedIndex].name = RenamePopupText.Text;
            GamesDataStr[gameListView.SelectedIndex] = RenamePopupText.Text;
            gameListView.UpdateDefaultStyle();
            RenamePopup.IsOpen = false;
            WriteData();
        }

        private void RenameCancelBtn_Click(object sender, RoutedEventArgs e)
        {
            RenamePopup.IsOpen = false;
        }

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            Globals.GamesData.Remove(Globals.GamesData[gameListView.SelectedIndex]);
            GamesDataStr.Remove(GamesDataStr[gameListView.SelectedIndex]);
            WriteData();
        }
        #endregion

        #region SaveHandling
        // Write data to a File
        void WriteData()
        {
            StreamWriter sw = new StreamWriter(PathToSaveFile, false);
            foreach (Game game in Globals.GamesData)
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
            StreamReader sr = new StreamReader(PathToSaveFile);
            string name, path;
            if (sr == null)
            {
                return;
            }
            while (sr.Peek() >= 0)
            {
                name = sr.ReadLine();
                path = sr.ReadLine();
                GameAdd(name, path);
            }
            sr?.Close();
        }
        #endregion

    }
}

