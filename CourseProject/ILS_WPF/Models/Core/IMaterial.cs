using ILS_WPF.Models.Core.Enums;

namespace ILS_WPF.Models.Core
{
    public interface IMaterial
    {
        MaterialType MaterialType { get; set; }
        public string Name { get; set; }
    }
}
