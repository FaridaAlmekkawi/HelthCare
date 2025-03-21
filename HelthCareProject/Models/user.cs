﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HelthCareProject.Models;

[Index("Email", Name = "idx_users_email")]
[Index("FirstName", "LastName", Name = "idx_users_name")]
public partial class user
{
    [Key]
    [StringLength(100)]
    public string Id { get; set; }

    [StringLength(30)]
    public string FirstName { get; set; }

    [StringLength(30)]
    public string LastName { get; set; }

    [StringLength(30)]
    public string Email { get; set; }

    [StringLength(30)]
    public string PhoneNumber { get; set; }

    [StringLength(250)]
    public string PasswordHash { get; set; }

    [Column(TypeName = "bit(1)")]
    public ulong? EmailConfirmed { get; set; }

    [Column(TypeName = "bit(1)")]
    public ulong? PhoneNumberConfirmed { get; set; }

    [InverseProperty("User")]
    public virtual assistant assistant { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<chatbotmessage> chatbotmessages { get; set; } = new List<chatbotmessage>();

    [InverseProperty("User")]
    public virtual doctor doctor { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<message> messages { get; set; } = new List<message>();

    [InverseProperty("User")]
    public virtual ICollection<notification> notifications { get; set; } = new List<notification>();

    [InverseProperty("User")]
    public virtual patient patient { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<userclaim> userclaims { get; set; } = new List<userclaim>();

    [ForeignKey("UserId")]
    [InverseProperty("Users")]
    public virtual ICollection<chat> Chats { get; set; } = new List<chat>();

    [ForeignKey("UserId")]
    [InverseProperty("Users")]
    public virtual ICollection<role> Roles { get; set; } = new List<role>();
}