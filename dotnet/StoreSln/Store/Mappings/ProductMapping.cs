using System;
using System.Collections.Generic;
using Store.Models.DTOs;
using Store.Models.Entities;
using AutoMapper;

namespace Store.Mappings
{
    public class ProductMapping : Profile
    {
        public ProductMapping() 
        {
            CreateMap<Product, ProductDTO>();
            CreateMap<ProductDTO, Product>();
        }
    }
}
