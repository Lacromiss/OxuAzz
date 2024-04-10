using OxuAzz.Models;
using System.ComponentModel.DataAnnotations;

namespace OxuAzz.Dtos.NewDto
{
    public record NewPostDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }
        //Relations

        public int CategoryId { get; set; }
    }
}
