using MyShop.Models.MyShopModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyShop.Models
{
    public class RoleViewModel
    {
        public UserRole userRole { get; set; }
        public ICollection <Credential> allCredential { get; set; }
        public int[] SelectedCredential { get; set; }
        public RoleViewModel(UserRole userRole, ICollection<Credential> allCredential)
        {
            this.userRole = userRole;
            this.allCredential = allCredential;
        }
        public RoleViewModel()
        {
            allCredential = new List<Credential>();
        }
        //public FillSelectedCredential()
        //{
        //    DBContext _db = new DBContext();
        //    foreach (int item in role.SelectedCredential)
        //    {
        //        _db.Credentials
        //        role.userRole.Credential.FirstOrDefault(c => c.Id == item).UserRole.Add(role.userRole);
        //        //role.userRole.Credential.Add(_credential.GetAll().FirstOrDefault(c => c.Id == item));
        //    }
        //}
    }
}