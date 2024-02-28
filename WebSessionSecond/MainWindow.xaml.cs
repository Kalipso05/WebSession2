using DataCenter.Model;
using System;
using System.Collections.Generic;
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
using System.Windows.Threading;

namespace WebSessionSecond
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TrackerService _trackerService;
        private double originalImageWith = 7460;
        private double originalImageHeight = 2580;

        private readonly DispatcherTimer _refreshTimer;


        public MainWindow()
        {
            InitializeComponent();
            _trackerService = new TrackerService();
            _refreshTimer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(3)
            };
            _refreshTimer.Tick += RefreshTimer_Tick;
            _refreshTimer.Start();
        }

        private async void RefreshTimer_Tick(object sender, EventArgs e)
        {
            var trackingData = await _trackerService.GetTrackingDataAsync();
            DisplayPeopleOnCanvas(trackingData);
        }

        private async void buttonUpdateLocation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var jsonResponse = await _trackerService.GetTrackingDataAsync();
                DisplayPeopleOnCanvas(jsonResponse);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DisplayPeopleOnCanvas(List<SecurityAccessLog> securityAccessLogs)
        {
            TrackingCanvas.Children.Clear();

            foreach(var person in  securityAccessLogs)
            {
                int scudNumber = person.LastSecurityPointNumber;
                Point location = GetCanvasCoordinatesFromCanvas(scudNumber);

                Ellipse personMarker = new Ellipse()
                {
                    Width = 10,
                    Height = 10,
                    Fill = person.PersonRole == "Клиент" ? Brushes.Green : Brushes.Blue
                };

                Canvas.SetLeft(personMarker, location.X);
                Canvas.SetTop(personMarker, location.Y);

                TrackingCanvas.Children.Add(personMarker);
                string direction = person.LastSecurityPointDirection == "in" ? "Зашел" : "Вышел";
                TextBlock infoSticker = new TextBlock()
                {
                    Text = $"{person.PersonCode}\n {person.PersonRole}\n {person.LastSecurityPointTime}\n {direction}",
                    Foreground = Brushes.Black,
                    Background = Brushes.White,
                    Padding = new Thickness(2)
                };

                Canvas.SetLeft(infoSticker, location.X + 15);
                Canvas.SetTop(infoSticker, location.Y - 10);
                TrackingCanvas.Children.Add(infoSticker);
            }
        }

        private Dictionary<int, Point> scudNumberToCanvasCoordinates = new Dictionary<int, Point>()
        {
            {21, new Point(2139, 957) },
            {22, new Point(2747, 916) },
        };

        private Point GetCanvasCoordinatesFromCanvas(int scudNumber)
        {
            if (scudNumberToCanvasCoordinates.TryGetValue(scudNumber, out Point imageCoord))
            {
                double scaleX = TrackingCanvas.ActualWidth / originalImageWith;
                double scaleY = TrackingCanvas.ActualHeight / originalImageHeight;

                double canvasX = imageCoord.X * scaleX;
                double canvasY = imageCoord.Y * scaleY;

                return new Point(canvasX, canvasY);
            }
            else
            {
                return new Point(0, 0);
            }
        }
    }
}
