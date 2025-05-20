using CounterMoney.Models;
using CounterMoney.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;

namespace CounterMoney
{
    /// <summary>
    /// Сервис по работе с файлом-отчетом работы за месяц.
    /// </summary>
    class DateFileService
    {
        /// <summary>
        /// Путь к папке с файлами-отчетами за месяц.
        /// </summary>
        public const string PATH_TO_FILES_DATE = "../../DateResults/";

        /// <summary>
        /// Генерация файла-отчета за месяц.
        /// </summary>
        /// <param name="month">Номер месяца: 1, 2, 3, ..., 12</param>
        /// <param name="year">Номер года: 19, 20, 21...</param>
        /// <param name="hasOffDays">Генерировать выходные дни?</param>
        /// <returns>true - файл сгенерировался; false - ошибка генерации</returns>
        public bool GenerateFile(SelectedDatesCollection dates)
        {
            if(dates.Count > 0)
            {
                int month = dates[0].Month;
                int year = dates[0].Year;

                if (!this.FileExists(month, year))
                {
                    //создание главного объекта документа
                    XmlDocument xmlDoc = new XmlDocument();

                    //создание объявления (декларации) документа
                    XmlDeclaration xmlDec = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                    //добавляем в документ
                    xmlDoc.AppendChild(xmlDec);

                    /*<Days></Days>*/
                    //создание корневого элемента 
                    XmlElement elementDays = xmlDoc.CreateElement("Days");
                    xmlDoc.AppendChild(elementDays);

                    // Добавляем конфигурационный блок.
                    XmlElement elementConfig = xmlDoc.CreateElement("Configs");

                    XmlElement elementDate = xmlDoc.CreateElement("Date");
                    elementDate.SetAttribute("info", "Дата");
                    elementDate.InnerText = month + "." + year;
                    elementConfig.AppendChild(elementDate);

                    XmlElement elementDollarRateUAH = xmlDoc.CreateElement("DollarRateUAH");
                    elementDollarRateUAH.SetAttribute("info", "Курс доллара к гривне");
                    elementDollarRateUAH.InnerText = "24,15";
                    elementConfig.AppendChild(elementDollarRateUAH);

                    XmlElement elementRecommendMaxPauseMin = xmlDoc.CreateElement("RecommendMaxPauseMin");
                    elementRecommendMaxPauseMin.SetAttribute("info", "Рекомендованная пауза за весь день (максимальная) в минутах");
                    elementRecommendMaxPauseMin.InnerText = "60";
                    elementConfig.AppendChild(elementRecommendMaxPauseMin);

                    XmlElement elementElaborationDays = xmlDoc.CreateElement("ElaborationDays");
                    elementElaborationDays.SetAttribute("info", "Запланированное число дополнительных дней (в месяц)");
                    elementElaborationDays.InnerText = "5";
                    elementConfig.AppendChild(elementElaborationDays);

                    XmlElement elementTakeMin = xmlDoc.CreateElement("TakeMin");
                    elementTakeMin.SetAttribute("info", "Отнять минут (на случай учитывания демо) (за текущий месяц)");
                    elementTakeMin.InnerText = "0";
                    elementConfig.AppendChild(elementTakeMin);

                    XmlElement elementSalaryRateUsd = xmlDoc.CreateElement("SalaryRateUsd");
                    elementSalaryRateUsd.SetAttribute("info", "Заработная ставка в долларах");
                    elementSalaryRateUsd.InnerText = "400";
                    elementConfig.AppendChild(elementSalaryRateUsd);

                    XmlElement elementHoursRateOfDay = xmlDoc.CreateElement("HoursRateOfDay");
                    elementHoursRateOfDay.SetAttribute("info", "Норма часов в день");
                    elementHoursRateOfDay.InnerText = "7";
                    elementConfig.AppendChild(elementHoursRateOfDay);

                    elementDays.AppendChild(elementConfig);

                    // Сортируем список дат по возрастанию дней.
                    var datesFormattes = dates.OrderBy(o => o.Day).ToList();

                    foreach (var date in datesFormattes)
                    {
                        /*<Day_1></Day_1>*/
                        //создание дочернего элемента уровня вложенности Days/Day_1
                        XmlElement elementDay = xmlDoc.CreateElement("Day_" + date.Day);

                        /*<SecondsWork></SecondsWork>*/
                        //создание дочернего элемента уровня вложенности Days/Day_1/SecondsWork
                        XmlElement elementSecondsWork = xmlDoc.CreateElement("SecondsWork");
                        elementSecondsWork.InnerText = "0";
                        elementSecondsWork.SetAttribute("verify", "False");
                        //добавляем в elementDay
                        elementDay.AppendChild(elementSecondsWork);

                        /*<SecondsPause></SecondsPause>*/
                        //создание дочернего элемента уровня вложенности Days/Day_1/SecondsPause
                        XmlElement elementSecondsPause = xmlDoc.CreateElement("SecondsPause");
                        elementSecondsPause.InnerText = "0";
                        //добавляем в elementDay
                        elementDay.AppendChild(elementSecondsPause);

                        /*<Type></Type>*/
                        //создание дочернего элемента уровня вложенности Days/Day_1/Type
                        XmlElement elementType = xmlDoc.CreateElement("Type");
                        elementType.InnerText = (date.DayOfWeek == DayOfWeek.Sunday ||
                            date.DayOfWeek == DayOfWeek.Saturday) ? "1" : "0";
                        //добавляем в elementDay
                        elementDay.AppendChild(elementType);

                        //добавляем в elementDays
                        elementDays.AppendChild(elementDay);

                        xmlDoc.Save(PATH_TO_FILES_DATE + month + "_" + year + ".xml");
                    }

                    return true;
                }
            }
            
            return false;
        }

