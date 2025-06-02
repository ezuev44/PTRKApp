using PTRKApp.AppData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace PTRKApp.Models
{
    public class UsersWithRole
    {
        public string RoleName { get; set; }

        public bool IsAdmin { get; set; }

        public int Id { get; set; }

        [StringLength(50)]
        public string Login { get; set; }

        [StringLength(50)]
        public string Password { get; set; }

        public int RoleId { get; set; }

        public string Speciality { get; set; }

        public string BirdDate { get; set; }

        public string FIO { get; set; }

        public string Email { get; set; }
    }
}
