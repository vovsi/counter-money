using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CounterMoney.Models
{
    class DateItemFull : DateItem
    {
        /// <summary>
        /// Дата
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Отработанное время (в текстовой форме)
        /// </summary>
        public string TimeWorkText { get; set; }
    }
}
