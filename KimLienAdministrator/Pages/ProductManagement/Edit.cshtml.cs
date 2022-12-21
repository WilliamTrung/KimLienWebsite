using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppCore;
using AppService.UnitOfWork;
using AppService.DTOs;
using AppService.Models;
using KimLienAdministrator.Helper.Azure.IBlob;

namespace KimLienAdministrator.Pages.ProductManagement
{
    public class EditModel : PageModel
    {
        public readonly string ACTION_CATEGORY_SELECT = "CategorySelect";
        public readonly string ACTION_CATEGORY_SAVE = "CategorySave";
        public readonly string ACTION_SAVE = "Save";
        //Messages
        public List<string> Messages { get; set; } = new List<string>();

        //Pictures value
        public readonly string PIC_DELETE = "Xóa";
        public readonly string PIC_PRIMARY = "Đặt ảnh Chính";
        public readonly string PIC_UPLOAD = "Tải lên";
        
        public List<IFormFile>? Files { get; set; }
        [BindProperty]
        public IList<string> DeserializedPics { get; set; } = null!;
        public SelectList Pictures { get; set; } = null!;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductBlob _productBlob;

        [BindProperty(SupportsGet = true)]
        public List<Guid> MainSelectedCategories { get; set; } = null!;
        [BindProperty(SupportsGet = true)]
        public List<Guid> SubSelectedCategories { get; set; } = null!;


        public Dictionary<string,SelectList> SubCategories { get; set; } =  new Dictionary<string, SelectList>();
        public SelectList MainCategories { get; set; } = null!;
        public EditModel(IUnitOfWork unitOfWork, IProductBlob productBlob)
        {
            _unitOfWork = unitOfWork;
            _productBlob = productBlob;
            
        }

        [BindProperty]
        public Product Product { get; set; } = default!;
        
        public ProductModel ProductModel { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(Guid? id, List<string>? messages)
        {
            if (id == null)
            {
                return RedirectToPage("./Index");
            }

            
            //Product = found;
            if(await Initialize((Guid)id))
            {
                if(messages != null)
                    Messages = messages;
                return Page();
            } else
            {
                return RedirectToPage("./Index");
            }
        }
        private async Task<bool> Initialize(Guid id)
        {
            var find = await _unitOfWork.ProductService.GetProductModels(m => m.Id == id);
            //var find =  await _unitOfWork.ProductService.GetDTOs(m => m.Id == id);
            if (find == null)
            {
                return false;
            }
            var found = find.FirstOrDefault();
            if (found == null)
            {
                return false;
            }
            ProductModel = found;
            Product = ProductModel.Product;
            DeserializedPics = Product.DeserializedPictures;
            MainCategories = await SetMainCategories();
            Pictures = new SelectList(DeserializedPics, Pictures);
            
            foreach (var category in MainSelectedCategories)
            {
                await SetSubCategories(category);
            }
            
            return true;
        }
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostUpdateAsync()
        {
            //https://localhost:7277/ProductManagement/Edit?id=fa53df43-bf29-4dbc-5495-08dad14a485b
            //update data
            if (!ModelState.IsValid && ModelState.ErrorCount > 1)
                //error on pictures
            {
                return Page();
            }
            var temp = Product;
            var result = await _unitOfWork.ProductService.Update(p => p.Id == Product.Id, Product);
            var check = await Initialize(Product.Id);
            return Page();
        }
        public async Task<IActionResult> OnPostCategoryAsync(string action)
        {
            if (action == ACTION_CATEGORY_SELECT)
            {
                /*
                foreach (var category in MainSelectedCategories)
                {
                    await SetSubCategories(category);
                }
                */
            }
            else if (action == ACTION_CATEGORY_SAVE)
            {
                var subCategories = SubSelectedCategories;
                var mainCategories = MainSelectedCategories;
                var categories = new List<Guid>();
                categories.AddRange(subCategories);
                categories.AddRange(mainCategories);
                var check = await _unitOfWork.ProductCategoryService.Implement(Product, categories);
            }
            var check2 = await Initialize(Product.Id);
            return Page();
        }
        public async Task<IActionResult> OnPostSetPrimaryPicture(string selected)
        {
            var pic_name = selected;
            var check1 = await Initialize(Product.Id);
            if (pic_name != DeserializedPics.First())
            {
                var check = await _unitOfWork.ProductService.SetPrimaryPicture(Product.Id, pic_name);
            }
            
            return Page();
        }
        //DeleteSelectedPicture
        public async Task<IActionResult> OnPostDeleteSelectedPictureAsync(string selected)
        {
            var pic_name = selected;
            //Delete picture
            var check_deleted = await _productBlob.DeleteAsync(Product.Id, pic_name);

            var check = await Initialize(Product.Id);
            return Page();
        }
        //
        public async Task<IActionResult> OnPostUploadPicturesAsync()
        {
            
            if (Files != null && Files.Count > 0)
            {
                var check = await _productBlob.UploadAsync(Files, Product.Id);
            }
            var check1 = await Initialize(Product.Id);
            return Page();
        }
        private async Task<SelectList> SetMainCategories()
        {
            var categories = await _unitOfWork.CategoryService.GetDTOs(filter: c => c.ParentId == null);
            return new SelectList(categories, "Id", "Name");
        }
        private async Task SetSubCategories(Guid parentId)
        {
            var subCategories = await _unitOfWork.CategoryService.GetDTOs(filter: c => c.ParentId == parentId);
            if(subCategories != null && subCategories.Count() > 0)
            {
                string name = parentId.ToString();
                var list = new SelectList(subCategories, "Id", "Name");
                SubCategories.Add(subCategories.First().Parent.Name, list);
            }
            
        }
    }
}
