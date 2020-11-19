using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace KinAndCarta.API
{
    public static class Helper
    {
        public static HttpResponseMessage ReturnStatus(bool result)
        {
            if (result)
                return new HttpResponseMessage(HttpStatusCode.OK);
            else
                return new HttpResponseMessage(HttpStatusCode.BadRequest); ;
        }
    }
}