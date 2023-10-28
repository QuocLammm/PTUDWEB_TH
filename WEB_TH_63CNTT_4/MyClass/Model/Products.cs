using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Model
{
    [Table("Products")]
    public class Products
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Mặt hàng")]
        public int CatID { get; set; }

        [Required]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }

        [Display(Name = "Nhà cung cấp")]
        public string Supplier { get; set; }

        [Display(Name = "Tên rút gọn")]
        public string Slug { get; set; }

        [Required]
        [Display(Name = "Chi tiết")]
        public string Detail { get; set; }

        [Display(Name = "Ảnh sản phẩm")]
        public string Image { get; set; }

        [Required]
        [Display(Name = "Giá cả")]
        public decimal Price { get; set; }

        [Display(Name = "Giá khuyến mãi")]
        public decimal SalePrice { get; set; }

        [Display(Name = "Số lượng")]
        public decimal Amount { get; set; }

        [Required]
        [Display(Name = "Mô tả")]
        public string MetaDesc { get; set; }

        [Required]
        [Display(Name = "Từ khóa")]
        public string MetaKey { get; set; }

        [Required]
        [Display(Name = "Người tạo")]
        public int CreateBy { get; set; }

        [Required]
        [Display(Name = "Ngày tạo")]
        public DateTime CreateAt { get; set; }

        [Required]
        [Display(Name = "Người cập nhật")]
        public int UpdateBy { get; set; }

        [Required]
        [Display(Name = "Ngày cập nhật")]
        public DateTime UpdateAt { get; set; }

        [Required]
        [Display(Name = "Trạng thái")]
        public int Status { get; set; }

    }
}
