using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace AVBS.Office.UI.Models.User {
    public class UserTableModel {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string ImageUrl { get; set; }

    }
}