using AutoMapper;
using KL_Repository.UnitOfWork;
using Models.Entities;
using Models.ServiceModels.Categories;
using Models.ServiceModels.Product.Operation;
using Models.ServiceModels.Product.View;
using static System.Net.Mime.MediaTypeNames;

namespace KL_ManagementFeature
{
    public interface IProductManagementFeature
    {
        IEnumerable<ProductAdminViewModel> GetProductAdminView();
        ProductAdminViewModel GetProductAdminById(Guid id);
        Task AddProduct(ProductAddModel model);
        Task AddCategoryToProduct(ProductCategoryAddModel model);
        Task RemoveCategoryFromProduct(ProductCategoryModel model);
        Task ModifyProduct(ProductModifyModel model);
        IEnumerable<CategoryAdminViewModel> GetMainCategories(Guid productId);
        IEnumerable<CategoryAdminViewModel> GetSubCategories(Guid productId, Guid parentId);
        //need funtions to modify product images 
        //add new images (append to last)
        Task AddImage(Guid productId, List<string> images);
        //adjust image position
        Task AdjustImagesPosition(Guid productId, string[] images);
        Task DeleteImage(Guid productId, string imageUrl);
    }
    public class ProductManagementFeature : IProductManagementFeature
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public ProductManagementFeature(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _uow = unitOfWork;
            _mapper = mapper;
        }
        private Product GetProductById(Guid id)
        {
            var product = _uow.ProductRepository.GetFirst(c => c.Id == id, nameof(Product.ProductCategories), $"{nameof(Product.ProductCategories)}.{nameof(ProductCategory.Category)}");
            if (product == null)
            {
                throw new KeyNotFoundException("Product not found for id: " + id);
            }
            return product;
        }
        public async Task AddCategoryToProduct(ProductCategoryAddModel model)
        {
            var product = GetProductById(model.ProductId);
            //add product category
            foreach (var categoryId in model.Categories)
            {
                var category = _uow.CategoryRepository.GetFirst(c => c.Id == categoryId);
                if(category == null)
                {
                    continue;
                } else
                {
                    //check existed category -- if exists -> continue, else add record
                    if (product.ProductCategories.Any(c => c.CategoryId == categoryId))
                    {
                        continue;
                    }
                    else 
                    {
                        //add parent
                        if (category.ParentId != null)
                        {
                            var productParentCategory = new ProductCategory()
                            {
                                CategoryId = (Guid)category.ParentId,
                                ProductId = model.ProductId,
                            };
                            product.ProductCategories.Add(productParentCategory);
                        }
                        //add
                        var productCategory = new ProductCategory()
                        {
                            CategoryId = categoryId,
                            ProductId = model.ProductId,
                        };
                        product.ProductCategories.Add(productCategory);
                    } 
                }
                
            }
            //end add
            _uow.ProductRepository.Update(product);
            await _uow.SaveAsync();
        }

        public async Task AddProduct(ProductAddModel model)
        {
            var product = _mapper.Map<Product>(model);
            _uow.ProductRepository.Add(product);
            await _uow.SaveAsync();
            var productCategories = new ProductCategoryAddModel()
            {
                Categories = model.Categories,
                ProductId = product.Id
            };
            await AddCategoryToProduct(productCategories);
        }

        public async Task ModifyProduct(ProductModifyModel model)
        {            
            var product = GetProductById(model.Id);
            if (model.IsDeleted != null)
            {
                product.IsDeleted = (bool)model.IsDeleted;
            }
            if(model.Name != null)
            {
                product.Name = model.Name;
            }
            _uow.ProductRepository.Update(product);
            await _uow.SaveAsync();
        }

        public async Task RemoveCategoryFromProduct(ProductCategoryModel model)
        {
            var product = GetProductById(model.ProductId);
            var itemToRemove = product.ProductCategories.SingleOrDefault(c => c.CategoryId == model.CategoryId);
            if(itemToRemove != null)
            {
                product.ProductCategories.Remove(itemToRemove);
                _uow.ProductRepository.Update(product);
                await _uow.SaveAsync();
            }
        }

        public IEnumerable<ProductAdminViewModel> GetProductAdminView()
        {
            var products = _uow.ProductRepository.Get(expression: null, orderBy: c => c.OrderBy(e => e.Name), nameof(Product.ProductCategories), $"{nameof(Product.ProductCategories)}.{nameof(ProductCategory.Category)}");
            var result = _mapper.Map<List<ProductAdminViewModel>>(products);
            return result;
        }

        public IEnumerable<CategoryAdminViewModel> GetMainCategories(Guid productId)
        {
            var product = GetProductById(productId);
            var parents = product.ProductCategories.Where(c => c.Category.ParentId == null).ToList();
            var result = _mapper.Map<List<CategoryAdminViewModel>>(parents);
            return result;
        }

        public IEnumerable<CategoryAdminViewModel> GetSubCategories(Guid productId, Guid parentId)
        {
            var product = GetProductById(productId);
            var parents = product.ProductCategories.Where(c => c.Category.ParentId == parentId).ToList();
            var result = _mapper.Map<List<CategoryAdminViewModel>>(parents);
            return result;
        }

        public ProductAdminViewModel GetProductAdminById(Guid id)
        {
            var product = GetProductById(id);
            var result = _mapper.Map<ProductAdminViewModel>(product);
            return result;
        }

        public async Task AddImage(Guid productId, List<string> images)
        {
            var product = GetProductById(productId);
            var current_images = product.Pictures.Split(",");
            var new_images = current_images.Concat(images);
            string pictures = string.Empty;
            new_images.ToList().ForEach(image => { pictures += image + ","; });
            product.Pictures = pictures;
            _uow.ProductRepository.Update(product);
            await _uow.SaveAsync();
        }

        public async Task AdjustImagesPosition(Guid productId, string[] images)
        {
            var product = GetProductById(productId);
            var current_images = product.Pictures.Split(",").ToList();
            if(current_images.Count != images.ToList().Count)
            {
                throw new InvalidDataException("Adjusted images array mismatch!");
            }
            foreach (var image in current_images)
            {
                if (!images.Contains(image))
                {
                    throw new InvalidDataException("Adjusted images array mismatch!");
                }
            }
            string pictures = string.Empty;
            images.ToList().ForEach(image => { pictures += image + ","; });
            product.Pictures = pictures;
            _uow.ProductRepository.Update(product);
            await _uow.SaveAsync();
        }

        public async Task DeleteImage(Guid productId, string imageUrl)
        {
            var product = GetProductById(productId);
            var current_images = product.Pictures.Split(",").ToList();
            current_images.Remove(imageUrl);
            string pictures = string.Empty;
            current_images.ForEach(image => { pictures += image + ","; });
            product.Pictures = pictures;
            _uow.ProductRepository.Update(product);
            await _uow.SaveAsync();
        }
    }
}