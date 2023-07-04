﻿using System;
using System.Collections.Generic;

namespace manager_properties_usa.Models.Model;

public partial class PropertyImage
{
    public int IdPropertyImage { get; set; }

    public int IdProperty { get; set; }

    public byte[] File { get; set; } = null!;

    public bool Enabled { get; set; }

    public virtual Property IdPropertyNavigation { get; set; } = null!;
}
