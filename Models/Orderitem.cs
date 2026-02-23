using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WpfApp1.Models;

[Table("orderitems", Schema = "shop")]
[Index("OrderId", "ProductId", Name = "orderitems_unique", IsUnique = true)]
public partial class Orderitem
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("order_id")]
    public int OrderId { get; set; }

    [Column("product_id")]
    public int ProductId { get; set; }

    [Column("quantity")]
    public int Quantity { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("Orderitems")]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("Orderitems")]
    public virtual Product Product { get; set; } = null!;
}