        /// <summary>
        /// Проверка на наличие файла-отчета.
        /// </summary>
        /// <param name="month">Номер месяца: 1, 2, 3, ..., 12</param>
        /// <param name="year">Номер года: 19, 20, 21...</param>
        /// <returns>true - есть файл; false - нету</returns>
        public bool FileExists(int month, int year)
        {
            return System.IO.File.Exists(PATH_TO_FILES_DATE + month + "_" + year + ".xml");
        }

        /// <summary>
        /// Получение рабочего времени, и времени паузы за конкретный день.
        /// </summary>
        /// <param name="date">Дата</param>
        /// <returns>Элемент данных за текущий день.</returns>
        public DateItem GetDateItem(DateTime date)
        {
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(PATH_TO_FILES_DATE + date.Month + "_" + date.Year + ".xml");

                XmlElement xRoot = xDoc.DocumentElement;

                var elements = xRoot.GetElementsByTagName("Day_" + date.Day);

                int secWork = 0;
                int secPause = 0;
                bool verifyWorkTime = false;
                DateItem.TypeDay typeDay = DateItem.TypeDay.WeekDay;

                for (int i = 0; i < elements.Count; i++)
                {
                    var childElements = elements[i].ChildNodes;
                    for (int j = 0; j < childElements.Count; j++)
                    {
                        if (childElements[j].Name == "SecondsWork")
                        {
                            secWork = Int32.Parse(childElements[j].InnerText);
                            verifyWorkTime = Boolean.Parse(childElements[j].Attributes[0].Value); // verify
                        }
                        if (childElements[j].Name == "SecondsPause")
                        {
                            secPause = Int32.Parse(childElements[j].InnerText);
                        }
                        if (childElements[j].Name == "Type")
                        {
                            typeDay = (DateItem.TypeDay)Int32.Parse(childElements[j].InnerText);
                        }
                    }
                }

                DateItem dateItem = new DateItem();
                dateItem.Verify = verifyWorkTime;
                dateItem.SecondsWork = secWork;
                dateItem.SecondsPause = secPause;
                dateItem.Type = typeDay;

                return dateItem;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Получить список всех дней в файле.
        /// </summary>
        /// <param name="date">Месяц и год</param>
        /// <returns></returns>
        public List<DateItemFull> GetDateItems(DateTime date)
        {
            List<DateItemFull> items = new List<DateItemFull>();
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(PATH_TO_FILES_DATE + date.Month + "_" + date.Year + ".xml");

                XmlElement xRoot = xDoc.DocumentElement;

                var elements = xRoot.ChildNodes;

                
                for (int i = 1; i < elements.Count; i++) // Начинаем с 1 так как первым блоком идут конфиги.
                {
                    var childElements = elements[i].ChildNodes;

                    int secWork = 0;
                    int secPause = 0;
                    bool verifyWorkTime = false;
                    DateItem.TypeDay typeDay = DateItem.TypeDay.WeekDay;
                    DateTime dateItemDate = new DateTime(date.Year, date.Month, Int32.Parse(elements[i].Name.Substring(4)));

                    for (int j = 0; j < childElements.Count; j++)
                    {
                        if (childElements[j].Name == "SecondsWork")
                        {
                            secWork = Int32.Parse(childElements[j].InnerText);
                            verifyWorkTime = Boolean.Parse(childElements[j].Attributes[0].Value); // verify
                        }
                        if (childElements[j].Name == "SecondsPause")
                        {
                            secPause = Int32.Parse(childElements[j].InnerText);
                        }
                        if (childElements[j].Name == "Type")
                        {
                            typeDay = (DateItem.TypeDay)Int32.Parse(childElements[j].InnerText);
                        }
                    }

                    var formattedTimeWork = ConverterTimeService.ConvertSecondsToTime(secWork);
                    DateItemFull dateItem = new DateItemFull();
                    dateItem.Verify = verifyWorkTime;
                    dateItem.SecondsWork = secWork;
                    dateItem.SecondsPause = secPause;
                    dateItem.Date = dateItemDate;
                    dateItem.Type = typeDay;
                    dateItem.TimeWorkText = formattedTimeWork.Hour + "ч. " + formattedTimeWork.Minute + "м.";

                    items.Add(dateItem);
                }
                
                return items;
            }
            catch (Exception ex)
            {
                return items;
            }
        }

