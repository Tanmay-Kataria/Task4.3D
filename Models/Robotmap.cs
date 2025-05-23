﻿using System;
using System.Collections.Generic;

namespace robot_controller_api.Models;

public partial class Robotmap
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public int Columns { get; set; }

    public int Rows { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime Modifieddate { get; set; }

    public bool? Issquare { get; set; }
}
