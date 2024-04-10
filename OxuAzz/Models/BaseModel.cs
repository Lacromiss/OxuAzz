namespace OxuAzz.Models
{
    public class BaseModel
    {
        public int Id { get; set; }
        public bool isDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
