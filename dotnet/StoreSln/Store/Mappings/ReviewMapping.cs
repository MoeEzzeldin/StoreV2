using AutoMapper;
using Store.Models.DTOs;
using Store.Models.Entities;

namespace Store.Mappings
{
    public class ReviewMapping : Profile
    {

        public ReviewMapping()
        {
            CreateMap<Review, ReviewDTO>();
            CreateMap<ReviewDTO, Review>();
        }

    }
}
