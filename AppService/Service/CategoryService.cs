using AutoMapper;
using AppService.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCore;
using AppCore.Entities;

namespace AppService.Service
{
    public class CategoryService : BaseService<Category, DTOs.Category>, ICategoryService
    {
        private IProductService _productService;
        public CategoryService(SqlContext context, IMapper mapper) : base(context, mapper)
        {
            _productService = new ProductService(context, mapper);
        }
        public override void DisableSelfReference(ref Category entity)
        {
            //IDK what to do here yet. Have to ask
        }
    }
}
