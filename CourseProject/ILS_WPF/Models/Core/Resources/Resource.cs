using ILS_WPF.Models.Core.Enums;
using ILS_WPF.Models.Core.Requests;

namespace ILS_WPF.Models.Core.Resources
{
    public abstract class Resource : WarehouseEntry<float>
    {
        public const float MIN_QUANTITY = 0.1f;
        public MeasureUnit MeasureUnit { get; set; }
        public float Quantity { get; set; }

        public override bool IsEmpty() => Quantity < MIN_QUANTITY;

        public override void Increase(float amount) => Quantity += amount;

        public override float Decrease(float amount)
        {
            var actualDecrease = Math.Min(amount, Quantity);
            Quantity -= actualDecrease;
            return actualDecrease;
        }
    }

}
