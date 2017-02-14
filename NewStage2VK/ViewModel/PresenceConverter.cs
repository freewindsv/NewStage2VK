using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

using NewStage2VK.DataAccess.DataModel;

namespace NewStage2VK.ViewModel
{
    public class PresenceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string txt = string.Empty;
            Profile profile = value as Profile;
            if (profile.IsFriend == true)
            {
                txt += "В друзьях";
            }
            if (profile.IsMember == true)
            {
                if (string.IsNullOrEmpty(txt))
                {
                    txt += "В группе";
                }
                else
                {
                    txt += ", в группе";
                }
            }
            return txt;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
