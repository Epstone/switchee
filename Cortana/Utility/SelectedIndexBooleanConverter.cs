using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace CortanaHomeAutomation.MainApp.Utility
{
    public class SelectedIndexBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type typeName, object parameter, string language)
        {
            var val = System.Convert.ToInt32(value);
            
            if (val != -1)
            {
                return true;
            }

            return false;
        }

        public object ConvertBack(object value, Type typeName, object parameter, string language)
        {
            throw new NotImplementedException();
        }

    }
}
