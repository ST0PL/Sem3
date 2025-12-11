using ILS_WPF.Models.Core.Enums;
using ILS_WPF.Models.Core.Equipments;
using ILS_WPF.Models.Core.Resources;
using System.Windows;
using System.Windows.Controls;

namespace ILS_WPF
{
    internal class WarehouseEntryTemplateSelector : DataTemplateSelector
    {
        public string Pattern { get; set; } = "";
        public override DataTemplate? SelectTemplate(object item, DependencyObject container)
        {
            MaterialType? type =
                item is Resource resource ? resource.MaterialType : (item is Equipment equipment ? equipment.MaterialType : null);
            
            if (type is null || type == MaterialType.AnyType)
                return null;

            return (Application.Current.Resources[string.Format(Pattern, type.ToString())] as DataTemplate);
        }
    }
}
