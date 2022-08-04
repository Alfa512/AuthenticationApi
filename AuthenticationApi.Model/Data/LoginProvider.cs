using System;
using System.Collections.Generic;

namespace AuthenticationApi.Model.Data;

public class LoginProvider
{
    public int Id { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime ChangeDate { get; set; }
    public string ChangeBy { get; set; }
    public string ProviderCode { get; set; }
    public string ProviderDisplayName { get; set; }
    public string ProviderUrl { get; set; }

    public virtual ICollection<Login> Logins { get; set; }
}