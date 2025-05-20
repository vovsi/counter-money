using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CounterMoney.Models
{
    class DateItem
    {
        /// <summary>
        /// Тип дня.
        /// </summary>
        public enum TypeDay
        {
            [Description("Будний")]
            WeekDay = 0,
            [Description("Выходной")]
            DayOff = 1
        }

        /// <summary>
        /// Секунд рабочего времени.
        /// </summary>
        public int SecondsWork { get; set; }

        /// <summary>
        /// Верификация (подтверждение) отработанного времени от явары.
        /// </summary>
        public bool Verify { get; set; }

        /// <summary>
        /// Секунд находящегося на паузе.
        /// </summary>
        public int SecondsPause { get; set; }

        /// <summary>
        /// Тип дня.
        /// </summary>
        public TypeDay Type { get; set; }
    }
}
