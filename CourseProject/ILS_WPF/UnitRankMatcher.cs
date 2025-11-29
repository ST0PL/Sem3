using ILS_WPF.Models.Core.Enums;

namespace ILS_WPF
{
    static class UnitRankMatcher
    {
        public const Rank MaxBattalionRank = Rank.LieutenantColonel;
        public static Dictionary<UnitType, Rank[]> CommanderRanks = new Dictionary<UnitType, Rank[]>
        {
            { UnitType.Battalion, [Rank.Major, Rank.LieutenantColonel]},
            { UnitType.Regiment, [Rank.LieutenantColonel, Rank.Colonel] },
            { UnitType.Brigade, [Rank.Colonel, Rank.MajorGeneral] },            
            { UnitType.Division, [Rank.MajorGeneral, Rank.LieutenantGeneral] },
            { UnitType.Army, [Rank.LieutenantGeneral, Rank.ArmyGeneral] }
        };
    }
}
