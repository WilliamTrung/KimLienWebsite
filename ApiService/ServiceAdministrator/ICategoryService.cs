using ApiService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiService.ServiceAdministrator
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetCategories();
        /// <summary>
        /// Update new Category
        /// <para>Throw DuplicateNameException: Duplicated name on category</para>
        /// </summary>
        /// <param name="category"></param>
        /// <exception cref="DuplicateNameException"></exception>
        void Update(Category category);
        //void AdjustPicture(Product product);
        /// <summary>
        /// Remove any record and its children related to deleted category in ProductCategories table
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(Guid id);
        /// <summary>
        /// Add new Category
        /// <para>Throw DuplicateNameException: Duplicated name on category</para>
        /// <para>Throw ArgumentException: Selected parent for this category is a child of another category</para>
        /// </summary>
        /// <param name="category"></param>
        /// <exception cref="DuplicateNameException"></exception>
        /// <exception cref="ArgumentException"></exception>
        void Add(Category category);
    }
}
