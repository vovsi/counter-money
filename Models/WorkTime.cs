using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CounterMoney.Models
{
    /// <summary>
    /// Модель представляющая рабочее время. (у которого дни зависят от нормы часов) Норма часов - 1 день
    /// </summary>
    class WorkTime
    {
        public int Days { get; set; }

        public int Hours { get; set; }

        public int Minutes { get; set; }

        public int Seconds { get; set; }

        public int TotalHours { get; set; }

        public int TotalMinutes { get; set; }

        public int TotalSeconds { get; set; }
    }
}
