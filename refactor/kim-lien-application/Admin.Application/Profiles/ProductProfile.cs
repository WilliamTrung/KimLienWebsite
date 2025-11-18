using Admin.Application.Commands.Product;
using Admin.Application.Models.Product;
using Admin.Contract.Commands;
using AutoMapper;
using Common.Domain.Entities;
using Common.Kernel.Constants;

namespace Admin.Application.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile() 
        {
            CreateMap<ModifyProductCommand, Product>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.PictureAssets, opt => opt.MapFrom(s => s.Images))
                ;
            CreateMap<CreateProductCommand, Product>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s => ProductStatus.Inactive))
                .ForMember(d => d.ProductCategories, 
                    opt => opt.MapFrom(s => s.CategoryIds.Select(x => new ProductCategory
                    {
                        CategoryId = x,
                    })))
                .ForMember(d => d.PictureAssets, opt => opt.MapFrom(s => s.Images))
                .AfterMap((src, dest) =>
                {
                    var now = DateTime.UtcNow;
                    dest.CreatedDate = now;

                    if (dest.PictureAssets != null)
                    {
                        foreach (var a in dest.PictureAssets)
                        {
                            // assumes AssetDto has a writable CreatedAt (or CreatedDate) property
                            a.CreatedAt = now;   // change to a.CreatedDate if that¡¯s your property name
                        }
                    }
                })
                ;
            CreateMap<Product, ProductDto>()
                .ForMember(d => d.Categories, opt => opt.MapFrom(s => s.ProductCategories.Select(x => x.Category)))
                .ForMember(d => d.Images, opt => opt.MapFrom(s => s.PictureAssets))
                .ForMember(d => d.ViewCount, opt => opt.MapFrom(s => s.ProductViews.Sum(x => x.ViewCount)))
                .ForMember(d => d.Favorites, opt => opt.MapFrom(s => s.ProductFavors.Count))
                ;
            CreateMap<Category, ProductCategoryDto>()
                .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.CategoryId, opt => opt.MapFrom(s => s.Id))
                ;
            CreateMap<CreateProductContractCommand, CreateProductCommand>();
        }
    }
}
