﻿using AutoMapper;
using ECommerceAPI.Models;
using ECommerceAPI.DTOs;

namespace ECommerceAPI.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            // Mapping for creating a product
            CreateMap<ProductCreateDTO, Product>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.Category, opt => opt.Ignore()); // Ignore Category since we only have the ID to map

            // Mapping for returning a product
            CreateMap<Product, ProductDTO>();
            CreateMap<Category, CategoryDTO>();
        }
    }
}
