using CounterMoney.Models;
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
    /// Логика взаимодействия для Config.xaml
    /// </summary>
    public partial class ConfigWindow : Window
    {
        /// <summary>
        /// Сервис по работе с файлом-отчетом работы за день.
        /// </summary>
        private DateFileService dateFileService;

        /// <summary>
        /// Текущая конфигурация текущего месяца. (обновляется после инициализации, и после сохранения изменений)
        /// </summary>
        private ConfigMonth currentConfig;

        public ConfigWindow()
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

            LoadConfigToWindow();
        }

        /// <summary>
        /// Загрузка всех конфигураций в окно.
        /// </summary>
        public void LoadConfigToWindow()
        {
            this.currentConfig = this.dateFileService.GetConfigMonth(DateTime.Now);

            this.dateTextBox.Text = currentConfig.Date.Month + "." + currentConfig.Date.Year;
            this.dollarRateUAHTextBox.Text = currentConfig.DollarRateUAH.ToString();
            this.elaborationDaysTextBox.Text = currentConfig.ElaborationDays.ToString();
            this.hoursRateOfDayTextBox.Text = currentConfig.HoursRateOfDay.ToString();
            this.recommendMaxPauseMinTextBox.Text = currentConfig.RecommendMaxPauseMin.ToString();
            this.salaryRateUsdTextBox.Text = currentConfig.SalaryRateUsd.ToString();
            this.takeMinTextBox.Text = currentConfig.TakeMin.ToString();
        }

        /// <summary>
        /// Сохранение конфигурации.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            String dollarRateUAH = this.dollarRateUAHTextBox.Text;
            String elaborationDays = this.elaborationDaysTextBox.Text;
            String hoursRateOfDay = this.hoursRateOfDayTextBox.Text;
            String recommendMaxPauseMin = this.recommendMaxPauseMinTextBox.Text;
            String salaryRateUsd = this.salaryRateUsdTextBox.Text;
            String takeMin = this.takeMinTextBox.Text;

            if (!String.IsNullOrWhiteSpace(dollarRateUAH) || !String.IsNullOrWhiteSpace(elaborationDays) ||
                !String.IsNullOrWhiteSpace(hoursRateOfDay) || !String.IsNullOrWhiteSpace(recommendMaxPauseMin) ||
                !String.IsNullOrWhiteSpace(salaryRateUsd) || !String.IsNullOrWhiteSpace(takeMin))
            {
                currentConfig.DollarRateUAH = Double.Parse(dollarRateUAH);
                currentConfig.ElaborationDays = Int32.Parse(elaborationDays);
                currentConfig.HoursRateOfDay = Int32.Parse(hoursRateOfDay);
                currentConfig.RecommendMaxPauseMin = Int32.Parse(recommendMaxPauseMin);
                currentConfig.SalaryRateUsd = Int32.Parse(salaryRateUsd);
                currentConfig.TakeMin = Int32.Parse(takeMin);

                if (this.dateFileService.SetConfigMonth(this.currentConfig))
                {

                }

                MessageBox.Show("Сохранено. Перезагрузите приложение.", "ОК");
            }
            else
            {
                MessageBox.Show("Найдены пустые поля! Заполните все.", "Ошибка");
            }
        }
    }
}
