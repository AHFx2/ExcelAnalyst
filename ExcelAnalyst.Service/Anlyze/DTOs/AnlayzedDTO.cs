
namespace ExcelAnalyst.Service.Anlyze.DTOs
{
    public class AnlayzedDTO
    {
        public double Consumption { get; set; }
        public int ActiveCars { get; set; }
        public int InactiveCars { get; set; }
        public int TotalCars { get; set; }
        public double HighestConsumer { get; set; }
        public double LowestConsumer { get; set; }
        public List<FuelConsumerDTO> FuelConsumers { get; set; } = new List<FuelConsumerDTO>();
    }
}
