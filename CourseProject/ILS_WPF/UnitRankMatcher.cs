using ILS_WPF.Models.Core.Enums;

namespace ILS_WPF
{
    static class UnitRankMatcher
    {
        public static Dictionary<UnitType, Rank> MaxRankByUnitType { get; } = new Dictionary<UnitType, Rank>
        {
            { UnitType.Battalion, Rank.LieutenantColonel },
            { UnitType.Regiment, Rank.Colonel },
            { UnitType.Brigade, Rank.MajorGeneral },            
            { UnitType.Division, Rank.LieutenantGeneral },
            { UnitType.Army, Rank.ColonelGeneral }
        };
    }
}