        /// <summary>
        /// Получение конфигурации в файле-отчете за месяц.
        /// </summary>
        /// <param name="date">Дата</param>
        /// <returns>Конфиг</returns>
        public ConfigMonth GetConfigMonth(DateTime date)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(PATH_TO_FILES_DATE + date.Month + "_" + date.Year + ".xml");

            XmlElement xRoot = xDoc.DocumentElement;

            var elements = xRoot.GetElementsByTagName("Configs");

            ConfigMonth configMonth = new ConfigMonth();
            if (elements.Count == 1)
            {
                var childElements = elements[0].ChildNodes;
                for (int j = 0; j < childElements.Count; j++)
                {
                    switch (childElements[j].Name)
                    {
                        case "Date":
                            configMonth.Date = DateTime.ParseExact(childElements[j].InnerText, "MM.yyyy", null);
                            break;
                        case "DollarRateUAH":
                            configMonth.DollarRateUAH = Double.Parse(childElements[j].InnerText);
                            break;
                        case "ElaborationDays":
                            configMonth.ElaborationDays = Int32.Parse(childElements[j].InnerText);
                            break;
                        case "HoursRateOfDay":
                            configMonth.HoursRateOfDay = Int32.Parse(childElements[j].InnerText);
                            break;
                        case "RecommendMaxPauseMin":
                            configMonth.RecommendMaxPauseMin = Int32.Parse(childElements[j].InnerText);
                            break;
                        case "SalaryRateUsd":
                            configMonth.SalaryRateUsd = Int32.Parse(childElements[j].InnerText);
                            break;
                        case "TakeMin":
                            configMonth.TakeMin = Int32.Parse(childElements[j].InnerText);
                            break;
                        default:
                            break;
                    }
                }
            }

            return configMonth;
        }

