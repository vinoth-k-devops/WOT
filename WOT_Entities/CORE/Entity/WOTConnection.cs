using System;
using System.Collections.Generic;

namespace WOT_API;

public partial class WOTConnection
{
    public string ConnId { get; set; } = null!;

    public string? ServerName { get; set; }

    public string? DbName { get; set; }

    public string? UserId { get; set; }

    public string? Passcode { get; set; }

    public bool? Active { get; set; }
}
