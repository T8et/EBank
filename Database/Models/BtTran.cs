using System;
using System.Collections.Generic;

namespace Database.Models;

public partial class BtTran
{
    public string TranId { get; set; } = null!;

    public int? TranFrAccId { get; set; }

    public int? TranToAccId { get; set; }

    public int? Amount { get; set; }

    public DateTime? TranDate { get; set; }

    public int? TranSts { get; set; }
}
