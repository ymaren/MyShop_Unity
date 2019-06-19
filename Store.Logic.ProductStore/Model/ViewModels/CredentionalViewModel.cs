using Store.Logic.Entity;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace Store.Logic.ProductStore.Models.ViewModels
{
   
    public class CredentionalViewModel
    {
        public int Id { get; set; }
        public string NameCredential { get; set; }
        public string FullNameCredential { get; set; }
        public int ParentCredentialId { get; set; }
        public int Order { get; set; }
        public string Url { get; set; }

        public CredentionalViewModel()
        {

        }
        public CredentionalViewModel(int id, string nameCredential, string fullNameCredential, int parentCredentialId,
            int order, string url
            )
        {
            this.Id = id;
            this.NameCredential = nameCredential;
            this.FullNameCredential = fullNameCredential;
            this.ParentCredentialId = parentCredentialId;
            this.Order = order;
            this.Url = url;
        }
    }
}
