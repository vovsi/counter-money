using CounterMoney.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CounterMoney
{
    /// <summary>
    /// Сервис для подсчета статистики и других каунтеров.
    /// </summary>
    class CounterService
    {
        /// <summary>
        /// Валюта
        /// </summary>
        public enum Currency
        {
            UAH = 1,
            USD = 2
        }
        
        /// <summary>
        /// Сервис по работе с файлом-отчетом работы за месяц.
        /// </summary>
        private DateFileService dateFileService;

        public CounterService()
        {
            dateFileService = new DateFileService();
        }

        /// <summary>
        /// Сумма зарплаты в секунду.
        /// </summary>
        /// <param name="currency">Валюта к которой преобразовываем сумму</param>
        /// <returns>Сумма за секунду</returns>
        public double GetMoneySec(Currency currency)
        {
            double result = 0.0;
            var config = this.dateFileService.GetConfigMonth(DateTime.Now);

            // Ставка в долл
            double sallaryRate = config.SalaryRateUsd;
            // Кол-во рабочих дней в месяце 
            int countDays = this.dateFileService.GetCountDays(DateTime.Now, DateItem.TypeDay.WeekDay);
            // Норма часов в день
            int hoursOfDay = config.HoursRateOfDay;

            // По-умолчанию считаем сначала в долларах, так как ставка в конфиге в долларах указана.
            double dollOfSec = sallaryRate / countDays / hoursOfDay / 60 / 60;

            switch (currency)
            {
                case Currency.UAH:
                    double dollarRateUah = config.DollarRateUAH;
                    return dollOfSec * dollarRateUah;
                case Currency.USD:
                    return result;
                default:
                    return result;
            }
        }

        /// <summary>
        /// Добавить N количество секунд работы к текущему дню.
        /// </summary>
        /// <param name="second">Секунды</param>
        public void AddSecondWork(int second)
        {
            DateTime date = DateTime.Now;

            DateItem dateItem = this.dateFileService.GetDateItem(date);
            if(null != dateItem)
            {
                dateItem.SecondsWork += second;

                if (!this.dateFileService.SetDateItem(dateItem, date))
                {
                    MessageBox.Show("Ошибка добавления секунды работы к текущему дню.", "Ошибка");
                }
            }
        }

        /// <summary>
        /// Добавить N количество секунд паузы к текущему дню.
        /// </summary>
        /// <param name="second">Секунды</param>
        public void AddSecondPause(int second)
        {
            DateTime date = DateTime.Now;

            DateItem dateItem = this.dateFileService.GetDateItem(date);
            if(null != dateItem)
            {
                dateItem.SecondsPause += second;

                if (!this.dateFileService.SetDateItem(dateItem, date))
                {
                    MessageBox.Show("Ошибка добавления секунды паузы к текущему дню.", "Ошибка");
                }
            }
        }

        /// <summary>
        /// Получить сумму в указанной валюте работы за день.
        /// </summary>
        /// <param name="date">Дата</param>
        /// <param name="currency">Валюта</param>
        /// <returns></returns>
        public double GetSummOfDay(DateTime date, Currency currency)
        {
            var dateItem = this.dateFileService.GetDateItem(date);
            int secWorkOfDay = dateItem.SecondsWork;
            var summ = this.GetMoneySec(currency);

            return summ * secWorkOfDay;
        }

        /// <summary>
        /// Получить норму времени на тек. день.
        /// </summary>
        /// <returns></returns>
        public DateTime GetTimeInRate()
        {
            var dateTime = new DateTime();
            var config = this.dateFileService.GetConfigMonth(DateTime.Now);

            int hours = config.HoursRateOfDay;

            dateTime = dateTime.AddHours(hours);

            return dateTime;
        }
    }
}
