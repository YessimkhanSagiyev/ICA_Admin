namespace ThAmCo.Admin.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? DispatchDate { get; set; }
        public string Status { get; set; } = "Pending";
    }
}
