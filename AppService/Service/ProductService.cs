using AppCore;
using AppCore.Entities;
using AppService.IService;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppService.Service
{
    public class ProductService : BaseService<Product, DTOs.Product>, IProductService
    {
        public ProductService(SqlContext context, IMapper mapper) : base(context, mapper)
        {
        }
        public override void DisableSelfReference(ref Product entity)
        {
            //IDK what to do here yet
        }
    }
}
