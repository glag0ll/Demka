using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WpfApp1.Models;

/// <summary>
/// Здесь основная характеристика о каждом товаре
/// </summary>
[Table("products", Schema = "shop")]
[Index("Article", Name = "products_unique", IsUnique = true)]
public partial class Product
{
    /// <summary>
    /// Уникальные номер товара
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// Уникальный артикул товара
    /// </summary>
    [Column("article", TypeName = "character varying")]
    public string Article { get; set; } = null!;

    /// <summary>
    /// Наименованеие товара
    /// </summary>
    [Column("name", TypeName = "character varying")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Единица измерения (например шт.)
    /// </summary>
    [Column("unit", TypeName = "character varying")]
    public string Unit { get; set; } = null!;

    [Column("price")]
    [Precision(10, 2)]
    public decimal Price { get; set; }

    [Column("supplier_id")]
    public int SupplierId { get; set; }

    [Column("manufacturer_id")]
    public int ManufacturerId { get; set; }

    [Column("category_id")]
    public int CategoryId { get; set; }

    [Column("discount")]
    [Precision(5, 2)]
    public decimal Discount { get; set; }

    [Column("stock_quantity")]
    public int StockQuantity { get; set; }

    [Column("description")]
    public string Description { get; set; } = null!;

    [Column("photo", TypeName = "character varying")]
    public string? Photo { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Products")]
    public virtual Category Category { get; set; } = null!;

    [ForeignKey("ManufacturerId")]
    [InverseProperty("Products")]
    public virtual Manufacturer Manufacturer { get; set; } = null!;

    [InverseProperty("Product")]
    public virtual ICollection<Orderitem> Orderitems { get; set; } = new List<Orderitem>();

    [ForeignKey("SupplierId")]
    [InverseProperty("Products")]
    public virtual Supplier Supplier { get; set; } = null!;
}
