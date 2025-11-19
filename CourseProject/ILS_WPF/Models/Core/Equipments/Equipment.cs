namespace ILS_WPF.Models.Core.Equipments
{
    public abstract class Equipment : WarehouseEntry<int>
    {
        public const int MIN_COUNT = 1;
        public int Count { get; set; }

        public override bool IsEmpty() => Count < MIN_COUNT;

        public override void Increase(int amount)
            => Count += amount;

        public override int Decrease(int amount)
        {
            var actualDecrease = Math.Min(amount, Count);
            Count -= actualDecrease;
            return actualDecrease;
        }
    }
}
