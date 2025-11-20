using ILS_WPF.Models.Core.Enums;

namespace ILS_WPF.Models.Core
{
    public class Staff
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public Rank Rank { get; set; }
        public Speciality Speciality { get; set; }
        public int? UnitId { get; set; }
        public virtual Unit? Unit { get; set; }
    }
}
