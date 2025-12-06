using Fyp_rx.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Fyp_rx.Controllers
{
    public class UserController : ApiController
    {
        RX_SCREENINGEntities db = new RX_SCREENINGEntities();
        [HttpPost]
        public HttpResponseMessage Signup(User obj)
        {
            if (obj != null)
            {
                db.User.Add(obj);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Signup Successfull");
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Some Thing Wrong");
        }
        [HttpGet]
        public HttpResponseMessage Signin(String email, String Password)
        {
            if (email != null && Password != null)
            {

                var res = db.User.Where(u => u.Email == email && u.Password == Password).FirstOrDefault();
                if (res == null)
                {

                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Password Or Email Is Incorrect");

                }
                else
                {


                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Login SuccessFull...");


                }
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, "Fields Empty Enter Data.........");


        }
    }
}

