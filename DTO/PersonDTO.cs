using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class PersonDTO
    {
        public int PersonId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool? Status { get; set; }

        public DateTime RowInsertTime { get; set; }
        public DateTime RowUpdateTime { get; set; }
        public DateTime? RowInsertTimeStatus { get; set; }
        public DateTime? RowUpdateTimeStatus { get; set; }
    }
}
