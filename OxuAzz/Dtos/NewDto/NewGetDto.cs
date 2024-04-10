﻿using OxuAzz.Models;

namespace OxuAzz.Dtos.NewDto
{
    public record NewGetDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }
        //Relations
        public int CategoryId { get; set; }
    }
}
