using System.ComponentModel.DataAnnotations;

namespace BuoiToi.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    
    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;
    
    public string NameCustomer { get; set; } = string.Empty;

    public string NumberPhone { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;
}