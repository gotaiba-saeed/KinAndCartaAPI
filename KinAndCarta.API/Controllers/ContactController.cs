using KinAndCarta.API.Managers;
using KinAndCarta.API.Models;
using KinAndCarta.API.Repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using RoutePrefixAttribute = System.Web.Http.RoutePrefixAttribute;

namespace KinAndCarta.API.Controllers
{
    /// <summary>
    /// Contact controller:
    /// GET:
        /// GetContactByAddress
        /// SearchContactByEmailOrPhone
    /// POST
    /// PUT
    /// DELETE
    /// </summary>
    [RoutePrefix("api/contact")]
    public class ContactController : ApiController
    {
        static readonly IContact repository = new ContactRepository();
        public ContactController() { }
        public ContactController(IContact contacts)
        {
             contacts=repository;
        }
        /// <summary>
        /// Getting list of contacts based on the city or state
        /// </summary>
        /// <param name="city">optional</param>
        /// <param name="state">optional</param>
        /// <returns>List of contacts</returns>
        [HttpGet]
        [Route("GetContactsByAddress/{city?}/{state?}")]
        public IEnumerable<Contact> GetContactsByAddress(string city = null, string state = null)
        {
            return repository.GetAllContactsByAddress(city,state);
        }
        /// <summary>
        /// Search in a list of contacts by email or phone.
        /// </summary>
        /// <param name="email">optional</param>
        /// <param name="Phone">optional and acts as the wor kphone or personal phone</param>
        /// <returns>list of contacts</returns>
        [HttpGet]
        [Route("Search/{email?}/{phone?}")]
        public async Task<IEnumerable<Contact>> SearchContactByEmailOrPhone(string email=null, string Phone=null)
        {
            return await repository.GetContactByEmailOrPhone(email, Phone);
        }
        /// <summary>
        /// Create a new contact and using the Multipart form in order to catch the a image if exsited 
        /// </summary>
        /// <returns>status 200 or 400 or validation error</returns>
        public async Task<HttpResponseMessage> Post()
        {
            var tuple=await ContactManager.ProcessMultiPartAsync(Request);
            Validate(tuple._contact);
            if (tuple._provider.FileData.Count>0)
            {
                var mediaType = tuple._provider.FileData.Select(a => a.Headers.ContentType.MediaType).FirstOrDefault();
                if (!ContactManager.ValidateContentType(mediaType))
                {
                    ModelState.AddModelError("Image", "Invalid image format");
                }
                if(tuple._provider.FileData.Count>1)
                {
                    ModelState.AddModelError("Image", "One image is allowed");
                }
            }
            if (ModelState.IsValid)                            
                return Helper.ReturnStatus(await repository.Insert(tuple._contact, tuple._provider));            
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }        
        /// <summary>
        /// Update certain contact with provided data. Use Multipart form to catch the an image if exsited
        /// </summary>
        /// <returns>Status 200 or 400 or validation errors</returns>
        public async Task<HttpResponseMessage> Put()
        {
            var tuple = await ContactManager.ProcessMultiPartAsync(Request);
            Validate(tuple._contact);
            if (tuple._provider.FileData.Count > 0)
            {
                var mediaType = tuple._provider.FileData.Select(a => a.Headers.ContentType.MediaType).FirstOrDefault();
                if (!ContactManager.ValidateContentType(mediaType))
                {
                    ModelState.AddModelError("Image", "Invalid image format");
                }
                if (tuple._provider.FileData.Count > 1)
                {
                    ModelState.AddModelError("Image", "One image is allowed");
                }
            }
            if (ModelState.IsValid)
                return Helper.ReturnStatus(await repository.Update(tuple._contact, tuple._provider));
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }
        /// <summary>
        /// Delete a contact document from the list
        /// </summary>
        /// <param name="id">required - the contact id</param>
        /// <returns>status 200 or 400</returns>
        public async Task<HttpResponseMessage> Delete(int id)
        {            
            return Helper.ReturnStatus(await repository.Delete(id));
        }        
    }
}
