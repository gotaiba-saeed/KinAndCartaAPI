using KinAndCarta.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace KinAndCarta.API.Repository
{
    public interface IContact
    {
        IEnumerable<Contact> GetAllContactsByAddress(string city=null,string state=null);
        Task<Contact> GetContactById(int id);
        Task<bool> Insert(Contact contact, MultipartFormDataStreamProvider provider);
        Task<bool> Update(Contact contact,MultipartFormDataStreamProvider provider);
        Task<bool> Delete(int id);
        Task<IEnumerable<Contact>> GetContactByEmailOrPhone(string email = null, string Phone=null);
    }
}