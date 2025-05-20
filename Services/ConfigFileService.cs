using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CounterMoney
{
    /// <summary>
    /// Сервис по работе с конфигурационным файлом.
    /// </summary>
    class ConfigFileService
    {
        /// <summary>
        /// Путь к конфиг-файлу.
        /// </summary>
        const string CONFIG_FILE_NAME = "../../Config.xml";

        /// <summary>
        /// Получение значения из конфиг-файла.
        /// </summary>
        /// <param name="nameConfig">Название конфигурации.</param>
        /// <returns>Значение из конфиг файла.</returns>
        public string GetValue(string nameConfig)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(CONFIG_FILE_NAME);

            XmlElement xRoot = xDoc.DocumentElement;
            var elements = xRoot.GetElementsByTagName(nameConfig);

            return (elements.Count > 0) ? elements[0].InnerText : "";
        }
    }
}
