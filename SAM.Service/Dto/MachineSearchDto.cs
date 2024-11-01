﻿using SAM.Entities.Enum;
using System.Diagnostics.CodeAnalysis;

namespace SAM.Services.Dto;
[ExcludeFromCodeCoverage]
public class MachineSearchDto : BaseDto
{
    public string? Name { get; set; }
    public MachineStatusEnum? Status { get; set; }
    public DateTime? LastMaintenance { get; set; }
    public int? IdUnit { get; set; }
}
