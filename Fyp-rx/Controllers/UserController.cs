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
            if (email==null || password==null)
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

        [HttpGet]
        public HttpResponseMessage Profile(int id)
        {
            if (id<=0)
            {
                //id jo login ka time variable ma save ho ge front end ma
                return Request.CreateResponse(HttpStatusCode.BadRequest, "something wrong please put id from your saved variable");
            }
           
                var Profile = db.User.FirstOrDefault(p => p.UserId==id);

            if (Profile==null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Profile Not Found");
            }

            return (Request.CreateResponse(HttpStatusCode.OK,
                        new
                        {
                            
                            Profile.Name,
                            Profile.Email,
                            Profile.Contact,
                            Profile.Address,
                        }));
 

        }
        [HttpPut]
        [Route("api/User/EditProfile")]
        public HttpResponseMessage EditProfile(int id=0,int age=0,String email=null,String contact = null,String address = null)
        {
            if (id == 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Put Id");

            }
            if (age == 0 && string.IsNullOrEmpty(email) && string.IsNullOrEmpty(contact) && string.IsNullOrEmpty(address)){
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Atleast One Field is required to update");

            }
            var user = db.User.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, "Not Found User of this id");
            }

            bool isupdated = false;
            if (age != 0)
            {
                if (age < 15 || age > 100)
                {
                    return Request.CreateResponse(HttpStatusCode.Conflict, "Age Must Be Between 15 to 100");
                }
                if (user.Age != age) { 
                user.Age = age;
                isupdated = true;

                    }
            }
            if (!string.IsNullOrEmpty(email))
            {
                bool echeck = db.User.Any(u => u.Email == email&& user.UserId!=id);
                if (echeck)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Email ALready Exist");
                }
                if (user.Email != email)
                {
                    user.Email = email;
                    isupdated = true;
                }

            }

                if (!string.IsNullOrEmpty(contact))
                {
                if (user.Contact != contact)
                {
                    user.Contact = contact;
                    isupdated = true;
                }
                }
                if (!string.IsNullOrEmpty(address))
                {
                if (user.Address != address)
                {
                    user.Address = address;
                    isupdated = true;
                }
                }
                if (isupdated)
                {
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, "Profile Updated successfully");
                }

            return Request.CreateResponse(HttpStatusCode.OK, "No Need To Update");

            
        }
       

    }

}