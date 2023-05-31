using ApiService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiService.ServiceCustomer
{
    public interface IProductService
    {
        //-- important note: CUSTOMER FUNCTION MUST RETURN A CUSTOM VIEW MODEL
        IQueryable<ProductView> GetProducts();
        
    }
}
