using ILS_WPF.Models.Core.Enums;
using ILS_WPF.Models.Core.Requests;
using System.Windows;
using System.Windows.Controls;

namespace ILS_WPF
{
    internal class RequestDetailsTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate? SelectTemplate(object item, DependencyObject container)
        {
            SupplyRequestDetail? detail = item as SupplyRequestDetail;
            if (detail is null)
                return null;

            return (DataTemplate)Application.Current.Resources[$"{detail.MaterialType.ToString()}DetailTemplate"];
        }
    }
}
