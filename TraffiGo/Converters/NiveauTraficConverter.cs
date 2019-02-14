using System;
using System.Globalization;
using System.Windows.Data;
using TraffiGo.Modeles.Data;

namespace TraffiGo.Converters
{
    public class NiveauTraficConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Trafic nivTrafic = (Trafic)value;
            switch (nivTrafic)
            {
                case Trafic.Faible:
                    return Trafic.Faible.ToString();
                case Trafic.Moyen:
                    return Trafic.Moyen.ToString();
                case Trafic.Élevé:
                    return Trafic.Élevé.ToString();
            }
            throw new Exception("Niveau de trafic introuvable");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
