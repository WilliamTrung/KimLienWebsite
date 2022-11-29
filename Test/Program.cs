// See https://aka.ms/new-console-template for more information
using AppCore;
using AppCore.Entities;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

var context = new SqlContext();
//context.Set<Product>().Add(new Product()
//{
//    CreatedDate = DateTime.UtcNow,
//    Description = "test",
//    IsDeleted = false,
//    ModifiedDate = DateTime.UtcNow,
//    Pictures = "https://res.cloudinary.com/dvqruuvxs/image/upload/v1669435692/cld-sample-4.jpg,https://res.cloudinary.com/dvqruuvxs/image/upload/v1669435691/cld-sample-3.jpg",
//    Name = "Túi"
//}) ;
//var product = context.Products.Where(p => p.Name == "Túi").FirstOrDefault();
//var category = context.Categories.Where(p => p.Name == "Sec").FirstOrDefault();
//if(product == null || category == null)
//{
//    return;
//}
//Console.WriteLine("asd");
//context.Set<ProductCategory>().Add(new ProductCategory()
//{
//    ProductId = product.Id,
//    CategoryId = category.Id
//}); ;
//context.SaveChanges();


//upload pictures
Account account = new Account(
  "dvqruuvxs",
  "788196118532125",
  "PnTvk-BOrwINlOuwE4vmVl4opfA");

Cloudinary cloudinary = new Cloudinary(account);
string test = "https://res.cloudinary.com/dvqruuvxs/image/upload/v1669435692/cld-sample-4.jpg,https://res.cloudinary.com/dvqruuvxs/image/upload/v1669435691/cld-sample-3.jpg";
var uploadParams = new ImageUploadParams()
{
    //File = 
    PublicId = "olympic_flag3"
};
var uploadResult = cloudinary.Upload(uploadParams);

var link = uploadResult.Uri;
Console.WriteLine(link);

var result = cloudinary.DeleteDerivedResources("olympic_flag3");
foreach(var resource in result.Deleted)
{
    Console.WriteLine(resource);
}
//https://res.cloudinary.com/dvqruuvxs/image/upload/v1669643099/olympic_flag.jpg
//https://res.cloudinary.com/dvqruuvxs/image/upload/v1669435692/cld-sample-5.jpg
//https://res.cloudinary.com/dvqruuvxs/image/upload/v1669643226/olympic_flag2.jpg
