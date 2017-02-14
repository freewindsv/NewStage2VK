using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

using NewStage2VK.DataAccess.DataModel;
using System.ComponentModel;

namespace NewStage2VK.ViewModel
{
    /// <summary>
    /// Конвертер аватарок из объекта профиля
    /// </summary>
    public class AvatarConverter : IValueConverter
    {
        /// <summary>
        /// Метод, возвращающий байты изображения аватарки или же Url для загрузки изображения
        /// </summary>
        /// <param name="value">Объект конвертации, в нашем случае - объект профиля</param>
        /// <param name="targetType">Тип целевого свойства привязки</param>
        /// <param name="parameter">Используемый параметр преобразователя</param>
        /// <param name="culture">Язык и региональные параметры, используемые в преобразователе</param>
        /// <returns>Преобразованное значение. Если этот метод возвращает null, используется допустимое значение NULL</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Profile profile = value as Profile;
            if (profile.Avatar_50 != null)
            {
                return profile.Avatar_50;
            }
            return profile.AvatarUrl_50;
        }

        /// <summary>
        /// Метод обратного преобразования
        /// </summary>
        /// <param name="value">Значение, произведенное целью привязки</param>
        /// <param name="targetType">Тип целевого свойства привязки</param>
        /// <param name="parameter">Используемый параметр преобразователя</param>
        /// <param name="culture">Язык и региональные параметры, используемые в преобразователе</param>
        /// <returns>Преобразованное значение. Если этот метод возвращает null, используется допустимое значение NULL</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
