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

        //signup func
        [HttpPost]
        public HttpResponseMessage Signup(User obj)
        {
            if (obj == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Data");
            }

            try
            {

                var checkEmail = db.User.FirstOrDefault(u => u.Email == obj.Email);
                if (checkEmail != null)
                {
                    return Request.CreateResponse(HttpStatusCode.Conflict, "Email already exists");
                }
                db.User.Add(obj);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "Signup Successful");
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Server Error: " + ex.Message);
            }
        }

        //login api
        [HttpGet]
        public HttpResponseMessage Signin(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Fields empty. Please enter data.");
            }

            var user = db.User.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Password or Email is incorrect.");
            }


            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                Message = "Login Successful",
                User = new
                {
                    user.UserId,
                    user.Name
                }
            
            });
        }

    }

}