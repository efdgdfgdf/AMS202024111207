using System;
using System.Collections.Generic;

namespace AMS202024111207.Models;

public partial class Asset
{
    public int AssetId { get; set; }

    public string AssetName { get; set; } = null!;

    public string Specification { get; set; } = null!;

    public decimal Price { get; set; }

    public DateTime? PurchaseDate { get; set; }

    public string Location { get; set; } = null!;

    public int? CategoryId { get; set; }

    public string? ImgName { get; set; }

    public string? CustodianId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual Employee? Custodian { get; set; }
}
