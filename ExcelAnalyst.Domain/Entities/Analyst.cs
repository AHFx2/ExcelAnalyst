namespace ExcelAnalyst.Domain.Entities
{
    public class Analyst : BaseEntity<int>
    {
        public string Department { get; set; }
        public double Consumption { get; set; }
        public DateTime Date { get; set; }
        public int highConsumptionCarsCount { get; set; }
        public int lowConsumptionCarsCount { get; set; }
    }

    public class Car : BaseEntity<int>
    {
        public int DepartmentId { get; set; }
        public string plateNumbers { get; set; }
        public int PlateLetters { get; set; }
    }
}
