// See https://aka.ms/new-console-template for more information
using AppCore;
using AppCore.Entities;

var context = new SqlContext();
/*
context.Set<Role>().Add(new Role()
{
    Name = "Administrator"
});
*/
var role_admin = context.Set<Role>().FirstOrDefault();
context.Set<User>().Add(new User()
{
    RoleId = role_admin.Id,
    Password = "123"
}) ;

context.SaveChanges();
