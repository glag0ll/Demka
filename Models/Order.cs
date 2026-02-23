using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WpfApp1.Models;

[Table("orders", Schema = "shop")]
[Index("OrderNumber", Name = "orders_unique", IsUnique = true)]
public partial class Order
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("order_number")]
    public int OrderNumber { get; set; }

    [Column("order_date")]
    public DateOnly OrderDate { get; set; }

    [Column("delivery_date")]
    public DateOnly DeliveryDate { get; set; }

    [Column("pickup_point_id")]
    public int PickupPointId { get; set; }

    [Column("user_id")]
    public int? UserId { get; set; }

    [Column("pickup_code")]
    public int PickupCode { get; set; }

    [Column("status")]
    [StringLength(50)]
    public string Status { get; set; } = null!;

    [InverseProperty("Order")]
    public virtual ICollection<Orderitem> Orderitems { get; set; } = new List<Orderitem>();

    [ForeignKey("PickupPointId")]
    [InverseProperty("Orders")]
    public virtual Pickuppoint PickupPoint { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Orders")]
    public virtual User? User { get; set; }
}
