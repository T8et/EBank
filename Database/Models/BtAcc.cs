using System;
using System.Collections.Generic;

namespace Database.Models;

public partial class BtAcc
{
    public int AccId { get; set; }

    public int? UserId { get; set; }

    public int? AccBalance { get; set; }

    public string? AccPin { get; set; }
}
