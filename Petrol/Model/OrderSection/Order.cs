using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using gsst.Model.FuelStuff;

namespace gsst.Model
{
    public class Order : IEnumerable<CartItem>
    {
        public int Id { get; set; }
        public List<CartItem> Items { get; set; } = new ();

        public int? BonusCardId { get; set; }
        public double BonusSpent { get; set; }
        public double AccruedBonuses { get; set; }
        public int GasStationId { get; set; }
        public int? PumpId { get; set; }
        public Pump? Pump { get; set; }
        public string? PaymentCardNumber { get; set; }
        public string PaymentStatus { get; set; } = "Pending";

        public DateTime Date { get; set; } = DateTime.Now;

        public int UserId { get; set; }
        public User.User User { get; set; } = null!;


        public double Total
        {
            get
            {
                double total = 0;
                foreach (var item in Items)
                {
                    total += item.Subtotal;
                }
                return Math.Round(total - BonusSpent, 2);
            }
        }

        public Order() { }

        public IEnumerator<CartItem> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
