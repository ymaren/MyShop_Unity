using Store.Logic.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace Store.Logic.ProductStore.Models.ViewModels
{
   
    public class UserRoleViewModel
    {
        public int Id { get; set; }
        public string UserRoleName { get; set; }
        public List<CredentionalViewModel> UserCredentional { get; set; }

        public UserRoleViewModel()
        {
            UserCredentional = new List<CredentionalViewModel>() { };
        }
    
        public UserRoleViewModel(int id, string userRoleName, List<CredentionalViewModel> credentionalListViewModels)
        {
            this.Id = Id;
            this.UserRoleName = userRoleName;
            this.UserCredentional = credentionalListViewModels;
        }
    }
}
