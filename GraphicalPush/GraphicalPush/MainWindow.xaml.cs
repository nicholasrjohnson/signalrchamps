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
using System.ComponentModel;
using System.Collections.ObjectModel;
using DevExpress.Xpf.Charts;

namespace GraphicalPush
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private ObservableCollection<Point> _values;
        public ObservableCollection<Point> values
        {
            get
            {
                if( _values == null)
                {
                    _values = new ObservableCollection<Point>();
                }
                return _values;
            }
            set
            {
                _values = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(values)));
            }
        }
        /// <summary>
        /// Gets or sets the SMS Message Proxy object used by CG3.
        /// </summary>
        public BroadcastHubProxy BroadcastHubProxy { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            

          //  MessageBox.Show("Loaded");

            // Handles SMS messaging notifications.
            this.BroadcastHubProxy  = new BroadcastHubProxy();
            this.BroadcastHubProxy.ReceiveNewMessage += this.BroadcastHubProxy_ReceiveNewMessage;

        //    MessageBox.Show("Added the handler");
            await this.BroadcastHubProxy.ConnectAsync();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {

        }

        private void BroadcastHubProxy_ReceiveNewMessage(object sender, BroadcastProxyEventArgs e)
        {

           // MessageBox.Show("In the chart render");;
            
            List<Point> dispayedPoints = new List<Point>();

            PushChart.UpdateData();

            //Draws stuff on the chart
            foreach (Tuple<int?, int?> tuple in e.data)
            {
                dispayedPoints.Add(new Point((double)(tuple.Item1 ?? 0), (double)(tuple.Item2 ?? 0)));
            }

            values = new ObservableCollection<Point>(dispayedPoints);

            PushChart.Diagram.Series[0].Points.Clear();

            foreach ( Point p in values)
            {
                Bars.AddPoint(p.X, p.Y);
            }


            Bars.DataContext = this;

            Bars.Visible = true;

    //        MessageBox.Show("After the points");

            PushChart.Visibility = Visibility.Visible;

            values = null;
        }

    }
}
