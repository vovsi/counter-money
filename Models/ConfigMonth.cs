using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CounterMoney.Models
{
    /// <summary>
    /// Блок конфигурации в каждом файле-отчете за месяц.
    /// </summary>
    class ConfigMonth
    {
        /// <summary>
        /// Дата
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Курс доллара к гривне
        /// </summary>
        public double DollarRateUAH { get; set; }

        /// <summary>
        /// Рекомендованная пауза за весь день (максимальная) в минутах
        /// </summary>
        public int RecommendMaxPauseMin { get; set; }

        /// <summary>
        /// Запланированное число дополнительных дней (в месяц)
        /// </summary>
        public int ElaborationDays { get; set; }

        /// <summary>
        /// Отнять минут (на случай учитывания демо) (за текущий месяц)
        /// </summary>
        public int TakeMin { get; set; }

        /// <summary>
        /// Заработная ставка в долларах
        /// </summary>
        public int SalaryRateUsd { get; set; }

        /// <summary>
        /// Норма часов в день
        /// </summary>
        public int HoursRateOfDay { get; set; }
    }
}
