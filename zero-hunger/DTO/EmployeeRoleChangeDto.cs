using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using zero_hunger.EF.Models;

namespace zero_hunger.DTO
{
    public class EmployeeRoleChangeDto
    {
        public int Id { get; set; }
        public UserRole Role { get; set; }
    }
}