using CounterMoney.Models;
using CounterMoney.Services;
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
    /// Логика взаимодействия для VerifyWindow.xaml
    /// </summary>
    public partial class VerifyWindow : Window
    {
        /// <summary>
        /// Сервис по работе с файлом-отчетом работы за день.
        /// </summary>
        private DateFileService dateFileService;

        /// <summary>
        /// Дни месяца.
        /// </summary>
        private List<DateItemFull> dayItems;

        public VerifyWindow()
        {
            InitializeComponent();
            Init();
        }

        /// <summary>
        /// Инициализатор окна.
        /// </summary>
        private void Init()
        {
            dateFileService = new DateFileService();

            LoadDays();
        }

        /// <summary>
        /// Загрузка списка дней в лист UI и обновление суммы проработанного времени за месяц.
        /// </summary>
        private void LoadDays()
        {
            // Загрузка списка дней в лист.
            this.dayItems = this.dateFileService.GetDateItems(DateTime.Now);
            daysListView.ItemsSource = dayItems;

            // Обновляем лейбл суммы проработанного времени за месяц.
            int summSeconds = dayItems.Sum(s => s.SecondsWork);
            WorkTime summTime = ConverterTimeService.ConvertSecondsToWorkTime(summSeconds, DateTime.Now);
            this.allTime.Content = summTime.Days + "д. " + summTime.Hours + "ч. " + summTime.Minutes + "м. (" + summTime.TotalHours + "ч. или " + summTime.TotalMinutes + "м.)";
        }

        /// <summary>
        /// Двойной клик по дню из списка.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DaysListMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)e.OriginalSource).DataContext as DateItemFull;
            if (item != null)
            {
                if(null != item)
                {
                    SetVerifyWindow window = new SetVerifyWindow(item.SecondsWork / 60);
                    if (window.ShowDialog() == true)
                    {
                        string newMinutes = window.ResponseText.ToString();
                        if (!String.IsNullOrWhiteSpace(newMinutes))
                        {
                            item.SecondsWork = Int32.Parse(newMinutes) * 60;
                            item.Verify = true;
                            if(!this.dateFileService.SetDateItem(item, new DateTime(item.Date.Year, item.Date.Month, item.Date.Day)))
                            {
                                MessageBox.Show("Ошибка сохранения нового значенияю.", "Ошибка");
                            }
                            else
                            {
                                // Перезагружаем список дней в таблице.
                                this.LoadDays();
                            }
                        }
                    }
                }
            }
        }
    }
}
