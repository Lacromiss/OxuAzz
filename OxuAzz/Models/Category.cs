namespace OxuAzz.Models
{
    public class Category:BaseModel
    {
        public string Name { get; set; }
        public List<New>? News { get; set; }
    }
}
