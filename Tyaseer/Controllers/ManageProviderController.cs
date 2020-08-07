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
    public class ManageProviderController : ApiController
    {
        // DataBase Context 
        DB db = new DB();

        // ___________________________________ Authentication && Profile __________________________

        [HttpPost]
        public IHttpActionResult Register()
        {
            #region Params
            string Name = HttpContext.Current.Request.Form["Name"];
            string Email = HttpContext.Current.Request.Form["Email"];
            string Password = HttpContext.Current.Request.Form["Password"];
            string Phone = HttpContext.Current.Request.Form["Phone"];
            string TradeName = HttpContext.Current.Request.Form["TradeName"];
            string FCM = HttpContext.Current.Request.Form["FCM"];

            string Photo = HttpContext.Current.Request.Form["Photo"];
            string Photo_Identity = HttpContext.Current.Request.Form["Photo_Identity"];
            string Photo_Licence = HttpContext.Current.Request.Form["Photo_Licence"];
            #endregion

            //Create Provider Model
            string Token = Methods.encrypt(Email);
            Provider provider = new Provider
            {
                Name = Name,
                Email = Email,
                Phone = Phone,
                Password=Password,
                TradeName=TradeName,
                FCM=FCM,
                Token=Token,
                Expire_Date=null
            };
            //Validation for Model
            Validate(provider);
            if(!ModelState.IsValid)
            {
                return Ok(new { key = false, Message = ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).FirstOrDefault() });
            }
            //Photos Validation
            if(string.IsNullOrEmpty(Photo)||string.IsNullOrEmpty(Photo_Identity))
            {
                return Ok(new { key = false, Message = "من فضلك قم بتحديد صورة !!" });
            }
            //Email Unique Validation
            if (db.Providers.Any(x => x.Email == Email))
            {
                return Ok(new { Key = false, Message = "البريد الالكتروني مستخدم بالفعل .. برجاء ادخال برد اخر" });
            }
            //Add Model To DB
            db.Providers.Add(provider);
            db.SaveChanges();
            //Add Photos 
            byte[] PhotoBase64 = Convert.FromBase64String(Photo);
            File.WriteAllBytes(HttpContext.Current.Server.MapPath("~/Uploads/Provider_Photos/" + provider.ID + ".jpg"), PhotoBase64);
            byte[] IdentityPhotoBase64 = Convert.FromBase64String(Photo_Identity);
            File.WriteAllBytes(HttpContext.Current.Server.MapPath("~/Uploads/Provider_Identity/" + provider.ID + ".jpg"), IdentityPhotoBase64);
            if(!string.IsNullOrEmpty(Photo_Licence))
            {
                byte[] LicencePhotoBase64 = Convert.FromBase64String(Photo_Licence);
                File.WriteAllBytes(HttpContext.Current.Server.MapPath("~/Uploads/Provider_Licence/" + provider.ID + ".jpg"), LicencePhotoBase64);
            }
            return Ok(new { key = true, Message = "تم انشاء حساب جديد ... شكرا لك",Data=provider });
        }
        [HttpPost]
        public IHttpActionResult Login()
        {
            string Email = HttpContext.Current.Request.Form["Email"];
            string Password = HttpContext.Current.Request.Form["Password"];
            string FCM = HttpContext.Current.Request.Form["FCM"];

            var provider = db.Providers.Where(x => x.Email == Email && x.Password == Password).Select(x => new
            {
                x.ID,
                x.Token,
                x.Name,
                x.TradeName,
                x.Phone,
                x.Expire_Date,
                Photo = Methods.Domain + "/Uploads/Provider_Photos/"+x.ID+".jpg",
                IdentityPhoto = Methods.Domain + "/Uploads/Provider_Identity/" + x.ID + ".jpg",
                LicencePhoto = Methods.Domain + "/Uploads/Provider_Licence/" + x.ID + ".jpg",
            }).SingleOrDefault();
            if(provider==null)
            {
                return Ok(new { key = false, Message = "يرجاء ادخال البيانات بشكل صحيح لتتمكن لدخول " });
            }
            db.EditProviderFCM(provider.ID, FCM);
            db.SaveChanges();
            return Ok(new { key = true, Message = "تم الدخول بنجاح .... شكرا لك",Data=provider });
        }
        [HttpPut]
        public IHttpActionResult EditProfile()
        {
            #region Params
            string Token = HttpContext.Current.Request.Form["Token"];
            string Name = HttpContext.Current.Request.Form["Name"];
            string Email = HttpContext.Current.Request.Form["Email"];
            string Phone = HttpContext.Current.Request.Form["Phone"];
            string TradeName = HttpContext.Current.Request.Form["TradeName"];

            string Photo = HttpContext.Current.Request.Form["Photo"];
            string Photo_Identity = HttpContext.Current.Request.Form["Photo_Identity"];
            string Photo_Licence = HttpContext.Current.Request.Form["Photo_Licence"];
            #endregion
            Provider provider = db.Providers.SingleOrDefault(x=>x.Token==Token);
            if (!string.IsNullOrEmpty(Name))
                provider.Name = Name;
            if (!string.IsNullOrEmpty(Email))
            {
                if(db.Providers.Any(x=>x.ID!=provider.ID&&x.Email==Email))
                {
                    return Ok(new { key = false, Message = "البريد الالكتروني مستخدم بالفعل !!" });
                }
                provider.Email = Email;
                provider.Token = Methods.encrypt(Email);
            }
                
            if (!string.IsNullOrEmpty(Phone))
                provider.Phone = Phone;
            if (!string.IsNullOrEmpty(TradeName))
                provider.TradeName = TradeName;

            //Validation for Model
            Validate(provider);
            if (!ModelState.IsValid)
            {
                return Ok(new { key = false, Message = ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).FirstOrDefault() });
            }
            if (!string.IsNullOrEmpty(Photo))
            {
                byte[] PhotoBase64 = Convert.FromBase64String(Photo);
                File.WriteAllBytes(HttpContext.Current.Server.MapPath("~/Uploads/Provider_Photos/" + provider.ID + ".jpg"), PhotoBase64);
            }
            if (!string.IsNullOrEmpty(Photo_Identity))
            {
                byte[] IdentityPhotoBase64 = Convert.FromBase64String(Photo_Identity);
                File.WriteAllBytes(HttpContext.Current.Server.MapPath("~/Uploads/Provider_Identity/" + provider.ID + ".jpg"), IdentityPhotoBase64);
            }
            if (!string.IsNullOrEmpty(Photo_Licence))
            {
                byte[] LicencePhotoBase64 = Convert.FromBase64String(Photo_Licence);
                File.WriteAllBytes(HttpContext.Current.Server.MapPath("~/Uploads/Provider_Licence/" + provider.ID + ".jpg"), LicencePhotoBase64);
            }
            db.Entry(provider).State = EntityState.Modified;
            db.SaveChanges();
            return Ok(new { key=true,Message="تم تعديل الملف الشخصي بنجاح .. شكرا لك",Data=provider });
        }
        [HttpPut]
        public IHttpActionResult ChangePassword()
        {
            string Token = HttpContext.Current.Request.Form["Token"];
            string CurrentPassword = HttpContext.Current.Request.Form["CurrentPassword"];
            string NewPassword = HttpContext.Current.Request.Form["NewPassword"];

            //Find Provider
            Provider provider = db.Providers.SingleOrDefault(x=>x.Token==Token&&x.Password==CurrentPassword);
            if(provider==null)
            {
                return Ok(new {key=false,Message="مقدم الخدمة غير موجود !!" });
            }
            //Change Password To New Password And Pass To DB
            provider.Password = NewPassword;
            db.Entry(provider).State = EntityState.Modified;
            db.SaveChanges();
            return Ok(new {key=true,Message="تم تعديل كلمة المرور بنجاح" });
        }
        [HttpPost]
        public IHttpActionResult ForgetPassword()
        {
            string Email = HttpContext.Current.Request.Form["Email"];
            //Get Provider Using Email
            Provider provider = db.Providers.SingleOrDefault(x => x.Email == Email);
            string Password = provider.Password;
            //Send Mail To Provider With Password
            Methods.Send_Password(Password, Email);
            return Ok(new {key=true,Message="تم ارسال كلمة المرور الي البريد الالكتروني" });
        }
        [HttpPost]
        public IHttpActionResult MyProfile()
        {
            string Token = HttpContext.Current.Request.Form["Token"];
            var provider = db.Providers.Where(x => x.Token == Token).Select(x => new
            {
                x.ID,
                x.Name,
                x.TradeName,
                x.Email,
                x.Phone,
                x.Expire_Date,
                Rate=x.Provider_Rates.Where(r => r.Rate.HasValue).Count()>0? x.Provider_Rates.Where(r=>r.Rate.HasValue).Average(r=>r.Rate):0,
                Photo=Methods.Domain+ "/Uploads/Provider_Photos/" + x.ID+".jpg",
                IdentityPhoto = Methods.Domain + "/Uploads/Provider_Identity/" + x.ID + ".jpg",
                LicencePhoto = Methods.Domain + "/Uploads/Provider_Licence/" + x.ID + ".jpg",
            }).SingleOrDefault();
            return Ok(provider);
        }




        // ___________________________________ Products && Offers Of Provider _____________________
        [HttpPost]
        public IHttpActionResult GetProviderProducts(int Cat_ID)
        {
            string Token = HttpContext.Current.Request.Form["Token"];
            var products = db.Products.Where(x => x.Cat_ID == Cat_ID && x.Provider.Token == Token && !x.IsOffer).OrderByDescending(x=>x.ID).Select(x => new
            {
                x.ID,
                x.Name,
                x.Price,
                x.Lat,
                x.Log,
                x.Active,
                x.NumberOfUsers,
                Rate = x.Product_Rates.Where(r => r.Rate.HasValue).Count() > 0 ? x.Product_Rates.Where(r => r.Rate.HasValue).Average(r => r.Rate) : 0,
                x.Description,
                Photo = x.Product_Photos.Count > 0 ? Methods.Domain + "/Uploads/Products/" + x.Product_Photos.FirstOrDefault().ID + ".jpg" : null
            });
            return Ok(products);
        }
        [HttpPost]
        public IHttpActionResult GetProviderOffers(int Cat_ID)
        {
            string Token = HttpContext.Current.Request.Form["Token"];
            var products = db.Products.Where(x => x.Cat_ID == Cat_ID && x.Provider.Token == Token && x.IsOffer).OrderByDescending(x => x.ID).Select(x => new
            {
                x.ID,
                x.Name,
                x.Price,
                x.Lat,
                x.Log,
                x.Active,
                Rate = x.Product_Rates.Where(r => r.Rate.HasValue).Count() > 0 ? x.Product_Rates.Where(r => r.Rate.HasValue).Average(r => r.Rate) : 0,
                x.NumberOfUsers,
                x.Description,
                Photo = x.Product_Photos.Count > 0 ? Methods.Domain + "/Uploads/Products/" + x.Product_Photos.FirstOrDefault().ID + ".jpg" : null
            });
            return Ok(products);
        }
        [HttpPost]
        public IHttpActionResult AddProduct()
        {
            #region Params
            string Token = HttpContext.Current.Request.Form["Token"];
            string Name = HttpContext.Current.Request.Form["Name"];
            double Price =Convert.ToDouble(HttpContext.Current.Request.Form["Price"]);
            double Lat = Convert.ToDouble(HttpContext.Current.Request.Form["Lat"]);
            double Log = Convert.ToDouble(HttpContext.Current.Request.Form["Log"]);
            string Description = HttpContext.Current.Request.Form["Description"];
            int NumberOfVisitors =int.Parse(HttpContext.Current.Request.Form["NumberOfVisitors"]);
            int Cat_ID =int.Parse(HttpContext.Current.Request.Form["Cat_ID"]);
            int City_ID =int.Parse(HttpContext.Current.Request.Form["City_ID"]);
            bool IsOffer = Convert.ToBoolean(HttpContext.Current.Request.Form["IsOffer"]);
            bool Active = Convert.ToBoolean(HttpContext.Current.Request.Form["Active"]);

            string[] DataKeys = HttpContext.Current.Request.Form.GetValues("DataKeys");
            string[] DataValues = HttpContext.Current.Request.Form.GetValues("DataValues");
            string[] Types = HttpContext.Current.Request.Form.GetValues("Types");
            string[] NotAviableDates = HttpContext.Current.Request.Form.GetValues("NotAviableDates");
            string[] Photos = HttpContext.Current.Request.Form.GetValues("Photos");
            #endregion

            //Find Provider
            Provider provider = db.Providers.SingleOrDefault(x => x.Token == Token);
            if(provider==null)
            {
                return Ok(new {key=false,Message="هذا المقدم غير موجود" });
            }
            //Initialise Product
            Product product = new Product
            {
                Name=Name,
                Price=Price,
                Lat=Lat,
                Log=Log,
                Description=Description,
                NumberOfUsers=NumberOfVisitors,
                Active=Active,
                IsOffer=IsOffer,
                Cat_ID=Cat_ID,
                City_ID=City_ID,
                Provider_ID=provider.ID
            };
            //Validation for Model
            Validate(product);
            if (!ModelState.IsValid)
            {
                return Ok(new { key = false, Message = ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).FirstOrDefault() });
            }
            //Add Product Types
            if(Types!=null)
            {
                foreach (var item in Types)
                {
                    product.Product_Types.Add(new Product_Types { Type_ID = int.Parse(item) });
                }
            }          
            //Add product Not Aviable Dates
            if(NotAviableDates!=null)
            {
                foreach (var item in NotAviableDates)
                {
                    product.Product_NotAvaiableDates.Add(new Product_NotAvaiableDates { Date = Convert.ToDateTime(item) });
                }
            }            
            //Add Product Data
            if(DataKeys!=null)
            {
                for (int i = 0; i < DataKeys.Length; i++)
                {
                    product.Product_Data.Add(new Product_Data { Item = DataKeys[i], Value = DataValues[i] });
                }
            }
            
            //Add Product Photos
            if(Photos!=null)
            {
                foreach (var item in Photos)
                {
                    product.Product_Photos.Add(new Product_Photos { });
                }
            }
           
            //Add All Product Data To DB
            db.Products.Add(product);
            db.SaveChanges();
            //Add Photos To Server
            if(Photos!=null)
            {
                for (int i = 0; i < Photos.Length; i++)
                {
                    byte[] PhotoBase64 = Convert.FromBase64String(Photos[i]);
                    File.WriteAllBytes(HttpContext.Current.Server.MapPath("~/Uploads/Products/" + product.Product_Photos.ElementAt(i).ID + ".jpg"), PhotoBase64);
                }
            }            
            return Ok(new {key=true,Message="تم اضافة المنتج بنجاح" });
        }
        [HttpPut]
        public IHttpActionResult EditProduct(int ID)
        {
            #region Params
            string Token = HttpContext.Current.Request.Form["Token"];
            string Name = HttpContext.Current.Request.Form["Name"];
            string Price = HttpContext.Current.Request.Form["Price"];
            string Lat = HttpContext.Current.Request.Form["Lat"];
            string Log = HttpContext.Current.Request.Form["Log"];
            string Description = HttpContext.Current.Request.Form["Description"];
            string NumberOfVisitors = HttpContext.Current.Request.Form["NumberOfVisitors"];
            string Cat_ID = HttpContext.Current.Request.Form["Cat_ID"];
            string City_ID = HttpContext.Current.Request.Form["City_ID"];
            string IsOffer = HttpContext.Current.Request.Form["IsOffer"];
            string Active = HttpContext.Current.Request.Form["Active"];
            #endregion

            Product product = db.Products.SingleOrDefault(x=>x.ID==ID&&x.Provider.Token==Token);
            if(product==null)
            {
                return Ok(new {key=false,Message="هذا المنتج غير موجود !!" });
            }
            if (!string.IsNullOrEmpty(Name))
                product.Name = Name;
            if (!string.IsNullOrEmpty(Price))
                product.Price = Convert.ToDouble(Price);
            if (!string.IsNullOrEmpty(Lat))
                product.Lat = Convert.ToDouble(Lat);
            if (!string.IsNullOrEmpty(Log))
                product.Log = Convert.ToDouble(Log);
            if (!string.IsNullOrEmpty(Description))
                product.Description = Description;
            if (!string.IsNullOrEmpty(NumberOfVisitors))
                product.NumberOfUsers = int.Parse(NumberOfVisitors);
            if (!string.IsNullOrEmpty(Cat_ID))
                product.Cat_ID = int.Parse(Cat_ID);
            if (!string.IsNullOrEmpty(City_ID))
                product.City_ID = int.Parse(City_ID);
            if (!string.IsNullOrEmpty(IsOffer))
                product.IsOffer = Convert.ToBoolean(IsOffer);
            if (!string.IsNullOrEmpty(Active))
                product.Active = Convert.ToBoolean(Active);

            //Validation for Model
            Validate(product);
            if (!ModelState.IsValid)
            {
                return Ok(new { key = false, Message = ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).FirstOrDefault() });
            }
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
            return Ok(new {key=true,Message="تم تعديل المنتج بنجاح " });

        }
        [HttpPost]
        public IHttpActionResult DeleteProduct(int ID)
        {
            string Token = HttpContext.Current.Request.Form["Token"];
            //Find Product
            Product product = db.Products.SingleOrDefault(x => x.ID == ID && x.Provider.Token == Token);
            if (product == null)
            {
                return Ok(new { key = false, Message = "هذا المنتج غير موجود !!" });
            }
            //Delete Photos From Server
            foreach (var item in product.Product_Photos)
            {
                FileInfo F = new FileInfo(HttpContext.Current.Server.MapPath("~/Uploads/Products/" + item.ID + ".jpg"));
                if (F.Exists)
                {
                    F.Delete();
                }
            }
            //Delete Product From DB
            db.Products.Remove(product);
            db.SaveChanges();

            return Ok(new {key=true,Message="تم حذف المنتج بنجاح" });
        }
        //Photos
        [HttpPost]
        public IHttpActionResult AddPhoto(int Product_ID)
        {
            string Token = HttpContext.Current.Request.Form["Token"];
            string Photo = HttpContext.Current.Request.Form["Photo"];

            //Find Product
            Product product = db.Products.SingleOrDefault(x => x.ID == Product_ID && x.Provider.Token == Token);
            if(product==null)
            {
                return Ok(new {key=false,Message="هذا المنتج غير موجود" });
            }
            //Check Photo Is Found
            if(string.IsNullOrEmpty(Photo))
            {
                return Ok(new { key = false, Message = "من فضلك قم باختيار صوورة" });
            }
            //Add Photo To DB
            Product_Photos photo = new Product_Photos {Product_ID=product.ID };
            db.Product_Photos.Add(photo);
            db.SaveChanges();
            //Save Photo
            byte[] PhotoBase64 = Convert.FromBase64String(Photo);
            File.WriteAllBytes(HttpContext.Current.Server.MapPath("~/Uploads/Products/" + photo.ID + ".jpg"), PhotoBase64);
            return Ok(new { key = true, Message = "تم اضافة الصورة بنجاح" });
        }
        [HttpPost]
        public IHttpActionResult DeletePhoto(int ID)
        {
            string Token = HttpContext.Current.Request.Form["Token"];
            //Find Photo
            Product_Photos photo = db.Product_Photos.SingleOrDefault(x=>x.Product.Provider.Token==Token&&x.ID==ID);
            if(photo==null)
            {
                return Ok(new {key=false,Message="هذه الصورة غير موجودة" });
            }
            //Delete Photo From DB
            db.Product_Photos.Remove(photo);
            db.SaveChanges();
            FileInfo F = new FileInfo(HttpContext.Current.Server.MapPath("~/Uploads/Products/" + ID + ".jpg"));
            if (F.Exists)
            {
                F.Delete();
            }
            return Ok(new { key = true, Message = "تم مسح الصورة بنجاح" });
        }
        //Data
        [HttpPost]
        public IHttpActionResult AddProductData(int Product_ID)
        {
            string Token = HttpContext.Current.Request.Form["Token"];
            string Key = HttpContext.Current.Request.Form["Key"];
            string Value = HttpContext.Current.Request.Form["Value"];

            //Find Product
            Product product = db.Products.SingleOrDefault(x => x.ID == Product_ID && x.Provider.Token == Token);
            if(product==null)
            {
                return Ok(new { key = false, Message = "هذا المنتج غير موجود" });
            }
            //Add Data To DB
            Product_Data data = new Product_Data { Item = Key, Value = Value,Product_ID=product.ID };
            db.Product_Data.Add(data);
            db.SaveChanges();
            return Ok(new { key = true, Message = "تم اضافة العنصر بنجاح" });
        }
        [HttpPost]
        public IHttpActionResult DeleteProductData(int ID)
        {
            string Token = HttpContext.Current.Request.Form["Token"];
            //Find Data
            Product_Data data = db.Product_Data.SingleOrDefault(x => x.Product.Provider.Token == Token && x.ID == ID);
            if (data == null)
            {
                return Ok(new { key = false, Message = "هذه البيانات غير موجودة" });
            }
            //Delete Data From DB
            db.Product_Data.Remove(data);
            db.SaveChanges();
            return Ok(new { key = true, Message = "تم مسح العنصر بنجاح" });
        }
        //Types
        [HttpPost]
        public IHttpActionResult AddProductType(int Product_ID)
        {
            string Token = HttpContext.Current.Request.Form["Token"];
            int Type_ID =int.Parse(HttpContext.Current.Request.Form["Type_ID"]);

            //Find Product
            Product product = db.Products.SingleOrDefault(x => x.ID == Product_ID && x.Provider.Token == Token);
            if (product == null)
            {
                return Ok(new { key = false, Message = "هذا المنتج غير موجود" });
            }
            //Add Type To DB
            Product_Types type = new Product_Types { Type_ID=Type_ID,Product_ID=product.ID };
            db.Product_Types.Add(type);
            db.SaveChanges();
            return Ok(new { key = true, Message = "تم اضافة النوع بنجاح" });
        }
        [HttpPost]
        public IHttpActionResult DeleteProductType(int ID)
        {
            string Token = HttpContext.Current.Request.Form["Token"];
            //Find Type
            Product_Types type = db.Product_Types.SingleOrDefault(x => x.Product.Provider.Token == Token && x.ID == ID);
            if (type == null)
            {
                return Ok(new { key = false, Message = "هذا النوع غير موجودة" });
            }
            //Delete Type From DB
            db.Product_Types.Remove(type);
            db.SaveChanges();
            return Ok(new { key = true, Message = "تم مسح النوع بنجاح" });
        }
        //Not Avaiable Dates
        [HttpPost]
        public IHttpActionResult AddProductDate(int Product_ID)
        {
            string Token = HttpContext.Current.Request.Form["Token"];
            DateTime Date =Convert.ToDateTime(HttpContext.Current.Request.Form["Date"]);

            //Find Product
            Product product = db.Products.SingleOrDefault(x => x.ID == Product_ID && x.Provider.Token == Token);
            if (product == null)
            {
                return Ok(new { key = false, Message = "هذا المنتج غير موجود" });
            }
            //Add Date To DB
            Product_NotAvaiableDates notaviabledate = new Product_NotAvaiableDates { Date=Date, Product_ID = product.ID };
            db.Product_NotAvaiableDates.Add(notaviabledate);
            db.SaveChanges();
            return Ok(new { key = true, Message = "تم اضافة التاريخ بنجاح" });
        }
        [HttpPost]
        public IHttpActionResult DeleteProductDate(int ID)
        {
            string Token = HttpContext.Current.Request.Form["Token"];
            //Find Date
            Product_NotAvaiableDates date = db.Product_NotAvaiableDates.SingleOrDefault(x => x.Product.Provider.Token == Token && x.ID == ID);
            if (date == null)
            {
                return Ok(new { key = false, Message = "هذا التاريخ غير موجودة" });
            }
            //Delete Date From DB
            db.Product_NotAvaiableDates.Remove(date);
            db.SaveChanges();
            return Ok(new { key = true, Message = "تم مسح التاريخ بنجاح" });
        }


        // ___________________________________ Orders Of Provider _________________________________
        [HttpPost]
        public IHttpActionResult Orders()
        {
            string Token = HttpContext.Current.Request.Form["Token"];
            //Get Provider Data
            Provider provider = db.Providers.SingleOrDefault(x => x.Token == Token);
            if (provider == null)
            {
                return Ok(new { key = false, Message = "هذا المقدم غير موجود" });
            }
            //Get Orders
            var orders = provider.Products.SelectMany(x => x.Order_Details.OrderByDescending(y => y.ID).Select(y => new
            {
                 y.ID,
                 ProductName=y.Product.Name,
                 y.Price,
                 y.Count,
                 y.Order.Date,
                 UserName=y.Order.User.Name,
                 UserEmail = y.Order.User.Email,
                 UserPhone = y.Order.User.Phone,
                 Name=y.Order.Name,
                Address = y.Order.Address,
                Phone = y.Order.Phone,
                Email = y.Order.Email,
                y.Accepted,
                y.DateNeeded,
                y.Discount,
                y.FinalPrice
            }));
            return Ok(orders);
        }
        [HttpPost]
        public IHttpActionResult AcceptOrder(int Order_ID)
        {
            string Token = HttpContext.Current.Request.Form["Token"];

            Order_Details orderdetails = db.Order_Details.Where(x => x.ID == Order_ID && x.Product.Provider.Token == Token).SingleOrDefault();
            //Get Order Details 
            if (orderdetails==null)
            {
                return Ok(new { key = false, Message = "هذا الطلب غير موجود" });
            }
            //Make Order Is Accepted And Add To DB
            orderdetails.Accepted = true;
            db.Entry(orderdetails).State = EntityState.Modified;
            db.SaveChanges();
            //Send Notification To User
            string UserFCM = orderdetails.Order.User.FCM;
            Methods.Notify(new string[]{ UserFCM}, "وافق احد مقدمي الخدمات علي طلب لك برجاء المتابعة مع مقدم الخدمة", "تم الموافقة علي طلبك");
            return Ok(new { key = true, Message = "تم الموافقة علي الطلب" });
        }

    }
}
