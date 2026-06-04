namespace PetrolAPI.DTO
{
    public class TransactionDTO
    {
        public int OrderId { get; set; }
        public DateTime Date { get; set; }
        public string PumpName { get; set; } = "Unknown";
        public string FuelName { get; set; } = "Unknown";
        public double Volume { get; set; }
        public double Amount { get; set; }
        public string ClientPhone { get; set; } = "N/A";
        public string ClientBarcode { get; set; } = "N/A";
        public string PaymentCard { get; set; } = "N/A";
    }
}
