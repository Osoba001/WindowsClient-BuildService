using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace WindowsClientBuildSelfService.Share.ValueConverters
{
    /// <summary>
    /// Converts a string status to a corresponding Brush object for display purposes.
    /// </summary>
    public class StatusToBrushConverter : IValueConverter
    {

        /// <summary>
        /// Converts a string status to a corresponding Brush object for display purposes.
        /// </summary>
        /// <param name="value">The status value to convert.</param>
        /// <param name="targetType">The type of the target property.</param>
        /// <param name="parameter">Optional parameter to pass to the converter.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A Brush object corresponding to the status value.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status)
            {
                if (status == "Approved")
                {
                    return Brushes.Green;
                }
                else if (status == "Pending")
                {
                    return Brushes.Red;
                }
                else
                {
                    return Brushes.Black;
                }
            }
            else
            {
                return Brushes.Black;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
