using DAL.Model;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using homework1.Controllers;

namespace homework1.AddControllers
{
    [Route("api/User")]
    [ApiController]
    
    public class UserController : ControllerBase
    {
         List<User> users = new List<User>();
         Result result = new Result();
         DBOperations dbOperations = new DBOperations();
         SiteContext context = new SiteContext();



        /// <summary>
        /// T�m film listesini d�nd�r�r.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles ="Admin")]
        [HttpGet()]
        public List<User> GetUser()
        {
            //listeyi dolduruyor
           
           
            return dbOperations.GetUsers();

        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{userId}")]
        public User GetUser(int userId)
        {
            //listeyi dolduruyor
            users = dbOperations.GetUsers();
            User user_obj = new User();

            //kar��la�t�rma i�lemini yap�yor.
            user_obj = users.FirstOrDefault(x => x.Id == userId);
            return user_obj;

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public Result PostUser(User user)
        {
            Result result = new Result();
            User us = dbOperations.FindUser(user.Email);

            //verilen film bilgisi listede var m� diye bak�yor
            bool userCheck = (us != null) ? true : false;

            if (!userCheck)
            {
                if (dbOperations.AddUser(user) == true)
                {
                    result.status = 1;
                    result.message = "Yeni kullan�c� listeye eklendi.";
                }
                else
                {
                    result.status = 0;
                    result.message = "Hata, kullan�c� eklenemedi.";
                }
            }
            else
            {
                result.status = 0;
                result.message = "Bu eleman listede zaten var.";

            }

            return result;
        }


        [Authorize(Roles = "Admin")]
        [HttpPatch("{user_Id}")]

        public Result UpdateUser(int user_Id, User new_value)
        {

            users = dbOperations.GetUsers();

            //verilen film listede var m� diye kontrol ediyor
            User? old_value = users.Find(x => x.Id == user_Id);

            if (old_value != null)
            {
                dbOperations.UpdateUser(old_value, new_value);

                result.status = 1;
                result.message = "Kullan�c� bilgileri ba�ar�yla g�ncellendi.";


            }
            else
            {
                result.status = 0;
                result.message = "Kullan�c� bulunamad�.";
            }

            return result;



        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{user_Id}")]
        public Result DeleteUser(int user_Id)
        {

            if (dbOperations.DeleteUser(user_Id))
            {

                result.status = 1;
                result.message = "Kullan�c� silindi.";

            }
            else
            {
                result.status = 0;
                result.message = "Kullan�c� zaten silinmi�.";
            }

            return result;
        }






    }
}