
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ProductsApi
{
    public class Product
    {
    [Key]
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
   
    public double Price { get; set; }
    public string Description { get; set; } = string.Empty;
    }
}