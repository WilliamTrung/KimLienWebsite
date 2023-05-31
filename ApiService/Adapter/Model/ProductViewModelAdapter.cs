using ApiService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiService.Adapter.Model
{
    public static class ProductViewModelAdapter
    {
        public static ProductView FromProductDTO(Product product)
        {
            var pictures = product.Pictures == null? null: Extension.StringExtension.SplitListString(product.Pictures, ",");
            List<string> categories = new List<string>();
            foreach(var item in product.ProductCategories)
            {
                if (!item.IsDeleted)
                {
                    if(item.Category != null)
                    {
                        categories.Add(item.Category.Name);
                    }
                }
            }
            return new ProductView()
            {
                Id= product.Id,
                Categories= categories,
                Description= product.Description,
                ModifiedDate= product.ModifiedDate,
                Name= product.Name,
                Pictures = pictures 
            };
        }
        public static IList<ProductView> FromProductDTOs(IList<Product> products)
        {
            var productViews = new List<ProductView>();
            foreach (var product in products)
            {
                productViews.Add(FromProductDTO(product));
            }
            return productViews;
        }
    }
}
