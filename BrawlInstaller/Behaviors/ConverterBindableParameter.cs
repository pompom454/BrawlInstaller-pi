// even tho avalonia DOES support this natively, i chose to port this anyway as i dont feel like changing codebase components without knowing the repercussions
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using System;
using System.Globalization;

namespace BrawlInstaller.Behaviors
{
    public class ConverterBindableParameter : MarkupExtension
    {
        public IBinding Binding { get; set; }

        public BindingMode Mode { get; set; }

        public IValueConverter Converter { get; set; }

        public IBinding ConverterParameter { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var multi = new MultiBinding
            {
                Mode = Mode
            };

            multi.Bindings.Add(Binding);

            if (ConverterParameter != null)
                multi.Bindings.Add(ConverterParameter);

            multi.Converter = new Adapter
            {
                Converter = Converter
            };

            return multi;
        }

        private class Adapter : IMultiValueConverter
        {
            public IValueConverter Converter { get; set; }

            private object lastParameter;

            public object Convert(
                IList<object> values,
                Type targetType,
                object parameter,
                CultureInfo culture)
            {
                if (Converter == null)
                    return values[0];

                if (values.Count > 1)
                    lastParameter = values[1];

                return Converter.Convert(
                    values[0],
                    targetType,
                    lastParameter,
                    culture);
            }

            public object[] ConvertBack(
                object value,
                Type[] targetTypes,
                object parameter,
                CultureInfo culture)
            {
                return new[]
                {
                    Converter.ConvertBack(
                        value,
                        targetTypes[0],
                        lastParameter,
                        culture)
                };
            }
        }
    }
}
