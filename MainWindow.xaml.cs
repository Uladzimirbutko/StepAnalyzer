using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using StepAnalyzer.Entities;
using StepAnalyzer.Json;
using StepAnalyzer.Services;

namespace StepAnalyzer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<string, List<int>> UserDictionary { get; set; }
        private List<User> UsersList { get; set; }
        private readonly JsonDeserialize _jsonDeserialize;
        private readonly UserService _userService;
        private Line HorizontalLine { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            _jsonDeserialize = new JsonDeserialize();
            _userService = new UserService();
            UsersTable();
        }

        //Get Table with Users
        private bool UsersTable()
        {
            var getAllUsersFromFiles = _jsonDeserialize.JsonReader();
            if (getAllUsersFromFiles != null)
            {
                UserDictionary = _jsonDeserialize.UsersDictionary(getAllUsersFromFiles);
                var getAllUsersNames = _userService.GetAllNames(UserDictionary);
                UsersList = new List<User>();
                foreach (var name in getAllUsersNames)
                {
                    UsersList.Add(_userService.GetUserByName(UserDictionary, name));
                }
                UsersGrid.ItemsSource = UsersList;
                return true;
            }
            MessageBox.Show("Файлы не найдены! Добавьте папку с файлами \"TestData\" в путь StepAnalyzer\\bin\\Debug\\net5.0-windows ");
            return false;
        }
        //Get details information from user
        private void bt_Details_Click(object sender, RoutedEventArgs e)
        {
            Coordinates();
            var userName = ((Button)e.OriginalSource).Content.ToString();
            Details(userName);
        }
        //Save user to Directory
        private void bt_Save_Click(object sender, RoutedEventArgs e)
        {
            var saveUser = (User)((Button)e.OriginalSource).DataContext;
            MessageBox.Show(_userService.WriteToJson(UserDictionary, saveUser));
        }
        private void Coordinates()
        {
            HorizontalLine = new Line()
            {
                X1 = 440,
                X2 = 1050,
                Y1 = 600,
                Y2 = 600,
                Stroke = Brushes.Black
            };
            Grid.Children.Add(HorizontalLine);

            int countCoordinateX = 0;
            for (double i = HorizontalLine.X1; i <= HorizontalLine.X2 + 1; i += (HorizontalLine.X2 - HorizontalLine.X1) / 30)
            {
                Line dayLine = new Line()
                {
                    X1 = i,
                    X2 = i,
                    Y1 = HorizontalLine.Y1 - 5,
                    Y2 = HorizontalLine.Y2 + 5,
                    Stroke = Brushes.Black
                };
                Grid.Children.Add(dayLine);

                var textDay = new TextBlock()
                {
                    Margin = new Thickness(dayLine.X1 - 3, dayLine.Y2 + 5, 0, 0),
                    Text = countCoordinateX.ToString(),
                    FontSize = 10
                };
                countCoordinateX++;
                Grid.Children.Add(textDay);
            }

        }
        private void Details(string name)
        {
            var userFromButton = _userService.GetUserByName(UserDictionary, name);
            CoordinateGrid.Children.Clear();
            Line verticalLine = new Line()
            {
                X1 = 440,
                X2 = 440,
                Y1 = 10,
                Y2 = 600,
                Stroke = Brushes.Black
            };
            CoordinateGrid.Children.Add(verticalLine);

            var countCoordinateY = 0.0;
            var coordinateStap = (double)userFromButton.MaxSteps / 10;
            var step = (verticalLine.Y2 - verticalLine.Y1) / 10;
            for (double i = verticalLine.Y2; i >= verticalLine.Y1; i -= step)
            {
                Line stepsLine = new Line()
                {
                    X1 = verticalLine.X1 - 5,
                    X2 = verticalLine.X2 + 5,
                    Y1 = i,
                    Y2 = i,
                    Stroke = Brushes.Black
                };
                CoordinateGrid.Children.Add(stepsLine);

                var stepsValue = new TextBlock()
                {
                    Margin = new Thickness(stepsLine.X1 - 25, stepsLine.Y2 - 7, 0, 0),
                    Text = Math.Round(countCoordinateY).ToString(CultureInfo.InvariantCulture),
                    FontSize = 8
                };
                countCoordinateY += coordinateStap;
                CoordinateGrid.Children.Add(stepsValue);
            }

            var points = new PointCollection();
            var listOfUserSteps = _userService.UserStepsByName(UserDictionary, name);
            var countCoordinateX = (HorizontalLine.X2 - HorizontalLine.X1) / 30;
            var horizontalLine = HorizontalLine.X1;
            var onePercent = (verticalLine.Y2 - verticalLine.Y1) / 100;
            foreach (var steps in listOfUserSteps)
            {
                points.Add(new Point(Math.Round(horizontalLine + countCoordinateX),
                    Math.Round(verticalLine.Y2 - (double)steps * 100 / userFromButton.MaxSteps * onePercent)));
                horizontalLine += countCoordinateX;
            }
            var polyline = new Polyline()
            {
                Points = points,
                Stroke = new LinearGradientBrush()
                {
                    GradientStops = new GradientStopCollection()
                    {
                        new GradientStop()
                        {
                            Color = Color.FromArgb(255,255,0,0),
                            Offset = 0.62
                        },
                        new GradientStop()
                        {
                            Color = Color.FromArgb(255,15,255,0),
                            Offset = 0.421
                        }
                    },
                    StartPoint = new Point(0.5, 0),
                    EndPoint = new Point(0.5, 1),
                    MappingMode = BrushMappingMode.RelativeToBoundingBox
                },
                StrokeThickness = 3,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };
            CoordinateGrid.Children.Add(polyline);
        }
    }
}