        /// <summary>
        /// Заменить значение данных определенного дня.
        /// </summary>
        /// <param name="dateItem">Элемент дня</param>
        /// <param name="date">Дата этого дня</param>
        /// <returns>Успешность сохранения</returns>
        public bool SetDateItem(DateItem dateItem, DateTime date)
        {
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(PATH_TO_FILES_DATE + date.Month + "_" + date.Year + ".xml");

                XmlElement xRoot = xDoc.DocumentElement;
                var elements = xRoot.GetElementsByTagName("Day_" + date.Day);
                elements = elements[0].ChildNodes;

                for (int i = 0; i < elements.Count; i++)
                {
                    if (elements[i].Name == "SecondsWork")
                    {
                        elements[i].InnerText = dateItem.SecondsWork.ToString();
                        elements[i].Attributes[0].Value = dateItem.Verify.ToString(); // verify
                    }
                    if (elements[i].Name == "SecondsPause")
                    {
                        elements[i].InnerText = dateItem.SecondsPause.ToString();
                    }
                    if (elements[i].Name == "Type")
                    {
                        elements[i].InnerText = ((int)dateItem.Type).ToString();
                    }
                }

                xDoc.Save(PATH_TO_FILES_DATE + date.Month + "_" + date.Year + ".xml");

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Заменить значение конфига для файла-отчета за месяц.
        /// </summary>
        /// <param name="configMonth">Новый конфиг</param>
        /// <returns>Успешность сохранения</returns>
        public bool SetConfigMonth(ConfigMonth configMonth)
        {
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(PATH_TO_FILES_DATE + configMonth.Date.Month + "_" + configMonth.Date.Year + ".xml");

                XmlElement xRoot = xDoc.DocumentElement;
                var elements = xRoot.GetElementsByTagName("Configs");

                if (elements.Count == 1)
                {
                    elements = elements[0].ChildNodes;

                    for (int i = 0; i < elements.Count; i++)
                    {
                        switch (elements[i].Name)
                        {
                            case "DollarRateUAH":
                                elements[i].InnerText = configMonth.DollarRateUAH.ToString();
                                break;
                            case "ElaborationDays":
                                elements[i].InnerText = configMonth.ElaborationDays.ToString();
                                break;
                            case "HoursRateOfDay":
                                elements[i].InnerText = configMonth.HoursRateOfDay.ToString();
                                break;
                            case "RecommendMaxPauseMin":
                                elements[i].InnerText = configMonth.RecommendMaxPauseMin.ToString();
                                break;
                            case "SalaryRateUsd":
                                elements[i].InnerText = configMonth.SalaryRateUsd.ToString();
                                break;
                            case "TakeMin":
                                elements[i].InnerText = configMonth.TakeMin.ToString();
                                break;
                            default:
                                break;
                        }
                    }
                }

                xDoc.Save(PATH_TO_FILES_DATE + configMonth.Date.Month + "_" + configMonth.Date.Year + ".xml");

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Получить количество дней в отчете за месяц.
        /// </summary>
        /// <param name="date">Дата за какой месяц и год брать файл-отчет</param>
        /// <returns>Кол-во дней в файл-отчете</returns>
        public int GetCountDays(DateTime date)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(PATH_TO_FILES_DATE + date.Month + "_" + date.Year + ".xml");

            XmlElement xRoot = xDoc.DocumentElement;

            return xRoot.ChildNodes.Count - 1;
        }

        /// <summary>
        /// Получить количество дней в отчете за месяц (по типу)
        /// </summary>
        /// <param name="date">Дата за какой месяц и год брать файл-отчет</param>
        /// <returns>Кол-во дней в файл-отчете</returns>
        public int GetCountDays(DateTime date, DateItem.TypeDay type)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(PATH_TO_FILES_DATE + date.Month + "_" + date.Year + ".xml");

            XmlElement xRoot = xDoc.DocumentElement;

            var elements = xRoot.ChildNodes;

            int count = 0;
            for (int i = 0; i < elements.Count; i++)
            {
                var innerElements = elements[i].ChildNodes;
                for(int j = 0; j < innerElements.Count; j++)
                {
                    if (innerElements[j].Name == "Type")
                    {
                        if ((DateItem.TypeDay)Int32.Parse(innerElements[j].InnerText) == type)
                        {
                            count++;
                        }
                    }
                }
            }
            
            return count;
        }
    }
}
