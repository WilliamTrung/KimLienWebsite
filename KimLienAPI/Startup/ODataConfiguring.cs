using ApiService.DTOs;
//using AppCore.Entities;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace KimLienAPI.Startup
{
    public static class ODataConfiguring
    {
        public static IEdmModel GetEdmModel()
        {
            var modelBuilder = new ODataModelBuilder();
            modelBuilder.EntitySet<Product>("products");
            modelBuilder.EntitySet<ProductView>("products-view");
            modelBuilder.EntitySet<Category>("categories");
            modelBuilder.EntitySet<ProductCategory>("product-categories");
            modelBuilder.EntitySet<Role>("roles");
            modelBuilder.EntitySet<User>("users");
            return modelBuilder.GetEdmModel();
        }
    }
}
