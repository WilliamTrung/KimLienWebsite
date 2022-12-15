using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AppCore;
using AppCore.Entities;
using AppService.UnitOfWork;
using AppService.Models;
using AppService.Paging;

namespace KimLienCustomerView.Pages.ProductView
{
    public class ProductsModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public string Truncate(string proName, int maxChars, int maxCharShown)
        {
            if (proName.Length >= maxCharShown)
            {
                return proName.Length <= maxChars ? proName : proName.Substring(0, maxChars) + "...";
            }
            return proName;
        }

        public ProductsModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IList<ProductModel> Products { get;set; } = default!;
        [BindProperty]
        public int MaxPages { get; set; } = 10;
        [BindProperty]
        public int PageIndex { get; set; } = 0;
        public async Task OnPostAsync()
        {
            
        }
        public async Task OnGetAsync()
        {
            var page = new PagingRequest()
            {
                PageSize = 10,
                PageIndex = 0
            };
            var result = await _unitOfWork.ProductService.GetProductModels(paging: page);          
            Products = result.ToList();
            Products.Add(new ProductModel()
            {
                Product = new AppService.DTOs.Product()
                {
                    Name = "123",
                    Pictures = "https://images.unsplash.com/photo-1598449356475-b9f71db7d847?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxzZWFyY2h8M3x8aG9yaXpvbnRhbHxlbnwwfHwwfHw%3D&w=1000&q=80",
                    DeserializedPictures = new List<string>()
                    {
                        "https://static-cdn.jtvnw.net/ttv-boxart/513181-272x380.jpg",
                        "https://webstatic.hoyoverse.com/upload/contentweb/2022/06/28/6d512d56ac7ee8181b6fecb8b53c8941_692004913353329817.png",
                        "https://yt3.ggpht.com/UiAwwJ0v-iX3Q4LWn3wfkeITnmjdvEKWyDJW7kIzEdQWdckJCow8MrI-np8EI_9tY0lmk2h-qw=s900-c-k-c0x00ffffff-no-rj"
                    },
                    Id = Guid.NewGuid(),
                    Description = "blah blah blah boy i am god of war you mother fucker"
                }

            }); Products.Add(new ProductModel()
            {
                Product = new AppService.DTOs.Product()
                {
                    Name = "123",
                    Pictures = "https://media.tenor.com/zGW3B-JOrNwAAAAC/klee-genshin.gif",
                    DeserializedPictures = new List<string>()
                    {
                        "https://image.api.playstation.com/vulcan/img/rnd/202104/2507/Xdncb153Sz5UZMaF0X944NP5.png",
                        "https://assets-prd.ignimgs.com/2020/09/29/genshin-impact-button-fin-1601346152039.jpg",
                        "https://image.api.playstation.com/vulcan/ap/rnd/202207/1210/4xJ8XB3bi888QTLZYdl7Oi0s.png"
                    },
                    Id = Guid.NewGuid(),
                    Description = "blah blah blah boy i am god of war you mother fucker"
                }

            }); Products.Add(new ProductModel()
            {
                Product = new AppService.DTOs.Product()
                {
                    Name = "123",
                    Pictures = "https://media.tenor.com/zGW3B-JOrNwAAAAC/klee-genshin.gif",
                    DeserializedPictures = new List<string>()
                    {
                        "https://i.ytimg.com/vi/dIQGI36BxDE/mqdefault.jpg",
                        "https://i.pinimg.com/474x/68/aa/e4/68aae47bc6e440e1a53a088368466077.jpg",
                        "https://api.duniagames.co.id/api/content/upload/file/7819734811580202820.JPG"
                    },
                    Id = Guid.NewGuid(),
                    Description = "blah blah blah boy i am god of war you mother fucker"
                }

            });Products.Add(new ProductModel()
            {
                Product = new AppService.DTOs.Product()
                {
                    Name = "123",
                    Pictures = "https://media.tenor.com/zGW3B-JOrNwAAAAC/klee-genshin.gif",
                    DeserializedPictures = new List<string>()
                    {
                        "https://i.ytimg.com/vi/dIQGI36BxDE/mqdefault.jpg",
                        "https://i.pinimg.com/474x/68/aa/e4/68aae47bc6e440e1a53a088368466077.jpg",
                        "https://api.duniagames.co.id/api/content/upload/file/7819734811580202820.JPG"
                    },
                    Id = Guid.NewGuid(),
                    Description = "blah blah blah boy i am god of war you mother fucker"
                }

            });Products.Add(new ProductModel()
            {
                Product = new AppService.DTOs.Product()
                {
                    Name = "123",
                    Pictures = "https://media.tenor.com/zGW3B-JOrNwAAAAC/klee-genshin.gif",
                    DeserializedPictures = new List<string>()
                    {
                        "https://i.ytimg.com/vi/dIQGI36BxDE/mqdefault.jpg",
                        "https://i.pinimg.com/474x/68/aa/e4/68aae47bc6e440e1a53a088368466077.jpg",
                        "https://api.duniagames.co.id/api/content/upload/file/7819734811580202820.JPG"
                    },
                    Id = Guid.NewGuid(),
                    Description = "blah blah blah boy i am god of war you mother fucker"
                }

            });Products.Add(new ProductModel()
            {
                Product = new AppService.DTOs.Product()
                {
                    Name = "123",
                    Pictures = "https://media.tenor.com/zGW3B-JOrNwAAAAC/klee-genshin.gif",
                    DeserializedPictures = new List<string>()
                    {
                        "https://i.ytimg.com/vi/dIQGI36BxDE/mqdefault.jpg",
                        "https://i.pinimg.com/474x/68/aa/e4/68aae47bc6e440e1a53a088368466077.jpg",
                        "https://api.duniagames.co.id/api/content/upload/file/7819734811580202820.JPG"
                    },
                    Id = Guid.NewGuid(),
                    Description = "blah blah blah boy i am god of war you mother fucker"
                }

            });Products.Add(new ProductModel()
            {
                Product = new AppService.DTOs.Product()
                {
                    Name = "123",
                    Pictures = "https://media.tenor.com/zGW3B-JOrNwAAAAC/klee-genshin.gif",
                    DeserializedPictures = new List<string>()
                    {
                        "https://i.ytimg.com/vi/dIQGI36BxDE/mqdefault.jpg",
                        "https://i.pinimg.com/474x/68/aa/e4/68aae47bc6e440e1a53a088368466077.jpg",
                        "https://api.duniagames.co.id/api/content/upload/file/7819734811580202820.JPG"
                    },
                    Id = Guid.NewGuid(),
                    Description = "blah blah blah boy i am god of war you mother fucker"
                }

            });Products.Add(new ProductModel()
            {
                Product = new AppService.DTOs.Product()
                {
                    Name = "123",
                    Pictures = "https://media.tenor.com/zGW3B-JOrNwAAAAC/klee-genshin.gif",
                    DeserializedPictures = new List<string>()
                    {
                        "https://i.ytimg.com/vi/dIQGI36BxDE/mqdefault.jpg",
                        "https://i.pinimg.com/474x/68/aa/e4/68aae47bc6e440e1a53a088368466077.jpg",
                        "https://api.duniagames.co.id/api/content/upload/file/7819734811580202820.JPG"
                    },
                    Id = Guid.NewGuid(),
                    Description = "blah blah blah boy i am god of war you mother fucker"
                }

            });Products.Add(new ProductModel()
            {
                Product = new AppService.DTOs.Product()
                {
                    Name = "123",
                    Pictures = "https://media.tenor.com/zGW3B-JOrNwAAAAC/klee-genshin.gif",
                    DeserializedPictures = new List<string>()
                    {
                        "https://i.ytimg.com/vi/dIQGI36BxDE/mqdefault.jpg",
                        "https://i.pinimg.com/474x/68/aa/e4/68aae47bc6e440e1a53a088368466077.jpg",
                        "https://api.duniagames.co.id/api/content/upload/file/7819734811580202820.JPG"
                    },
                    Id = Guid.NewGuid(),
                    Description = "blah blah blah boy i am god of war you mother fucker"
                }

            });Products.Add(new ProductModel()
            {
                Product = new AppService.DTOs.Product()
                {
                    Name = "123",
                    Pictures = "https://media.tenor.com/zGW3B-JOrNwAAAAC/klee-genshin.gif",
                    DeserializedPictures = new List<string>()
                    {
                        "https://i.ytimg.com/vi/dIQGI36BxDE/mqdefault.jpg",
                        "https://i.pinimg.com/474x/68/aa/e4/68aae47bc6e440e1a53a088368466077.jpg",
                        "https://api.duniagames.co.id/api/content/upload/file/7819734811580202820.JPG"
                    },
                    Id = Guid.NewGuid(),
                    Description = "blah blah blah boy i am god of war you mother fucker"
                }

            });Products.Add(new ProductModel()
            {
                Product = new AppService.DTOs.Product()
                {
                    Name = "123",
                    Pictures = "https://media.tenor.com/zGW3B-JOrNwAAAAC/klee-genshin.gif",
                    DeserializedPictures = new List<string>()
                    {
                        "https://i.ytimg.com/vi/dIQGI36BxDE/mqdefault.jpg",
                        "https://i.pinimg.com/474x/68/aa/e4/68aae47bc6e440e1a53a088368466077.jpg",
                        "https://api.duniagames.co.id/api/content/upload/file/7819734811580202820.JPG"
                    },
                    Id = Guid.NewGuid(),
                    Description = "blah blah blah boy i am god of war you mother fucker"
                }

            });Products.Add(new ProductModel()
            {
                Product = new AppService.DTOs.Product()
                {
                    Name = "123",
                    Pictures = "https://media.tenor.com/zGW3B-JOrNwAAAAC/klee-genshin.gif",
                    DeserializedPictures = new List<string>()
                    {
                        "https://i.ytimg.com/vi/dIQGI36BxDE/mqdefault.jpg",
                        "https://i.pinimg.com/474x/68/aa/e4/68aae47bc6e440e1a53a088368466077.jpg",
                        "https://api.duniagames.co.id/api/content/upload/file/7819734811580202820.JPG"
                    },
                    Id = Guid.NewGuid(),
                    Description = "blah blah blah boy i am god of war you mother fucker"
                }

            });Products.Add(new ProductModel()
            {
                Product = new AppService.DTOs.Product()
                {
                    Name = "123",
                    Pictures = "https://media.tenor.com/zGW3B-JOrNwAAAAC/klee-genshin.gif",
                    DeserializedPictures = new List<string>()
                    {
                        "https://i.ytimg.com/vi/dIQGI36BxDE/mqdefault.jpg",
                        "https://i.pinimg.com/474x/68/aa/e4/68aae47bc6e440e1a53a088368466077.jpg",
                        "https://api.duniagames.co.id/api/content/upload/file/7819734811580202820.JPG"
                    },
                    Id = Guid.NewGuid(),
                    Description = "blah blah blah boy i am god of war you mother fucker"
                }

            });
            foreach (var product in Products)
            {
                var i = await _unitOfWork.ProductService.Create(product.Product);
            }
        }
        private void AdjustModels()
        {
            if(Products != null)
            {
                foreach (var product in Products)
                {
                    var productName = product.Product.Name;
                    int maxChars = 50;
                    int maxCharShown = 47;
                    product.Product.Name = Truncate(productName, maxChars, maxCharShown);
                }
            }
        }
    }
}
