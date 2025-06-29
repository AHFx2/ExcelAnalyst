namespace ExcelAnalyst.Domain.Entities
{
    public class BaseEntity<TKeyType>
    {
        public TKeyType Id { get; set; }
    }
}
