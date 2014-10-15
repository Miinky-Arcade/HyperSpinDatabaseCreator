using Avalon.Windows.Dialogs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace HyperSpinDatabaseCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PickDatabasePath_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.DefaultExt = ".xml";
            dialog.Filter = "XML documents (.xml)|*.xml";

            var result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                DatabasePath.Text = dialog.FileName;
            }
        }

        private void PickRomsPath_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            dialog.RootType = RootType.Path;
            dialog.RootPath = RomsPath.Text.Trim();

            var result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                RomsPath.Text = dialog.SelectedPath;
            }
        }

        private void CreateDatabase_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                // Read in the database xml
                var gameNodes = GetNodesFromDatabase();
                if (gameNodes.Count == 0)
                {
                    MessageBox.Show("No games were found in the XML Database.");
                    return;
                }

                // Read all of the ROM names based on the XML names
                var romNames = GetRomNames();
                if (romNames.Count == 0)
                {
                    MessageBox.Show("No ROMs were found in the ROM folder path.");
                    return;
                }

                var newFileName = CreateNewDatabaseName();
                CreateNewDatabase(newFileName, gameNodes, romNames);

                MessageBox.Show(string.Format("A new database file was created at {0}.", newFileName));
            }
            catch (FileNotFoundException exception)
            {
                MessageBox.Show(exception.Message, "File Not Found");
            }
            catch (DirectoryNotFoundException exception)
            {
                MessageBox.Show(exception.Message, "Directory Not Found");
            }
            catch (Exception exception)
            {
                MessageBox.Show("Something went wrong: " + exception.Message, "Unexpected Error");
            }
        }

        private XElement GetHeaderFromDatabase()
        {
            var path = DatabasePath.Text.Trim();
            if (!File.Exists(path))
                throw new FileNotFoundException("Enter a valid path the the XML database file that came with HyperSpin that contains all the ROMS for a particular emulator.");

            var document = XDocument.Load(path);

            return document.Descendants("header").First();
        }

        private List<XElement> GetNodesFromDatabase()
        {
            var path = DatabasePath.Text.Trim();
            if (!File.Exists(path))
                throw new FileNotFoundException("Enter a valid path to the XML database file that came with HyperSpin that contains all the ROMS for a particular emulator.");

            var document = XDocument.Load(path);

            return document.Descendants("game").ToList();
        }

        private List<string> GetRomNames()
        {
            var path = RomsPath.Text.Trim();
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException("Enter a valid path to the ROMs directory for the database that that you have defined. This does not look in sub-directories for ROMs.");

            var romDirectory = new DirectoryInfo(path);
            return romDirectory.GetFiles().Select(f => f.Name.Substring(0, f.Name.LastIndexOf("."))).ToList();
        }

        private void CreateNewDatabase(string filename, List<XElement> gameNodes, List<string> romNames)
        {
            // Create a new XML file in the same directory as the database XML.
            var document = XDocument.Parse(@"<?xml version=""1.0""?><menu></menu>", LoadOptions.PreserveWhitespace);

            var menuNode = document.Element("menu");

            var headerNode = GetHeaderFromDatabase();
            menuNode.Add(headerNode);
            
            // Only add the XML nodes that have matching ROM files
            foreach(var gameNode in gameNodes)
            {
                var attribute = gameNode.Attribute("name");
                if (attribute == null || !romNames.Contains(attribute.Value.Trim()))
                    continue;

                menuNode.Add(gameNode);
            }

            // Save the file
            document.Save(filename, SaveOptions.None);
        }

        private string CreateNewDatabaseName()
        {
            var databasePath = DatabasePath.Text.Trim();
            var databasePathWithoutExtension = databasePath.Substring(0, databasePath.LastIndexOf("."));
            var newFileName = databasePathWithoutExtension + "_custom.xml";
            return newFileName;
        }

    }
}
