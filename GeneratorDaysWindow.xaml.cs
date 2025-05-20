using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Логика взаимодействия для GeneratorDays.xaml
    /// </summary>
    public partial class GeneratorDaysWindow : Window
    {
        /// <summary>
        /// Сервис по работе с файлом-отчетом работы за день.
        /// </summary>
        private DateFileService dateFileService;

        public GeneratorDaysWindow()
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
        }

        /// <summary>
        /// Генерация дней месяца в новый файл.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGenerateDays(object sender, RoutedEventArgs e)
        {
            SelectedDatesCollection dates = generateCalendar.SelectedDates;

            if(dates.Count > 0)
            {
                if (this.dateFileService.GenerateFile(dates))
                {
                    // Открываем папку с сгенерированным файлом.
                    Process.Start("explorer", System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" + DateFileService.PATH_TO_FILES_DATE.Replace('/', '\\'));
                }
                else
                {
                    MessageBox.Show("Ошибка генерации файла. Убедитесь что файл не существует.", "Уведомление");
                }
            }
        }
    }
}
