using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Controls;

using NewStage2VK.DataAccess.DataModel;
using System.ComponentModel;

namespace NewStage2VK.ViewModel
{
    /// <summary>
    /// Валидатор для проверки заполнения поля
    /// </summary>
    public class NotEmptyValidator : ValidationRule
    {
        /// <summary>
        /// Выполнение проверки
        /// </summary>
        /// <param name="value">Проверяемое значение целевого объекта привязки</param>
        /// <param name="ci">Язык и региональные параметры, используемые в правиле</param>
        /// <returns>Результат проверки</returns>
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo ci)
        {
            if (string.IsNullOrEmpty(value as string))
            {
                return new ValidationResult(false, "Необходимо ввести текст");
            }
            return new ValidationResult(true, null);
        }
    }
}
