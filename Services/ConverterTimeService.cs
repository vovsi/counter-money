using CounterMoney.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CounterMoney.Services
{
    /// <summary>
    /// Сервис по преобразованию времени к удобному формату.
    /// </summary>
    class ConverterTimeService
    {
        /// <summary>
        /// Преобразовать секунды к объекту класса DateTime в котором установлены значения часов, минут и секунд.
        /// </summary>
        /// <param name="seconds">Секунды</param>
        /// <returns></returns>
        public static DateTime ConvertSecondsToTime(int seconds)
        {
            var dateTime = new DateTime();

            dateTime = dateTime.AddSeconds(seconds);
            
            return dateTime;
        }

        /// <summary>
        /// Получить отработанное время (в формате нормы часов)
        /// </summary>
        /// <param name="seconds">Секунды</param>
        /// <param name="date">Дата за которорую будет найдена норма раб. времени</param>
        /// <returns></returns>
        public static WorkTime ConvertSecondsToWorkTime(int seconds, DateTime date)
        {
            DateFileService dfService = new DateFileService();
            var config = dfService.GetConfigMonth(date);

            if(null != config)
            {
                int rateHours = config.HoursRateOfDay;

                int tmpAllSeconds = seconds;

                // Отнимаем от секунд дни, которые туда помещаются.
                int tmpDays = tmpAllSeconds / 60 / 60 / rateHours;
                if (tmpDays > 0)
                {
                    tmpAllSeconds -= tmpDays * 60 * 60 * rateHours;
                }
                // Отнимаем от секунд часы, которые туда помещаются (дней уже нет)
                int tmpHours = tmpAllSeconds / 60 / 60;
                if(tmpHours > 0)
                {
                    tmpAllSeconds -= tmpHours * 60 * 60;
                }
                // Отнимаем от секунд минуты, которые туда помещаются (дней и часов уже нет)
                int tmpMinutes = tmpAllSeconds / 60;
                if(tmpMinutes > 0)
                {
                    tmpAllSeconds -= tmpMinutes * 60;
                }
                // Остаточные секунды (дней, часов и минут уже нет)
                int tmpSeconds = tmpAllSeconds;
                
                var workTime = new WorkTime();
                workTime.Days = tmpDays;
                workTime.Hours = tmpHours;
                workTime.Minutes = tmpMinutes;
                workTime.Seconds = tmpSeconds;
                workTime.TotalSeconds = seconds;
                workTime.TotalMinutes = seconds / 60;
                workTime.TotalHours = seconds / 60 / 60;

                return workTime;
            }

            return new WorkTime();
        }

        /// <summary>
        /// Получить процент сколько осталось отработать за день (отработанное время - норма)
        /// </summary>
        /// <param name="worked">Проработанное время</param>
        /// <param name="rate">Норма времени</param>
        /// <returns></returns>
        public static double GetRestRateTimeInPercent(DateTime worked, DateTime rate)
        {
            double secondsWorked = worked.TimeOfDay.TotalSeconds;
            double secondsRate = rate.TimeOfDay.TotalSeconds;

            return (secondsWorked / secondsRate) * 100;
        }
    }
}
