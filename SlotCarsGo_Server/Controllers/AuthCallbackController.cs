using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.PeopleService.v1;
using Google.Apis.Services;
using SlotCarsGo_Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SlotCarsGo_Server.Controllers
{
    public class AuthCallbackController : Google.Apis.Auth.OAuth2.Mvc.Controllers.AuthCallbackController
    {
        protected override Google.Apis.Auth.OAuth2.Mvc.FlowMetadata FlowData
        {
            get { return new AppFlowMetadata(); }
        }

        public async Task<ActionResult> IndexAsync(CancellationToken cancellationToken)
        {
            var result = await new AuthorizationCodeMvcApp(this, new AppFlowMetadata()).
                AuthorizeAsync(cancellationToken);

            if (result.Credential != null)
            {
                PeopleServiceService peopleService = new PeopleServiceService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = result.Credential,
                    ApplicationName = "SlotCarsGo",
                });
                PeopleResource.GetRequest peopleRequest = peopleService.People.Get("me");
                peopleRequest.PersonFields = "coverPhotos";
                var profile = peopleRequest.Execute();

                return View();
            }
            else
            {
                return new RedirectResult(result.RedirectUri);
            }
        }
    }
}