using ILS_WPF.Models.Core.Enums;
using ILS_WPF.Models.Database;

namespace ILS_WPF.Models.Core
{
    public class Staff : IDbEntry
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public Rank Rank { get; set; }
        public Speciality Speciality { get; set; }
        public int? UnitId { get; set; }
        public virtual Unit? Unit { get; set; }

        public Staff() { }

        public Staff(string fullName, Rank rank, Speciality speciality, int? unitId)
        {
            FullName = fullName;
            Rank = rank;
            Speciality = speciality;
            UnitId = unitId;
        }
    }
}
