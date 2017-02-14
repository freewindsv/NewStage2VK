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
    /// Валидатор положительного числа
    /// </summary>
    public class PositiveIntAllowEmptyValidator : ValidationRule
    {
        /// <summary>
        /// Выполнение проверки
        /// </summary>
        /// <param name="value">Проверяемое значение целевого объекта привязки</param>
        /// <param name="ci">Язык и региональные параметры, используемые в правиле</param>
        /// <returns>Результат проверки</returns>
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo ci)
        {
            string str = value as string;
            if (string.IsNullOrEmpty(str))
            {
                return new ValidationResult(true, null);
            }

            int i;
            if (int.TryParse(str, out i))
            {
                if (i > 0)
                {
                    return new ValidationResult(true, null);
                }
                return new ValidationResult(false, "Число должно быть больше нуля");
            }
            return new ValidationResult(false, "Необходимо указать число или оставить пустым");
        }
    }
}
