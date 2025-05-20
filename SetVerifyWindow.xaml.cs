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
using System.Windows.Shapes;

namespace CounterMoney
{
    /// <summary>
    /// Логика взаимодействия для SetVerifyWindow.xaml
    /// </summary>
    public partial class SetVerifyWindow : Window
    {
        public SetVerifyWindow(int minutes)
        {
            InitializeComponent();

            MinutesTextBox.Text = minutes.ToString();
        }

        public string ResponseText
        {
            get { return MinutesTextBox.Text; }
            set { MinutesTextBox.Text = value; }
        }

        private void OnSetClick(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
