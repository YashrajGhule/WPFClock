using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFClock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

        public MainWindow()
        {
            InitializeComponent();
            this.Top = 0;
            this.Left = 1365 - this.Width;
            Thread thread = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    this.Dispatcher.Invoke(setTime);
                    Thread.Sleep(200);
                }
            }));
            thread.Start();
        }
        private void setTime()
        {
            DateTimeFormatInfo info = new DateTimeFormatInfo();
            DateTime dateTime = DateTime.Now;
            Hours.Text = dateTime.ToString("hh");
            AMPM.Text = dateTime.ToString("tt");
            Minutes.Text = dateTime.ToString("mm");
            Day.Text = dateTime.DayOfWeek.ToString();
            Date.Text = dateTime.Day.ToString()+" "+info.GetAbbreviatedMonthName(dateTime.Month)+", "+dateTime.Year.ToString();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            int style = (int)GetWindowLongPtr(helper.Handle, -20);
            SetWindowLong(helper.Handle, -20,(uint)(128|style));
        }
    }
}
