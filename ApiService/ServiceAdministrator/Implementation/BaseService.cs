using ApiService.Azure;
using ApiService.UnitOfWork;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiService.ServiceAdministrator.Implementation
{
    public class BaseService
    {
        internal readonly IUnitOfWork _unitOfWork = null!;
        internal readonly IMapper _mapper = null!;
        internal readonly IAzureService _azureService = null!;
        public BaseService(IUnitOfWork unitOfWork, IMapper mapper, IAzureService azureService)
        {
            _azureService= azureService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}
