using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationApi.Model.Data;

public class UserRole
{
    [Key, Column(Order = 0)]
    public long UserId { get; set; }
    [Key, Column(Order = 1)]
    public long RoleId { get; set; }
}