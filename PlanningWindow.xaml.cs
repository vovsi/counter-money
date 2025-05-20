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
    /// Логика взаимодействия для PlanningWindow.xaml
    /// </summary>
    public partial class PlanningWindow : Window
    {
        /// <summary>
        /// Сервис по работе с статистикой/каунтерами зарплаты.
        /// </summary>
        private CounterService counterService;

        /// <summary>
        /// Сервис по работе с файлом-отчетом работы за день.
        /// </summary>
        private DateFileService dateFileService;

        /// <summary>
        /// Дни месяца.
        /// </summary>
        private List<DateItemFull> dayItems;

        public PlanningWindow()
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
            UpdateLabels();
        }
        
        /// <summary>
        /// Загрузка списка дней в лист UI
        /// </summary>
        private void LoadDays()
        {
            // Загрузка списка дней в лист.
            this.dayItems = this.dateFileService.GetDateItems(DateTime.Now);
            //daysListView.ItemsSource = dayItems;
        }

        /// <summary>
        /// Загрузка данных в лейблы UI
        /// </summary>
        private void UpdateLabels()
        {
            List<DateItemFull> weekDays = this.dayItems.Where(d => d.Type == DateItem.TypeDay.WeekDay).ToList();
            var config = this.dateFileService.GetConfigMonth(DateTime.Now);

            // Обновляем лейбл Запланировано доп. дней
            this.daysInPlanLabel.Content = config.ElaborationDays;

            // Обновляем лейбл Норма
            int rateHours = weekDays.Count * config.HoursRateOfDay - (config.TakeMin / 60);
            WorkTime rateTime = ConverterTimeService.ConvertSecondsToWorkTime(rateHours * 60 * 60, DateTime.Now);
            this.rateTimeLabel.Content = rateTime.Days + "д. " + rateTime.Hours + "ч. " + rateTime.Minutes + "м.";
            
            // Обновляем лейбл Отработано
            int summSecondsWorked = this.dayItems.Sum(s => s.SecondsWork);
            WorkTime summTimeWorked = ConverterTimeService.ConvertSecondsToWorkTime(summSecondsWorked, DateTime.Now);
            this.workedTimeLabel.Content = summTimeWorked.Days + "рд. " + summTimeWorked.Hours + "ч. " + summTimeWorked.Minutes + "м.";

            // Обновляем лейбл Нужно отработать
            int needWorkHours = (config.ElaborationDays > 0) ? rateHours + (config.ElaborationDays * config.HoursRateOfDay) : rateHours;
            WorkTime needWorkTime = ConverterTimeService.ConvertSecondsToWorkTime(needWorkHours * 60 * 60, DateTime.Now);
            this.needTimeLabel.Content = needWorkTime.Days + "рд. " + needWorkTime.Hours + "ч. " + needWorkTime.Minutes + "м.";

            // Обновляем лейбл Осталось отработать
            int leftWorkedSeconds = (needWorkHours * 60 * 60) - summSecondsWorked;
            var leftWorkedDatetime = ConverterTimeService.ConvertSecondsToWorkTime(leftWorkedSeconds, DateTime.Now);
            this.leftTimeLabel.Content = leftWorkedDatetime.Days + "рд. " + leftWorkedDatetime.Hours + "ч. " + leftWorkedDatetime.Minutes + "м.";

            // Обновляем лейбл Осталось отработать (уч. норму наперед) ДОДЕЛАТЬ!!!!!!!!!!!!!!!!!!!
            var currDayItem = this.dayItems.Where(d => d.Date.Day == DateTime.Now.Day).First();
            int idxCurrDay = this.dayItems.IndexOf(currDayItem);
            int countItems = this.dayItems.Count;
            var futureDayItems = this.dayItems.GetRange(idxCurrDay, countItems);
            var futureDayWeekDaysItems = futureDayItems.Where(d => d.Type == DateItem.TypeDay.WeekDay).ToList();
            int futureRateHours = futureDayWeekDaysItems.Count * config.HoursRateOfDay;
            int needWorkIncludeFutureRateInSec = (needWorkHours * 60 * 60) - (futureRateHours * 60 * 60) - summSecondsWorked;
            var needWorkIncludeFutureRateInSecDatetime = ConverterTimeService.ConvertSecondsToWorkTime(needWorkIncludeFutureRateInSec, DateTime.Now);
            //this.leftTimeIncludeFutureRateLabel.Content = needWorkIncludeFutureRateInSecDatetime.Days + "рд. " + needWorkIncludeFutureRateInSecDatetime.Hours + "ч. " + needWorkIncludeFutureRateInSecDatetime.Minutes + "м.";
        }
    }
}
