using ApiService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiService.Adapter.Model
{
    public static class CategoryViewModelAdapter
    {
        private static CategoryView Converter(Category category)
        {
            return new CategoryView()
            {
                Name= category.Name,
            };
        }
        public static List<CategoryView> FromCategoryDTOModels(List<Category> categories)
        {            
            var result = new List<CategoryView>();
            var groups = categories.GroupBy(c => c.ParentId).OrderBy(g => g.Key);
            var group_parent = groups.Where(g => g.Key == null).First();
            foreach ( var parent in group_parent)
            {
                var current = Converter(parent);
                var group_children = groups.Where(g => g.Key == parent.Id).FirstOrDefault();
                if(group_children!= null)
                {
                    //has children
                    foreach (var child in group_children)
                    {
                        current.Children.Add(Converter(child));
                    }
                }
                result.Add(current);
            }
            return result;
        }
    }
}
