using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Controls;
using System.Text.RegularExpressions;

using NewStage2VK.DataAccess.DataModel;
using System.ComponentModel;

namespace NewStage2VK.ViewModel
{
    /// <summary>
    /// Валидатор идентификатора поста события (для записи на спектакль)
    /// </summary>
    public class EvtIdValidator : ValidationRule
    {
        private static Regex re = new Regex(@"^\d+_\d+$");

        /// <summary>
        /// Выполнение проверки
        /// </summary>
        /// <param name="value">Проверяемое значение целевого объекта привязки</param>
        /// <param name="ci">Язык и региональные параметры, используемые в правиле</param>
        /// <returns>Результат проверки</returns>
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo ci)
        {
            if (re.Match(value as string).Success)
            {
                return new ValidationResult(true, null);
            }
            return new ValidationResult(false, "Необходимо указать строку вида \"123456_78\"");
        }
    }
}
