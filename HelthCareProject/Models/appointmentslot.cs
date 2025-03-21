﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HelthCareProject.Models;

[Index("IsAvailable", Name = "idx_appointmentslots_availability")]
[Index("ClinicId", Name = "idx_appointmentslots_clinic")]
[Index("ClinicId", "IsAvailable", Name = "idx_appointmentslots_clinicavailability")]
[Index("StartTime", "EndTime", Name = "idx_appointmentslots_timerange")]
public partial class appointmentslot
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "time")]
    public TimeOnly? StartTime { get; set; }

    [Column(TypeName = "time")]
    public TimeOnly? EndTime { get; set; }

    public int? ClinicId { get; set; }

    [Column(TypeName = "bit(1)")]
    public ulong? IsAvailable { get; set; }

    [ForeignKey("ClinicId")]
    [InverseProperty("appointmentslots")]
    public virtual clinic Clinic { get; set; }
}