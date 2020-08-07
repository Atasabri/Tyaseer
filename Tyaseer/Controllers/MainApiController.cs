using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.IO;
using Tyaseer.Models;
using System.Data.Entity;

#region Author
/*
    *[Auth : Ata Sabri Ata Ahmed
    * Version : V_001
    * Info : Api Controller For Manage All Requests & Responses From Tyaseer APP 
    * E-Mail : ata.sabry@rooyadev.com]
 */
#endregion

namespace Tyaseer.Controllers
{
    public class MainApiController : ApiController
    {
        // DataBase Context 
        DB db = new DB();


        //Get Cities Data
        [HttpGet]
        public IHttpActionResult Cities()
        {
            var cities = db.Cities.OrderByDescending(x => x.ID).Select(x => new
            {
                x.ID,
                x.CityName,
                Photos=Methods.Domain+ "/Uploads/Cities/" + x.ID+".jpg"
            });
            return Ok(cities);
        }
        //Get Categories For Products
        [HttpGet]
        public IHttpActionResult ProductCategories()
        {
            var categories = db.Categories.OrderByDescending(x => x.ID).Where(x => !x.OffersOnly).Select(x => new
            {
                x.ID,
                x.Name
            });
            return Ok(categories);
        }
        //Get Offers Categories
        [HttpGet]
        public IHttpActionResult OfferCategories()
        {
            var categories = db.Categories.OrderByDescending(x => x.ID).Select(x => new
            {
                x.ID,
                x.Name
            });
            return Ok(categories);
        }
        //Get All Types
        [HttpGet]
        public IHttpActionResult Types()
        {
            var types = db.Types.OrderByDescending(x => x.ID).Select(x => new
            {
                x.ID,
                x.TypeName
            });
            return Ok(types);
        }
        //Get Single Product Data
        [HttpGet]
        public IHttpActionResult SingleProduct(int Product_ID)
        {
            var product = db.Products.Where(x=>x.ID==Product_ID).Select(x => new
            {
                x.ID,
                x.Name,
                x.Price,
                x.Provider_ID,
                x.Lat,
                x.Log,
                x.Active,
                x.NumberOfUsers,
                Rate = x.Product_Rates.Where(r => r.Rate.HasValue).Count() > 0 ? x.Product_Rates.Where(r => r.Rate.HasValue).Average(r => r.Rate) : 0,
                x.Description,
                Data = x.Product_Data.Select(d =>new { d.ID,d.Item,d.Value}),
                Types = x.Product_Types.Select(t =>new {t.ID,t.Type.TypeName }),
                NotAvaiableDates = x.Product_NotAvaiableDates.Select(s =>new { s.ID,s.Date}),
                Photos = x.Product_Photos.Select(p =>new
                {
                    p.ID,
                    Photo = Methods.Domain + "/Uploads/Products/" + p.ID+ ".jpg" 
                }),
                CatName=x.Category.Name
            }).SingleOrDefault();
            return Ok(product);
        }
        //Subscribe
        [HttpPost]
        public IHttpActionResult Subscribe()
        {
            string Email = HttpContext.Current.Request.Form["Email"];
            db.Subscribers.Add(new Subscriber { Email=Email});
            db.SaveChanges();
            return Ok(new {key=true,Message="تم المتابعة بنجاح" });
        }
    }
}
