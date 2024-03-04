using System;
using System.Collections.Generic;

namespace WOT_API;

public partial class DbUser
{
    public string ConnId { get; set; } = null!;

    public string DbName { get; set; } = null!;

    public int UserId { get; set; }

    public string EmpId { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Passcode { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string MiddleName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public DateTime Dob { get; set; }

    public string EmailId { get; set; } = null!;

    public string MobileNo { get; set; } = null!;

    public bool LoginEnable { get; set; }
}
