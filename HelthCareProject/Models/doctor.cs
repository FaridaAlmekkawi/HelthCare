﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HelthCareProject.Models;

[Index("GovernorateId", Name = "idx_doctors_governorate")]
[Index("SpecializationId", Name = "idx_doctors_specialization")]
[Index("UserId", Name = "idx_doctors_userid", IsUnique = true)]
[Index("IsVerified", Name = "idx_doctors_verified")]
public partial class doctor
{
    [Key]
    public int Id { get; set; }

    [StringLength(250)]
    public string Bio { get; set; }

    public byte[] ProfilePicture { get; set; }

    [Column(TypeName = "bit(1)")]
    public ulong? IsVerified { get; set; }

    public byte[] WorkCertificate { get; set; }

    public int? SpecializationId { get; set; }

    public int? GovernorateId { get; set; }

    [StringLength(100)]
    public string UserId { get; set; }

    [ForeignKey("GovernorateId")]
    [InverseProperty("doctors")]
    public virtual governorate Governorate { get; set; }

    [ForeignKey("SpecializationId")]
    [InverseProperty("doctors")]
    public virtual specialization Specialization { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("doctor")]
    public virtual user User { get; set; }

    [InverseProperty("Doctor")]
    public virtual ICollection<appointment> appointments { get; set; } = new List<appointment>();

    [InverseProperty("Doctor")]
    public virtual ICollection<assistant> assistants { get; set; } = new List<assistant>();

    [InverseProperty("Doctor")]
    public virtual ICollection<clinic> clinics { get; set; } = new List<clinic>();

    [InverseProperty("Doctor")]
    public virtual ICollection<message> messages { get; set; } = new List<message>();

    [InverseProperty("Doctor")]
    public virtual ICollection<patientdoctor> patientdoctors { get; set; } = new List<patientdoctor>();
}