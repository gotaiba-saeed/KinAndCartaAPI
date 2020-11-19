using KinAndCarta.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace KinAndCarta.API.Managers
{
    public static class ContactManager
    {
        /// <summary>
        /// Processing the request by adding a tupe of Contact type and DataStreamProvider to handle both raw data and image file
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public static async Task<(Contact _contact, MultipartFormDataStreamProvider _provider)> ProcessMultiPartAsync(HttpRequestMessage Request)
        {
            var provider = new MultipartFormDataStreamProvider(Constants.IMAGES_ROOT);
            var result = await Request.Content.ReadAsMultipartAsync(provider);
            var json = await result.Contents.FirstOrDefault(f => f.Headers.ContentType == null).ReadAsStringAsync();
            var contact = JsonConvert.DeserializeObject<Contact>(json);
            return (_contact: contact, _provider: provider);
        }
        public static string ProcessFileFromMultiPart(MultipartFormDataStreamProvider provider, int id)
        {
            string filename = null;
            if (provider != null)
            {
                if (provider.FileData.Count == 1)
                {
                    try
                    {
                        var file = provider.FileData.FirstOrDefault();
                        var name = file.Headers.ContentDisposition.FileName;
                        name = name.Trim('"');
                        filename = $"{DateTime.Now.ToString("hhmmss")}_{id}_{name}";
                        var localFileName = file.LocalFileName;
                        var filePath = Path.Combine(Constants.IMAGES_ROOT, filename);
                        File.Move(localFileName, filePath);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return filename;
        }
        public static bool ValidateContentType(string mediaType)
        {
            string[] mediaTypeList = { "image/jpg", "image/png", "image/jpeg" };
            return mediaTypeList.Any(a => a == mediaType);
        }
    }
}