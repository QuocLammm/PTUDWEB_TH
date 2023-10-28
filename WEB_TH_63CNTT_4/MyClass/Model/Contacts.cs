using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace MyClass.Model
    {
        [Table("Contacts")]
        public class Contacts
        {
            [Key]
            public int Id { get; set; }

            [Required]
            [Display(Name = "Mã người liên lạc")]
            public int UserId { get; set; }

            [Display(Name = "Tên liên lạc")]
            public string FullName { get; set; }

            [Display(Name = "Số điện thoại")] 
            public string Phone { get; set; }

            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Display(Name = "Tiêu đề")]
            public string Title { get; set; }

            [Required]
            [Display(Name = "Chi tiết")]
            public string Detail { get; set; }


            [Required]
            [Display(Name = "Ngày tạo")]
            public DateTime CreateAt { get; set; }

            [Required]
            [Display(Name = "Người cập nhật")]
            public int UpdateBy { get; set; }

            [Required]
            [Display(Name = "Ngày cập nhật")]
            public DateTime UpdateAt { get; set; }

            [Display(Name = "Trạng thái")]
            public int Status { get; set; }

        }
    }

}
