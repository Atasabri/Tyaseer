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
    public class ManageUserController : ApiController
    {
        // DataBase Context 
        DB db = new DB();

        //__________________________________________ Home & Search _________________________________
        [HttpGet]
        public IHttpActionResult Home()
        {
            var Data = db.Categories.Take(3).Select(x => new
            {
                x.ID,
                x.Name,
                Products = x.Products.Where(p=>p.Active).OrderByDescending(p => p.ID).Take(6).Select(p => new
                {
                    p.ID,
                    p.Name,
                    Photo = p.Product_Photos.Count > 0 ? Methods.Domain + "/Uploads/Products/" + p.Product_Photos.FirstOrDefault().ID + ".jpg" : null
                })
            });
            return Ok(Data);
        }
        [HttpGet]
        public IHttpActionResult CitiesProducts(int City_ID)
        {
            var Data = db.Products.Where(x => x.City_ID == City_ID && !x.IsOffer && x.Active).OrderByDescending(x=>x.ID).Select(x => new
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
            return Ok(Data);
        }
        [HttpPost]
        public IHttpActionResult Search(int Cat_ID,bool IsOffer)
        {
            int City_ID =int.Parse(HttpContext.Current.Request.Form["City_ID"]);
            DateTime Date =Convert.ToDateTime(HttpContext.Current.Request.Form["Date"]);
            int Type_ID = int.Parse(HttpContext.Current.Request.Form["Type_ID"]);
            int NumberOfVisitors =int.Parse(HttpContext.Current.Request.Form["NumberOfVisitors"]);

            var AllData = db.Products.Where(x => x.City_ID == City_ID&&x.IsOffer==IsOffer && x.Cat_ID == Cat_ID && x.NumberOfUsers >= NumberOfVisitors&&x.Active);
            var Data=AllData.Where(x=>!x.Product_NotAvaiableDates.Select(d => d.Date).Contains(Date)&&x.Product_Types.Select(t=>t.Type_ID).Contains(Type_ID)).Select(x => new
            {
                x.ID,
                x.Name,
                x.Price,
                x.Lat,
                x.Log,
                x.NumberOfUsers,
                Rate = x.Product_Rates.Where(r => r.Rate.HasValue).Count() > 0 ? x.Product_Rates.Where(r => r.Rate.HasValue).Average(r => r.Rate) : 0,
                x.Description,
                Photo = x.Product_Photos.Count > 0 ? Methods.Domain + "/Uploads/Products/" + x.Product_Photos.FirstOrDefault().ID + ".jpg" : null
            });
            return Ok(Data);
        }
        [HttpGet]
        public IHttpActionResult Offers(int Cat_ID)
        {
            var Data = db.Products.Where(x =>x.Cat_ID==Cat_ID&& x.IsOffer && x.Active).OrderByDescending(x => x.ID).Select(x => new
            {
                x.ID,
                x.Name,
                x.Price,
                x.Lat,
                x.Log,
                x.NumberOfUsers,
                Rate = x.Product_Rates.Where(r => r.Rate.HasValue).Count() > 0 ? x.Product_Rates.Where(r => r.Rate.HasValue).Average(r => r.Rate) : 0,
                x.Description,
                Photo = x.Product_Photos.Count > 0 ? Methods.Domain + "/Uploads/Products/" + x.Product_Photos.FirstOrDefault().ID + ".jpg" : null
            });
            return Ok(Data);
        }
        [HttpGet]
        public IHttpActionResult ProviderData(int Provider_ID)
        {
            var provider = db.Providers.Where(x => x.ID == Provider_ID).Select(x => new
            {
                x.ID,
                x.Name,
                x.TradeName,
                x.Email,
                x.Phone,
                Rate = x.Provider_Rates.Where(r => r.Rate.HasValue).Count() > 0 ? x.Provider_Rates.Where(r => r.Rate.HasValue).Average(r => r.Rate) : 0,
                Photo = Methods.Domain + "/Uploads/Provider_Photos/" + x.ID + ".jpg",
                IdentityPhoto = Methods.Domain + "/Uploads/Provider_Identity/" + x.ID + ".jpg",
                LicencePhoto = Methods.Domain + "/Uploads/Provider_Licence/" + x.ID + ".jpg",
                Products=x.Products.Where(p=>p.Active&&!p.IsOffer).OrderByDescending(p=>p.ID).Select(p=>new {
                    p.ID,
                    p.Name,
                    p.Price,
                    p.Lat,
                    p.Log,
                    CatName=p.Category.Name,
                    p.NumberOfUsers,
                    Rate = p.Product_Rates.Where(r => r.Rate.HasValue).Count() > 0 ? p.Product_Rates.Where(r => r.Rate.HasValue).Average(r => r.Rate) : 0,
                    p.Description,
                    Photo = p.Product_Photos.Count > 0 ? Methods.Domain + "/Uploads/Products/" + p.Product_Photos.FirstOrDefault().ID + ".jpg" : null
                }),
                Offers= x.Products.Where(p => p.Active && p.IsOffer).OrderByDescending(p => p.ID).Select(p => new {
                    p.ID,
                    p.Name,
                    p.Price,
                    p.Lat,
                    p.Log,
                    CatName = p.Category.Name,
                    p.NumberOfUsers,
                    Rate = p.Product_Rates.Where(r => r.Rate.HasValue).Count() > 0 ? p.Product_Rates.Where(r => r.Rate.HasValue).Average(r => r.Rate) : 0,
                    p.Description,
                    Photo = p.Product_Photos.Count > 0 ? Methods.Domain + "/Uploads/Products/" + p.Product_Photos.FirstOrDefault().ID + ".jpg" : null
                })
            }).SingleOrDefault();
            return Ok(provider);
        }


        //__________________________________________ Authentication && Profile ______________________
        [HttpPost]
        public IHttpActionResult Register()
        {
            #region Params
            string Name = HttpContext.Current.Request.Form["Name"];
            string Email = HttpContext.Current.Request.Form["Email"];
            string Password = HttpContext.Current.Request.Form["Password"];
            string Phone = HttpContext.Current.Request.Form["Phone"];
            string Facebook_ID = HttpContext.Current.Request.Form["Facebook_ID"];
            string Twitter_ID = HttpContext.Current.Request.Form["Twitter_ID"];
            string Google_ID = HttpContext.Current.Request.Form["Google_ID"];
            string FCM = HttpContext.Current.Request.Form["FCM"];

            string Photo= HttpContext.Current.Request.Form["Photo"];
            #endregion

            //Create User Model
            User user = new User
            {
                Name=Name,
                Email=Email,
                Phone=Phone,
                FCM=FCM,
                Token=Methods.encrypt(Email)
            };
            //Social Media Check
            #region Social Media Check
            if (!string.IsNullOrEmpty(Facebook_ID))
            {
                var facebookuser = Methods.FindFaceBookUser(Facebook_ID);
                if(facebookuser!=null)
                {
                    return Ok(new {key=true,Message="تم تسجيل الدخول باستخدام الفيس بوك",Data=facebookuser });
                }
                user.Facebook_ID = Facebook_ID;
            }
            else if (!string.IsNullOrEmpty(Twitter_ID))
            {
                var twitteruser = Methods.FindTwitterUser(Twitter_ID);
                if (twitteruser != null)
                {
                    return Ok(new { key = true, Message = "تم تسجيل الدخول باستخدام  تويتر", Data = twitteruser });
                }
                user.Twitter_ID = Twitter_ID;
            }
            else if (!string.IsNullOrEmpty(Google_ID))
            {
                var googleuser = Methods.FindGoogleUser(Google_ID);
                if (googleuser != null)
                {
                    return Ok(new { key = true, Message = "تم تسجيل الدخول باستخدام  جوجل", Data = googleuser });
                }
                user.Google_ID = Google_ID;
            }
            else
            {
                user.Password = Password;
            }
            #endregion

            if(string.IsNullOrEmpty(Photo))
            {
                return Ok(new { Key = false, Message = "قم باختيار صورة" });
            }
            //Validation for Model
            Validate(user);
            if (!ModelState.IsValid)
            {
                return Ok(new { key = false, Message = ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).FirstOrDefault() });
            }
            //Email Unique Validation
            if (db.Users.Any(x => x.Email == Email))
            {
                return Ok(new { Key = false, Message = "البريد الالكتروني مستخدم بالفعل .. برجاء ادخال برد اخر" });
            }
            //Add User To DB
            db.Users.Add(user);
            db.SaveChanges();
            //Save Photo To Server
            byte[] PhotoBase64 = Convert.FromBase64String(Photo);
            File.WriteAllBytes(HttpContext.Current.Server.MapPath("~/Uploads/Users/" + user.ID + ".jpg"), PhotoBase64);

            return Ok(new { Key = true, Message = "تم انشاء الحساب بنجاح ",Data=user });
        }
        [HttpPost]
        public IHttpActionResult Login()
        {
            #region Params
            string Email = HttpContext.Current.Request.Form["Email"];
            string Password = HttpContext.Current.Request.Form["Password"];
            string Facebook_ID = HttpContext.Current.Request.Form["Facebook_ID"];
            string Twitter_ID = HttpContext.Current.Request.Form["Twitter_ID"];
            string Google_ID = HttpContext.Current.Request.Form["Google_ID"];

            string FCM = HttpContext.Current.Request.Form["FCM"];
            #endregion

            //Login in Social Media
            #region FaceBook Login
            if (!string.IsNullOrEmpty(Facebook_ID))
            {
                dynamic facebookuser = Methods.FindFaceBookUser(Facebook_ID);
                if(facebookuser==null)
                {
                    return Ok(new {key=false,Message="لا يمكن تسجيل الدخول باستخدام الفيس بوك" });
                }
                db.EditUserFCM(facebookuser.ID, FCM);
                db.SaveChanges();
                return Ok(new {key=true,Message="تم تسجيل الدخول باستخدام الفيس بوك",Data=facebookuser });
            }
            #endregion
            #region Twitter Login
            else if (!string.IsNullOrEmpty(Twitter_ID))
            {
                dynamic twitteruser = Methods.FindTwitterUser(Twitter_ID);
                if (twitteruser == null)
                {
                    return Ok(new { key = false, Message = "لا يمكن تسجيل الدخول باستخدام  تويتر" });
                }
                db.EditUserFCM(twitteruser.ID, FCM);
                db.SaveChanges();
                return Ok(new { key = true, Message = "تم تسجيل الدخول باستخدام  تويتر", Data = twitteruser });
            }
            #endregion
            #region Google Login
            else if (!string.IsNullOrEmpty(Google_ID))
            {
                dynamic googleuser = Methods.FindGoogleUser(Google_ID);
                if (googleuser == null)
                {
                    return Ok(new { key = false, Message = "لا يمكن تسجيل الدخول باستخدام  جوجل" });
                }
                db.EditUserFCM(googleuser.ID, FCM);
                db.SaveChanges();
                return Ok(new { key = true, Message = "تم تسجيل الدخول باستخدام  جوجل", Data = googleuser });
            }
            #endregion
            //Login Use Email & Password
            else
            {
                var normaluser = db.Users.Where(x => x.Email == Email && x.Password == Password).Select(x => new
                {
                    x.ID,
                    x.Name,
                    x.Token,
                    x.Email,
                    x.Phone,
                    Photo =Methods.Domain + "/Uploads/Users/" + x.ID + ".jpg"
                }).SingleOrDefault();
                if(normaluser == null)
                {
                    return Ok(new { key = false, Message = "لا يمكن تسجيل الدخول برجاء التأكد من البيانات" });
                }
                db.EditUserFCM(normaluser.ID, FCM);
                db.SaveChanges();
                return Ok(new { key = true, Message = "تم تسجيل الدخول ", Data = normaluser });
            }
        }
        [HttpPut]
        public IHttpActionResult EditProfile()
        {
            #region Params
            string Token = HttpContext.Current.Request.Form["Token"];
            string Name = HttpContext.Current.Request.Form["Name"];
            string Email = HttpContext.Current.Request.Form["Email"];
            string Phone = HttpContext.Current.Request.Form["Phone"];

            string Photo = HttpContext.Current.Request.Form["Photo"];
            #endregion

            User user = db.Users.SingleOrDefault(x=>x.Token==Token);
            if(user==null)
            {
                return Ok(new {key=false,Message="هذا المستخدم غير موجود" });
            }
            if (!string.IsNullOrEmpty(Name))
                user.Name = Name;
            //Email Check If Found
            if (!string.IsNullOrEmpty(Email))
            {
                if(db.Users.Any(x=>x.ID!=user.ID&&x.Email==Email))
                {
                    return Ok(new { key = false, Message = "هذا البريد الالكتروني  موجود بالفعل" });
                }
                user.Email = Email;
            }
            if (!string.IsNullOrEmpty(Phone))
                user.Phone = Phone;
            if (!string.IsNullOrEmpty(Photo))
            {
                //Save Photo To Server
                byte[] PhotoBase64 = Convert.FromBase64String(Photo);
                File.WriteAllBytes(HttpContext.Current.Server.MapPath("~/Uploads/Users/" + user.ID + ".jpg"), PhotoBase64);
            }
            //Check validation
            Validate(user);
            if (!ModelState.IsValid)
            {
                return Ok(new { key = false, Message = ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).FirstOrDefault() });
            }
            //Save Change To DB
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return Ok(new { key = true, Message = "تم تعديل الملف الشخصي بنجاح" });
        }
        [HttpPut]
        public IHttpActionResult ChangePassword()
        {
            string Token = HttpContext.Current.Request.Form["Token"];
            string CurrentPassword = HttpContext.Current.Request.Form["CurrentPassword"];
            string NewPassword = HttpContext.Current.Request.Form["NewPassword"];

            //Find User
            User user = db.Users.SingleOrDefault(x => x.Token == Token && x.Password == CurrentPassword);
            if (user == null)
            {
                return Ok(new { key = false, Message = "المستخدم غير موجود !!" });
            }
            //Change Password To New Password And Pass To DB
            user.Password = NewPassword;
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return Ok(new { key = true, Message = "تم تعديل كلمة المرور بنجاح" });
        }
        [HttpPost]
        public IHttpActionResult ForgetPassword()
        {
            string Email = HttpContext.Current.Request.Form["Email"];
            //Get User Using Email
            User user = db.Users.SingleOrDefault(x => x.Email == Email);
            if(string.IsNullOrEmpty(user.Password))
            {
                return Ok(new { key = false, Message = "لقد قمت بالتسجيل باستخدام السوشيال ميديا" });
            }
            string Password = user.Password;
            //Send Mail To User With Password
            Methods.Send_Password(Password, Email);
            return Ok(new { key = true, Message = "تم ارسال كلمة المرور الي البريد الالكتروني" });
        }
        [HttpPost]
        public IHttpActionResult Profile()
        {
            string Token = HttpContext.Current.Request.Form["Token"];
            var user = db.Users.Where(x=>x.Token==Token).Select(x => new
            {
                x.ID,
                x.Name,
                x.Email,
                x.Phone,
                Photo=Methods.Domain+"/Uploads/Users/"+x.ID+".jpg",
                Orders = x.Orders.Select(o =>new
                {
                    o.ID,
                    o.Date,
                    o.Name,
                    o.Address,
                    o.Phone,
                    o.Email,
                    o.TotalPrice,
                    o.FinalPrice,
                    Details= o.Order_Details.Select(d =>new
                    {
                        d.ID,
                        d.Price,
                        Product_ID= d.Product.ID,
                        Product_Name=d.Product.Name,
                        Provider_ID=d.Product.Provider_ID,
                        Provider_Name = d.Product.Provider.Name,
                        d.Accepted,
                        d.Count,
                        d.DateNeeded,
                        d.Discount,
                        d.FinalPrice
                    })
                })
            }).SingleOrDefault();
            return Ok(user);
        }



        //_____________________________________________Rates && Comments_____________________________
        [HttpPost]
        public IHttpActionResult RateProvider(int Provider_ID)
        {
            string Token = HttpContext.Current.Request.Form["Token"];
            string Rate =HttpContext.Current.Request.Form["Rate"];
            string Comment = HttpContext.Current.Request.Form["Comment"];

            User user = db.Users.SingleOrDefault(x => x.Token == Token);
            //Check If User Rate Provider Once And Edit The Rate
            #region Last Rate
            var lastrate = user.Provider_Rates.SingleOrDefault(x => x.Provider_ID == Provider_ID);
            if (lastrate!=null)
            {
                if(!string.IsNullOrEmpty(Rate))
                {
                    lastrate.Rate =Convert.ToDouble(Rate);
                }
                if (!string.IsNullOrEmpty(Comment))
                {
                    lastrate.Comment = Comment;
                }                
                db.Entry(lastrate).State = EntityState.Modified;
                db.SaveChanges();
                return Ok(new { key = true, Message = "تم تعديل تقييم هذا المقدم" });
            }
            #endregion
            //Add New Rate If No Rates
            #region New Rate
            Provider_Rates rate = new Provider_Rates { Provider_ID = Provider_ID, User_ID = user.ID };
            if (!string.IsNullOrEmpty(Rate))
            {
                rate.Rate = Convert.ToDouble(Rate);
            }
            if (!string.IsNullOrEmpty(Comment))
            {
                rate.Comment = Comment;
            }
            db.Provider_Rates.Add(rate);
            db.SaveChanges();
            return Ok(new { key = true, Message = "تم تقييم هذا المقدم " });
            #endregion
        }
        [HttpPost]
        public IHttpActionResult RateProduct(int Product_ID)
        {
            string Token = HttpContext.Current.Request.Form["Token"];
            string Rate = HttpContext.Current.Request.Form["Rate"];
            string Comment = HttpContext.Current.Request.Form["Comment"];

            User user = db.Users.SingleOrDefault(x => x.Token == Token);
            //Check If User Rate Provider Once And Edit The Rate
            #region Last Rate
            var lastrate = user.Product_Rates.SingleOrDefault(x => x.Product_ID == Product_ID);
            if (lastrate != null)
            {
                if (!string.IsNullOrEmpty(Rate))
                {
                    lastrate.Rate = Convert.ToDouble(Rate);
                }
                if (!string.IsNullOrEmpty(Comment))
                {
                    lastrate.Comment = Comment;
                }
                db.Entry(lastrate).State = EntityState.Modified;
                db.SaveChanges();
                return Ok(new { key = true, Message = "تم تعديل تقييم هذا المنتج" });
            }
            #endregion
            //Add New Rate If No Rates
            #region New Rate
            Product_Rates rate = new Product_Rates { Product_ID = Product_ID, User_ID = user.ID };
            if (!string.IsNullOrEmpty(Rate))
            {
                rate.Rate = Convert.ToDouble(Rate);
            }
            if (!string.IsNullOrEmpty(Comment))
            {
                rate.Comment = Comment;
            }
            db.Product_Rates.Add(rate);
            db.SaveChanges();
            return Ok(new { key = true, Message = "تم تقييم هذا المنتج " });
            #endregion
        }
        //Get Product Rates & Comments
        [HttpGet]
        public IHttpActionResult ProductRatesComments(int Product_ID)
        {
            var comments = db.Product_Rates.Where(x => x.Product_ID == Product_ID).Select(x => new
            {
                x.ID,
                x.Rate,
                x.Comment,
                UserName = x.User.Name,
                Photo = Methods.Domain + "/Uploads/Users/" + x.User_ID + ".jpg"
            });
            return Ok(comments);
        }
        //Get Provider Rates & Comments
        [HttpGet]
        public IHttpActionResult ProviderRatesComments(int Provider_ID)
        {
            var comments = db.Provider_Rates.Where(x => x.Provider_ID == Provider_ID).Select(x => new
            {
                x.ID,
                x.Rate,
                x.Comment,
                UserName = x.User.Name,
                Photo = Methods.Domain + "/Uploads/Users/" + x.User_ID + ".jpg"
            });
            return Ok(comments);
        }



        //______________________________________________Order && Verify___________________________________
        [HttpPost]
        public IHttpActionResult Order()
        {
            #region Params
            string Token = HttpContext.Current.Request.Form["Token"];
            string Name = HttpContext.Current.Request.Form["Name"];
            string Phone= HttpContext.Current.Request.Form["Phone"];
            string Email = HttpContext.Current.Request.Form["Email"];
            string Address = HttpContext.Current.Request.Form["Address"];
            string Code = HttpContext.Current.Request.Form["Code"];
            string[] Products_IDS= HttpContext.Current.Request.Form.GetValues("Products_IDS");
            string[] Products_Counts = HttpContext.Current.Request.Form.GetValues("Products_Counts");
            string[] Products_DatesNeeded = HttpContext.Current.Request.Form.GetValues("Products_DatesNeeded");
            #endregion

            User user = db.Users.SingleOrDefault(x => x.Token == Token);
            if (user == null)
            {
                return Ok(new { key = false, Message = "هذا المستخدم غير موجود" });
            }
            Order order = new Order
            {
                User_ID = user.ID,
                Date = DateTime.Now,
                Phone = Phone,
                Email = Email,
                Address = Address,
                Name = Name
            };
            //Check validation
            Validate(order);
            if (!ModelState.IsValid)
            {
                return Ok(new { key = false, Message = ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).FirstOrDefault() });
            }
            //Check Code Is Found And Discount Of It
            #region Check Code Validation
            if (!string.IsNullOrEmpty(Code))
            {
                Code CodeDiscount = db.Codes.SingleOrDefault(x => x.Code1 == Code);
                if (CodeDiscount != null)
                {
                    order.Discount = CodeDiscount.Discount;
                    db.Codes.Remove(CodeDiscount);
                }
                else
                {
                    return Ok(new { Key = false, Message = "كود الخصم غير صحيح" });
                }
            }
            #endregion

            //Add Order Details To Order And Manage Prices
            string[] ProvidersFCM =new string[Products_IDS.Length];
            for (int i = 0; i < Products_IDS.Length; i++)
            {
                int Product_ID = int.Parse(Products_IDS[i]);
                Product product = db.Products.Find(Product_ID);
                ProvidersFCM[i] = product.Provider.FCM;
                
                Order_Details detail = new Order_Details
                {
                    Product_ID=Product_ID,
                    Count=int.Parse(Products_Counts[i]),
                    DateNeeded=Convert.ToDateTime(Products_DatesNeeded[i]),
                    Discount=order.Discount,
                };
                //Check Date
                if (product.Product_NotAvaiableDates.Select(x => x.Date).Contains(detail.DateNeeded))
                {
                    return Ok(new {key=false,Message="التاريخ المطلوب للمنتج غير موافق للمقدم" });
                }
                //Price And Final Price Of Order Details
                detail.Price = product.Price * detail.Count;
                detail.FinalPrice = Methods.GetPriceAfterDiscount(detail.Price,order.Discount);
                //Price And Final Price Of Order 
                order.TotalPrice += detail.Price;
                order.FinalPrice += detail.FinalPrice;
                order.Order_Details.Add(detail);
            }
            //Save Data To DB
            db.Orders.Add(order);
            db.SaveChanges();
            //Send Notification
            Methods.Notify(ProvidersFCM, "لديك كلب جديد في طلباتك برجاء الموافقة والموافقة علي الطلب", "لديك طلب جديد");
            return Ok(new {key=true,Message="تم عمل الطلب بنجاح" });
        }
        [HttpPost]
        public IHttpActionResult SendCodeToPhone()
        {
            string Phone = HttpContext.Current.Request.Form["Phone"];
            return Ok(new { request_id = Methods.SendVerifySMS(Phone) });
        }
        [HttpPost]
        public IHttpActionResult CheckVerifyPhone()
        {
            try
            {
                string Code = HttpContext.Current.Request.Form["Code"];
                string request_id = HttpContext.Current.Request.Form["request_id"];
                if (Methods.CheckVerify(Code, request_id))
                {
                    return Ok(new { key = true });
                }
                return Ok(new {key=false });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IHttpActionResult SendCodeToEmail()
        {
            string Email = HttpContext.Current.Request.Form["Email"];

            string Code = Methods.GetRandom().ToString();
            var UserEmail = db.EmailVerifies.SingleOrDefault(x => x.Email == Email);
            if(UserEmail==null)
            {
                db.EmailVerifies.Add(new EmailVerify {Email=Email,Code=Code });
            }
            else
            {
                UserEmail.Code = Code;
                db.Entry(UserEmail).State = EntityState.Modified;
            }
            Methods.Send_CodeToEmail(Code, Email);
            db.SaveChanges();
            return Ok(new {key=true,Message="تم ارسال كود التأكيد " });
        }
        [HttpPost]
        public IHttpActionResult CheckVerifyEmail()
        {
            string Email = HttpContext.Current.Request.Form["Email"];
            string Code = HttpContext.Current.Request.Form["Code"];

            var UserEmail = db.EmailVerifies.SingleOrDefault(x => x.Email == Email && x.Code == Code);
            if (UserEmail!=null)
            {
                db.EmailVerifies.Remove(UserEmail);
                db.SaveChanges();
                return Ok(new { key = true });
            }
            return Ok(new {key=false });
        }

    }
}
