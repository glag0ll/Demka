using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WpfApp1.Models;

[Table("pickuppoints", Schema = "shop")]
[Index("Address", Name = "pickuppoints_unique", IsUnique = true)]
public partial class Pickuppoint
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("address")]
    public string Address { get; set; } = null!;

    [InverseProperty("PickupPoint")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
