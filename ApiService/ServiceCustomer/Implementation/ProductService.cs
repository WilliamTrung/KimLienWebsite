using ApiService.Azure;
using ApiService.DTOs;
using ApiService.ServiceAdministrator.Implementation;
using ApiService.UnitOfWork;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiService.ServiceCustomer.Implementation
{
    public class ProductService : BaseService, IProductService
    {
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, IAzureService azureService) : base(unitOfWork, mapper, azureService)
        {
        }

        public IQueryable<ProductView> GetProducts()
        {
            using(var productRepos = _unitOfWork.ProductRepository)
            {
                var result = Adapter.Model.ProductViewModelAdapter.FromProductDTOs(_mapper.Map<List<DTOs.Product>>(productRepos.Get()));
                return result.AsQueryable();
            }
        }
    }
}
