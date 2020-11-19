using KinAndCarta.API.Managers;
using KinAndCarta.API.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;

namespace KinAndCarta.API.Repository
{
    public class ContactRepository:IContact
    {
        private List<Contact> contacts = new List<Contact>();
        private int _nextId = 1;

        private readonly Contact _contact = new Contact
        {
            name = "name Test 1",
            company = "Kin+Cartar Test 1",
            profileImageUri = "default-profile.jpg",
            email = "Test1@test.com",
            birthdate = DateTime.Today.ToShortDateString(),
            personalPhone = "123456789",
            workPhone = "123456789",
            address = new Address
            {                
                city = "Chicago",
                state = "Illinois",
                street = "8925 NE Ter"
            }
        };
        public ContactRepository()
        {            
             Insert(_contact,null);
        }
        public IEnumerable<Contact> GetAllContactsByAddress(string city = null, string state = null)
        {
            var allContacts = contacts.ToList();
            if (!string.IsNullOrEmpty(city))
                allContacts = allContacts.Where(c => c.address.city.Contains(city)).ToList();
            if (!string.IsNullOrEmpty(state))
                allContacts = allContacts.Where(c => c.address.state.Contains(state)).ToList();
            return allContacts;
        }
        public async Task<Contact> GetContactById(int id)
        {
            return await Task.Run(() =>
            {
                return contacts.FirstOrDefault(c => c.id == id);
            });
        }
        public async Task<bool> Insert(Contact contact, MultipartFormDataStreamProvider provider)
        {
            if (contact == null)
                throw new ArgumentNullException("Contact data is null");
            contact.id = _nextId++;
            contact.address.id = contact.id;
            string uri = ContactManager.ProcessFileFromMultiPart(provider, contact.id);
            contact.profileImageUri = uri;
            contacts.Add(contact);
            return true;
        }

        public async Task<bool> Update(Contact contact, MultipartFormDataStreamProvider provider)
        {
            if (contact == null)
                throw new ArgumentNullException("Contact data is null");
            int index = contacts.FindIndex(c => c.id == contact.id);
            if (index == -1)
            {
                return false;
            }
            contacts.RemoveAt(index);
            contact.address.id = contact.id;
            string uri = ContactManager.ProcessFileFromMultiPart(provider, contact.id);
            contact.profileImageUri = uri;
            contacts.Add(contact);
            return true;
        }
        public async Task<bool> Delete(int id)
        {
            return await Task.Run(() =>
            {
                if (contacts.Where(c => c.id == id).Any())
                {
                    contacts.RemoveAll(c => c.id == id);
                    return true;
                }
                else
                    return false;
            });            
        }
        public async Task<IEnumerable<Contact>> GetContactByEmailOrPhone(string email, string Phone)
        {
            return await Task.Run(() =>
            {
                var allContacts = contacts.ToList();
                if (!string.IsNullOrWhiteSpace(email))
                    allContacts = allContacts.Where(c => c.email.Contains(email)).ToList();
                if (!string.IsNullOrWhiteSpace(Phone))
                    allContacts = allContacts.Where(c => c.personalPhone.Contains(Phone) || c.workPhone.Contains(Phone)).ToList();
                return allContacts;
            });
        }
    }
}