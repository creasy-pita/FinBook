using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using User.API.Dto;

namespace User.API.Controllers
{
    public class BaseController : Controller
    {
        protected UserIdentity UserIdentity
        {
            get
            {
                var identity = new UserIdentity();
                //TBD
                //identity.UserId = Convert.ToInt32(User.Claims.FirstOrDefault());
                //identity.Name = User.Claims.FirstOrDefault(c => c.Type == "name").Value ?? "";
                //identity.Company = User.Claims.FirstOrDefault(c => c.Type == "company").Value ?? "";
                //identity.Avatar = User.Claims.FirstOrDefault(c => c.Type == "avatar").Value ?? "";
                //identity.Title = User.Claims.FirstOrDefault(c => c.Type == "title").Value ?? "";

                identity.UserId = 1;
                identity.Name = "creasypita";
                identity.Company = "company";
                identity.Avatar = "avatar";
                identity.Title = "title";
                return identity;
            }


        }



    }
}
