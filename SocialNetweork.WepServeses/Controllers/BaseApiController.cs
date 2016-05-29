using SocialNetwork.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SocialNetweork.WepServeses.Controllers
{
    public class BaseApiController : ApiController
    {
        public BaseApiController() : this(new SocialNetworkContext())
        {

        }

        public BaseApiController(SocialNetworkContext data)
        {
            this.Data = data;
        }

        public SocialNetworkContext Data { get; set; }
    }
}