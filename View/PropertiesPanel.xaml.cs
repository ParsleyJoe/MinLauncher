using gameLauncher.Classes;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;
using System.Windows.Controls;

namespace gameLauncher.View
{
    public partial class PropertiesPanel : UserControl
    {
        public PropertiesPanel()
        {
            InitializeComponent();
        }

        private void PropertiesMenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            GameName.Text = Globals.SelectedGame;
            string pathGame = "";
            foreach(Game game in Globals.GamesData)
            {
                if (game.name == Globals.SelectedGame)
                {
                    pathGame = game.pathToGame;
                }
            }
            GamePath.Text = pathGame;
        }
    }
}
