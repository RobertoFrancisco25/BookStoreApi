using AutoMapper;
using BookstoreApi.DTOs;
using BookstoreApi.Models;

namespace BookstoreApi.Mapping;

public class MappingProfile :  Profile
{
    public MappingProfile()
    {
        CreateMap<CategoryCreateDTO, Category>();
        CreateMap<Category, CategoryDTO>().ReverseMap();
        CreateMap<Book, BookDTO>();
        CreateMap<BookCreateDTO,Book>();
        CreateMap<Purchase, PurchaseResponseDTO>();
        CreateMap<PurchaseItem, PurchaseItemResponseDTO>();
    }
}