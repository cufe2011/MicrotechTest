using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TestDevWebApi.Models;

public partial class Account
{
    [Key]
    [Column("Acc_Number")]
    [StringLength(10)]
    public string AccNumber { get; set; } = null!;

    [Column("ACC_Parent")]
    [StringLength(10)]
    public string? AccParent { get; set; }

    [Column(TypeName = "decimal(20, 9)")]
    public decimal? Balance { get; set; }

    [ForeignKey("AccParent")]
    [InverseProperty("InverseAccParentNavigation")]
    public virtual Account? AccParentNavigation { get; set; }

    [InverseProperty("AccParentNavigation")]
    public virtual ICollection<Account> InverseAccParentNavigation { get; set; } = new List<Account>();
}
