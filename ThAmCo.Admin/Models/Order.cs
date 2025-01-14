namespace ThAmCo.Admin.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? DispatchDate { get; set; }
        public string Status { get; set; }
        public int UserId { get; set; }
    }
}
