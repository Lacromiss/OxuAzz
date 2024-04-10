using System.ComponentModel.DataAnnotations;

namespace OxuAzz.Models
{
    public class New:BaseModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string ImgUrl { get; set; }
        //Relations
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
