namespace AVBS.Office.UI.Models.User {
    public class UserSessionModel {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int OfficeId { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public int RoleId { get; set; }

        public string FullName {
            get { return Name + " " + Surname; }
            set { }
        }

    }
}