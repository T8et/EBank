using System;
using System.Collections.Generic;

namespace Database.Models;

public partial class BtUser
{
    public int UserId { get; set; }

    public string? UserName { get; set; }

    public string? UserPhone { get; set; }

    public string? UserEmail { get; set; }

    public string? UserAddress { get; set; }
}
