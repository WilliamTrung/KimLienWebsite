using ApiService.Azure.Container;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiService.Azure
{
    public interface IAzureService
    {
        IProductContainer ProductContainer { get; }
        IPictureContainer PictureContainer { get; }
    }
}
