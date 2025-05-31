using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.CRUD.MVC.DAL.Models
{
    public class Department : BaseEntity
    {
       

        [Required(ErrorMessage = "Code Is Required !!")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Name Is Required !!")]
        public string Name { get; set; }

        [DisplayName("Date Of Creation")]
        public DateTime DateOfCreation { get; set; }

        public ICollection<Employee>? Employees { get; set; } // Navigational Property
    }
}
