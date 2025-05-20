using CounterMoney.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace CounterMoney
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
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
        /// Сумма зарплаты за секунду в гривнах. (ставка) (Только при инициализации)
        /// </summary>
        private double sallaryUahSecond;

        /// <summary>
        /// Сумма зарплаты за секунду в гривнах (от старта) (Обновляется каждую сек Если Старт)
        /// </summary>
        private double currentSallaryUah;

        /// <summary>
        /// Сумма зарплаты за секунду в гривнах (за весь день) (Обновляется каждую сек Если Старт)
        /// </summary>
        private double currentSallaryUahOfDay;

        /// <summary>
        /// Состояние счетчиков (запущенные/остановленные)
        /// </summary>
        private bool counterStatus;

        /// <summary>
        /// Включить в отчет время на паузе?
        /// </summary>
        private bool enablePauseTime;

        /// <summary>
        /// Всего секунд после старта. (Обновляется каждую сек Если Старт)
        /// </summary>
        private int secondsAfterStart;

        /// <summary>
        /// Всего секунд за текущий день. (Обновляется каждую сек Если Старт)
        /// </summary>
        private int secondsOfDay;

        /// <summary>
        /// Всего секунд паузы за текущий день. (Обновляется каждую сек Если Стоп и чек-бокс стоит)
        /// </summary>
        private int secondsPauseOfDay;

        /// <summary>
        /// Норма в день. (Только при инициализации)
        /// </summary>
        private DateTime timeInRate;

        /// <summary>
        /// Процент отработанного времени от запланированного (если больше 100 то это уже бонусное время)
        /// </summary>
        private double percentRestPlanWork;

        /// <summary>
        /// Бонусное время за тек. день (Обновляется каждую сек Если Старт и есть бонусы)
        /// </summary>
        private DateTime bonusTime;

        /// <summary>
        /// Бонусная сумма за тек. день (Обновляется каждую сек Если Старт и есть бонусы)
        /// </summary>
        private double bonusMoney;

        /// <summary>
        /// Рабочее время за весь тек. день.
        /// </summary>
        private DateTime workTimeOfDay;

        public MainWindow()
        {
            InitializeComponent();
            var res = this.Init();

            if (!res)
            {
                MessageBox.Show("Файл отчета за текщий месяц не найден! Необходимо сгенерировать.", "Ошибка");
            }
            else
            {
                this.UpdateLabelsValue();
                this.UpdatePauseCounters();

                new Thread(CounterSummOfSecondTimer).Start();
            }
        }

        /// <summary>
        /// Инициализация переменных класса.
        /// </summary>
        /// <returns>false - если файл-отчета за тек. месяц не найден; true - если найден и всё ок</returns>
        public bool Init()
        {
            this.counterService = new CounterService();
            this.dateFileService = new DateFileService();
            this.counterStatus = false;
            this.enablePauseTime = false;
            this.secondsAfterStart = 0;
            this.percentRestPlanWork = 0;
            this.bonusTime = new DateTime();
            this.bonusMoney = 0.00;

            var dateItem = this.dateFileService.GetDateItem(DateTime.Now);
            if(null == dateItem)
            {
                return false;
            }

            this.sallaryUahSecond = this.counterService.GetMoneySec(CounterService.Currency.UAH);
            this.currentSallaryUahOfDay = this.counterService.GetSummOfDay(DateTime.Now, CounterService.Currency.UAH);
            this.timeInRate = this.counterService.GetTimeInRate();
            this.secondsOfDay = dateItem.SecondsWork;
            this.secondsPauseOfDay = dateItem.SecondsPause;
            this.workTimeOfDay = (new DateTime()).AddSeconds(dateItem.SecondsWork);
            // Узнаем бонусное время.
            if(this.workTimeOfDay > this.timeInRate)
            {
                this.bonusTime = new DateTime() + this.workTimeOfDay.Subtract(timeInRate);
                this.bonusMoney = this.bonusTime.TimeOfDay.TotalSeconds * this.sallaryUahSecond;
            }

            return true;
        }

        /// <summary>
        /// Обновление значений каунтеров в UI (обновляется каждую секунду, если был старт, а также при старте приложения)
        /// </summary>
        public void UpdateLabelsValue()
        {
            DateTime timeAfterStart = ConverterTimeService.ConvertSecondsToTime(this.secondsAfterStart);
            DateTime timeOfDay = ConverterTimeService.ConvertSecondsToTime(this.secondsOfDay);

            this.percentRestPlanWork = ConverterTimeService.GetRestRateTimeInPercent(timeOfDay, timeInRate);

            // Обновляем лейбл с суммой от старта.
            this.currentMoney.Content = "+" + this.currentSallaryUah.ToString("F" + 2) + " ₴";
            // Обновляем лейбл с суммой за весь день.
            this.currentMoneyOfDay.Content = "За день: " + this.currentSallaryUahOfDay.ToString("F" + 2) + " ₴";
            // Обновляем лейбл с проработанным временем (после старта)
            this.currentTime.Content = "+" + timeAfterStart.Hour + "ч."+timeAfterStart.Minute + "м.";
            // Обновляем лейбл с нормой на тек. день (для прогресс-бара)
            this.progressTimeWorkLabel.Content = timeOfDay.Hour + "ч." + timeOfDay.Minute + "м./" + timeInRate.Hour + "ч." + timeInRate.Minute + "м." + " (" + percentRestPlanWork.ToString("F" + 2) + "%)";
            // Задаем в прогресс-бар процент отработанного времени от запланированного.
            this.progressTimeWork.Value = this.percentRestPlanWork;
            // Отображаем/обновляем лейблы о бонусном времени.
            if(this.percentRestPlanWork > 100)
            {
                this.blockBonus.Visibility = Visibility.Visible;
                this.bonusMoneyLabel.Content = "+" + this.bonusMoney.ToString("F" + 2) + " ₴";
                this.bonusTimeLabel.Content = "(" + this.bonusTime.Hour + "ч." + this.bonusTime.Minute + "м.)";
            }
            else
            {
                this.blockBonus.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Обновление каунтеров для вывода (обновляется каждую секунду, если был старт)
        /// </summary>
        public void UpdateCounters()
        {
            // Прибавляем в файл +1 секунду.
            this.counterService.AddSecondWork(1);

            // Увеличиваем переменную с суммой от старта.
            this.currentSallaryUah += this.sallaryUahSecond;
            // Увеличиваем переменную с суммой за весь день.
            this.currentSallaryUahOfDay += this.sallaryUahSecond;
            // Увеличиваем переменную с числом секунд работы после старта в программе.
            this.secondsAfterStart++;
            // Увеличиваем переменную с числом секунд за всё время.
            this.secondsOfDay++;
            // Увеличиваем бонусное время на 1 секунду (если оно есть)
            if(this.secondsOfDay > this.timeInRate.TimeOfDay.TotalSeconds)
            {
                this.bonusTime = this.bonusTime.AddSeconds(1);
                this.bonusMoney += this.sallaryUahSecond;
            }
            

            this.UpdateLabelsValue();
        }

        /// <summary>
        /// Обновление каунтеров для паузы.
        /// </summary>
        public void UpdatePauseCounters()
        {
            // Прибавляем в файл +1 секунду.
            this.counterService.AddSecondPause(1);

            // Увеличиваем переменную с числом секунд паузы за тек. день.
            this.secondsPauseOfDay++;

            DateTime timeOfDay = ConverterTimeService.ConvertSecondsToTime(this.secondsPauseOfDay);
            this.pauseTimeOfDay.Content = "["+timeOfDay.Hour+"ч."+timeOfDay.Minute+"м.]";
        }

        /// <summary>
        /// Увеличение каунтеров (таймер)
        /// </summary>
        /// <param name="sender"></param>
        public void CounterSummOfSecondTimer(object sender)
        {
            while (true)
            {
                Thread.Sleep(1000);
                try
                {
                    this.Dispatcher.Invoke(() => {
                        if (this.counterStatus) // Учитываем рабочее время.
                        {
                            this.UpdateCounters();
                        }
                        else  
                        {
                            this.PauseReminder();
                            // Учитываем время на паузе (если включено)
                            if (this.enablePauseTime)
                            {
                                this.UpdatePauseCounters();
                            }
                        }
                    });
                }
                catch (System.Threading.Tasks.TaskCanceledException ex)
                {
                    Environment.Exit(0);
                }
            }
        }

        /// <summary> 
        /// Анимация фона приложения в качестве напоминания о паузе.
        /// </summary>
        public void PauseReminder()
        {
            if(this.Background == Brushes.Red)
            {
                this.Background = Brushes.White;
            }
            else
            {
                this.Background = Brushes.Red;
            }
        }

        /// <summary>
        /// Начать считать счетчики.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartCounters(object sender, RoutedEventArgs e)
        {
            this.Background = Brushes.White;
            this.counterStatus = true;
            this.counterToggleBtn.Content = "Стоп";
        }

        /// <summary>
        /// Остановить подсчет счетчиков.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopCounters(object sender, RoutedEventArgs e)
        {
            var timeWorkedAfterStart = ConverterTimeService.ConvertSecondsToTime(this.secondsAfterStart);

            this.counterStatus = false;
            this.currentSallaryUah = 0;
            this.secondsAfterStart = 0;
            this.counterToggleBtn.Content = "Старт";
            
            MessageBoxResult result = MessageBox.Show("Вы проработали " + timeWorkedAfterStart.Hour + " часов и " + timeWorkedAfterStart.Minute + " минут - запишите это в задачу! Учитывать перерыв?", "Вопрос", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    this.enablePauseTime = true;
                    break;
                case MessageBoxResult.No:
                    this.enablePauseTime = false;
                    break;
            }
        }
        
        /// <summary>
        /// Открытие окна для генерирования дней.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenGeneratorDaysWindow(object sender, RoutedEventArgs e)
        {
            GeneratorDaysWindow window = new GeneratorDaysWindow();
            window.Show();
        }

        /// <summary>
        /// Открытие окна с конфигурацией.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenConfigWindow(object sender, RoutedEventArgs e)
        {
            ConfigWindow window = new ConfigWindow();
            window.Show();
        }

        /// <summary>
        /// Открытие окна с планировкой.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenPlanningWindow(object sender, RoutedEventArgs e)
        {
            PlanningWindow window = new PlanningWindow();
            window.Show();
        }

        /// <summary>
        /// Перезагрузка приложения.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RestartApplication(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Отрыктие окна с верификацией дней.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenVerifyWindow(object sender, RoutedEventArgs e)
        {
            VerifyWindow window = new VerifyWindow();
            window.Show();
        }
    }
}
