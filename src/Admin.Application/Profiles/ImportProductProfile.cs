using Admin.Contract.Commands;
using AutoMapper;
using Common.Domain.Entities;
using Common.Kernel.Constants;

namespace Admin.Application.Profiles
{
    public class ImportProductProfile : Profile
    {
        public ImportProductProfile()
        {
            CreateMap<CreateProductContractCommand, Product>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s => ProductStatus.Active))
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
        }
    }
}
