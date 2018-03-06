using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlotCarsGo_Server.Models
{
    public class DaimToGoogleOAuth
    {
        /// <summary>
        /// Creates the auth url
        /// https://accounts.google.com/o/oauth2/auth?client_id={clientid}&amp;redirect_uri=urn:ietf:wg:oauth:2.0:oob&amp;scope={scope}&amp;response_type=code
        /// client id from google developer console.
        /// scope comma seproated 
        /// </summary>
        /// <returns></returns>
        public static Uri GetAuthURI()
        {
            return new Uri(string.Format("https://accounts.google.com/o/oauth2/auth?client_id={0}&redirect_uri=urn:ietf:wg:oauth:2.0:oob&scope={1}&response_type=code", "Properties.Resources.clientId", "Properties.Resources.scope"));
        }


    }
}