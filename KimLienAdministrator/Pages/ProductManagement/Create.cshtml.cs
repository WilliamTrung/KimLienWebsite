using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using AppCore;
using AppService.UnitOfWork;
using KimLienAdministrator.Helper.Azure.IBlob;
using AppService.DTOs;
using AppService.Extension;
using NuGet.Packaging.Signing;
using AppService;

namespace KimLienAdministrator.Pages.ProductManagement
{
    //Implement each properties for Product
    //On Create click --> Submit each property and conclude into Product
    //Validate properties --> Create Product
    [Authorized("Administrator")]
    public class CreateModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductBlob _productBlob;
        //Categories value
        public readonly string ACTION_CATEGORY_SELECT = "CategorySelect";
        public readonly string ACTION_CREATE = "Create";
        //Message
        private readonly string MESSAGE_NAME_INVALID = "Product name is too short!";
        private readonly string MESSAGE_DESCRIPTION_INVALID = "Product description is not valid!";
        private readonly string MESSAGE_PICTURE_REQUIRED = "Must include a picture!";
        public List<string> Messages { get; set; } = new List<string>(); 
        //Categories Selection
        [BindProperty(SupportsGet = true)]
        public List<Guid> MainSelectedCategories { get; set; } = null!;
        [BindProperty(SupportsGet = true)]
        public List<Guid> SubSelectedCategories { get; set; } = null!;
        [BindProperty]
        public List<Guid> SelectedCategories { get; set; } = null!;
        public Dictionary<string, SelectList> SubCategories { get; set; } = new Dictionary<string, SelectList>();
        public SelectList MainCategories { get; set; } = null!;

        //Product Properties
        [BindProperty]
        public string Name { get;set; } = null!;
        [BindProperty]
        public string Description { get; set; } = null!;

        public CreateModel(IProductBlob productBlob, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _productBlob = productBlob;
        }

        public async Task<IActionResult> OnGet()
        {
            await Initialize();
            return Page();
        }
        private async Task Initialize()
        {
            MainCategories = await SetMainCategories();
            foreach (var category in MainSelectedCategories)
            {
                await SetSubCategories(category);
            }
        }
        private async Task<SelectList> SetMainCategories()
        {
            var categories = await _unitOfWork.CategoryService.GetDTOs(filter: c => c.ParentId == null);
            return new SelectList(categories, "Id", "Name");
        }
        private async Task SetSubCategories(Guid parentId)
        {
            var subCategories = await _unitOfWork.CategoryService.GetDTOs(filter: c => c.ParentId == parentId);
            if (subCategories != null && subCategories.Count() > 0)
            {
                string name = parentId.ToString();
                var list = new SelectList(subCategories, "Id", "Name");
                SubCategories.Add(subCategories.First().Parent.Name, list);
            }

        }
        [BindProperty]
        public Product Product { get; set; } = new Product();
        [BindProperty]
        public List<IFormFile>? Files { get; set; } = null!;

        private void SetSelectedCategories()
        {
            var subCategories = SubSelectedCategories;
            var mainCategories = MainSelectedCategories;
            SelectedCategories = new List<Guid>();
            SelectedCategories.AddRange(subCategories);
            SelectedCategories.AddRange(mainCategories);

        }
        public async Task<IActionResult> OnPostAsync(string action)
        {
            //if (!ModelState.IsValid)
            //  {
            //      return Page();
            //  }

            //  _context.Products.Add(Product);
            //  await _context.SaveChangesAsync();
            var t = Files;
            if(action == ACTION_CATEGORY_SELECT)
            {
                //SetSelectedCategories();
                //await for Initialize
            } else
            {
                //Create
                //clear error messages
                Messages.Clear();
                //check values
                Product = new Product()
                {
                    CreatedDate = DateTime.Now,
                    Description = Description,
                    IsDeleted = false,
                    ModifiedDate = DateTime.Now,
                    Name = Name,
                    Pictures = String.Empty
                };
                var state_action = ModelState["action"];
                var state_picture = ModelState["Pictures"];
                if (state_action != null)
                {
                    state_action.ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
                }
                if(state_picture != null && Files != null && Files.Count > 0)
                {
                    state_picture.ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
                    
                } else
                {
                    Messages.Add(MESSAGE_PICTURE_REQUIRED);
                }
                if(ModelState.IsValid)
                {
                    Product.IsDeleted = true;
                    var created = await _unitOfWork.ProductService.Create(Product);
                    SetSelectedCategories();
                    var check_categories = await _unitOfWork.ProductCategoryService.Implement(created, SelectedCategories);
                    var check = await _productBlob.UploadAsync(Files, created.Id);
                    if (check)
                    {
                        var pictures = _productBlob.GetURL(created);
                        if(pictures != null)
                        {
                            created.Pictures = AppService.Extension.Helper.MergeListString(pictures);
                            var updated = await _unitOfWork.ProductService.Update(p => p.Id == created.Id, created);
                        } else
                        {
                            return RedirectToPage("./Edit/"+created.Id);
                        }
                        
                    }
                    return RedirectToPage("./Index");
                } else
                {
                    
                    //set error messages
                    var state_name = ModelState["Name"];
                    var state_description = ModelState["Description"];
                    if(state_name != null && state_name.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
                    {
                        Messages.Add(MESSAGE_NAME_INVALID);
                    }
                    if(state_description != null && state_description.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
                    {
                        Messages.Add(MESSAGE_DESCRIPTION_INVALID);
                    }
                    await Initialize();
                    return Page();
                }
            }
            await Initialize();
            return Page();
        }
    }
}
