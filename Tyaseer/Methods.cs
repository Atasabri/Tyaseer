using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using Nexmo.Api;
using Tyaseer.Models;
using System.Web.Script.Serialization;

namespace Tyaseer
{
    public class Methods
    {
        // DataBase Context 
        static DB db = new DB();

        public static string CipherKey = ConfigurationManager.AppSettings["Cipher"].ToString();
        public static string Domain = ConfigurationManager.AppSettings["Domain"].ToString();
        public static string ApiKey = ConfigurationManager.AppSettings["ApiKey"].ToString();
        public static string ApiSecret = ConfigurationManager.AppSettings["ApiSecret"].ToString();

        //__________________________________________ Encryption ___________________________________
        /// <summary>
        /// Encrypt A string And Retuen Encryption String
        /// </summary>
        /// <param name="encryptString"></param>
        /// <returns></returns>
        public static string encrypt(string encryptString)
        {
            string EncryptionKey = CipherKey;
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
            });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptString = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptString;
        }

        //__________________________________________ Send Mail && Password Confirmation ___________
        /// <summary>
        /// Send Html E-Mail To Muli-Users
        /// </summary>
        /// <param name="Subject"></param>
        /// <param name="file"></param>
        /// <param name="To"></param>
        public static void Send_Mail(string Subject, HttpPostedFileBase file, List<string> To)
        {
            string From = ConfigurationManager.AppSettings["Email"].ToString();
            string Pass = ConfigurationManager.AppSettings["Password"].ToString();
            string Host = ConfigurationManager.AppSettings["Host"].ToString();
            int Port = int.Parse(ConfigurationManager.AppSettings["Port"].ToString());
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(From);
            foreach (var item in To)
            {
                if (item.Contains("@"))
                {
                    mail.To.Add(item);
                }
            }
            mail.Subject = Subject;
            StreamReader read = new StreamReader(file.InputStream);
            mail.Body = read.ReadToEnd();
            mail.IsBodyHtml = true;
            ///-------------------------------------------------------------------------//
            SmtpClient smtpMail = new SmtpClient();
            smtpMail.EnableSsl = false;
            smtpMail.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpMail.Host = Host;
            smtpMail.Port = Port;

            smtpMail.UseDefaultCredentials = false;
            smtpMail.Credentials = new NetworkCredential(From, Pass);
            ///-------------------------------------------------------------------------//
            smtpMail.Send(mail);
        }
        /// <summary>
        /// Send Password To E-Mail
        /// </summary>
        /// <param name="Subject"></param>
        /// <param name="file"></param>
        /// <param name="To"></param>
        public static void Send_Password(string UserPass, string To)
        {
            string From = ConfigurationManager.AppSettings["Email"].ToString();
            string Pass = ConfigurationManager.AppSettings["Password"].ToString();
            string Host = ConfigurationManager.AppSettings["Host"].ToString();
            int Port = int.Parse(ConfigurationManager.AppSettings["Port"].ToString());
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(From);
            mail.To.Add(To);
            mail.Subject = "Tyaseer Confirmation Password";

            mail.Body = "Hi , Your Password Is : " + UserPass;
            ///-------------------------------------------------------------------------//
            SmtpClient smtpMail = new SmtpClient();
            smtpMail.EnableSsl = false;
            smtpMail.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpMail.Host = Host;
            smtpMail.Port = Port;

            smtpMail.UseDefaultCredentials = false;
            smtpMail.Credentials = new NetworkCredential(From, Pass);
            ///-------------------------------------------------------------------------//
            smtpMail.Send(mail);
        }


        //_________________________________________ Get Random ____________________________________
        /// <summary>
        /// This Function For Return Random Number From 0 to 999999
        /// </summary>
        /// <returns></returns>
        public static int GetRandom()
        {
            Random random = new Random();
            return random.Next(999999);
        }


        //__________________________________________ Social Media Login ____________________________
        /// <summary>
        /// Find User That Register Using FaceBook
        /// </summary>
        /// <param name="FaceBook_ID"></param>
        /// <returns></returns>
        public static object FindFaceBookUser(string FaceBook_ID)
        {
            var facebookuser = db.Users.Where(x => x.Facebook_ID == FaceBook_ID).Select(x => new
            {
                x.ID,
                x.Name,
                x.Token,
                x.Email,
                x.Facebook_ID,
                x.Phone,
                Photo=Domain+"/Uploads/Users/"+x.ID+".jpg"
            }).FirstOrDefault();

            return facebookuser;
        }

        /// <summary>
        /// Find User That Register Using Twitter
        /// </summary>
        /// <param name="Twitter_ID"></param>
        /// <returns></returns>
        public static object FindTwitterUser(string Twitter_ID)
        {
            var facebookuser = db.Users.Where(x => x.Facebook_ID == Twitter_ID).Select(x => new
            {
                x.ID,
                x.Name,
                x.Token,
                x.Email,
                x.Facebook_ID,
                x.Phone,
                Photo = Domain + "/Uploads/Users/" + x.ID + ".jpg"
            }).FirstOrDefault();

            return facebookuser;
        }

        /// <summary>
        /// Find User That Register Using Google
        /// </summary>
        /// <param name="Google_ID"></param>
        /// <returns></returns>
        public static object FindGoogleUser(string Google_ID)
        {
            var facebookuser = db.Users.Where(x => x.Facebook_ID == Google_ID).Select(x => new
            {
                x.ID,
                x.Name,
                x.Token,
                x.Email,
                x.Facebook_ID,
                x.Phone,
                Photo = Domain + "/Uploads/Users/" + x.ID + ".jpg"
            }).FirstOrDefault();

            return facebookuser;
        }


        //___________________________________________ Manage Final Price ___________________________
        /// <summary>
        /// This Function For Return The Price After Discount
        /// </summary>
        /// <param name="Price"></param>
        /// <param name="Discount"></param>
        /// <returns></returns>
        public static double GetPriceAfterDiscount(double Price, double? Discount)
        {
            if (Discount.HasValue)
            {
                double DiscountPrice = (Discount.Value * Price) / 100;
                Price = Price - DiscountPrice;
            }
            return Price;
        }


        //___________________________________________ Notifications _________________________________
        /// <summary>
        /// This Function For Send Notifications For FCM Mobile Users
        /// </summary>
        /// <param name="device"></param>
        /// <param name="body"></param>
        /// <param name="title"></param>
        public static void Notify(string[] devices, string body, string title)
        {
            string SERVER_API_KEY = ConfigurationManager.AppSettings["SERVER_API_KEY"];
            var SENDER_ID = ConfigurationManager.AppSettings["SENDER_ID"];
            try
            {
                var applicationID = SERVER_API_KEY;
                var senderId = SENDER_ID;
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var data = new
                {
                    registration_ids = devices,
                    notification = new
                    {
                        body = body,
                        title = title,
                        icon = "http://traveler-apps.com/images/favicon.png"
                    },
                    priority = "high"

                };

                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(data);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                tRequest.ContentLength = byteArray.Length;

                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);

                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }


        //____________________________________________ Phone Verify ________________________________
        /// <summary>
        /// Send Verfiy Code To Phone
        /// </summary>
        /// <param name="Phone"></param>
        /// <returns></returns>
        public static string SendVerifySMS(string Phone)
        {
            var client = new Client(creds: new Nexmo.Api.Request.Credentials
            {
                ApiKey = ApiKey,
                ApiSecret = ApiSecret
            });
            var start = client.NumberVerify.Verify(new NumberVerify.VerifyRequest
            {
                number = Phone,
                brand = "Tyaseer"
            });
            return start.request_id;
        }
        /// <summary>
        /// Check Verify Code Using request_id
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="request_id"></param>
        /// <returns></returns>
        public static bool CheckVerify(string Code, string request_id)
        {
            var client = new Client(creds: new Nexmo.Api.Request.Credentials
            {
                ApiKey = ApiKey,
                ApiSecret = ApiSecret
            });
            var result = client.NumberVerify.Check(new NumberVerify.CheckRequest
            {
                code = Code,
                request_id = request_id
            });

            if (result.status == "0")
            {
                return true;
            }
            return false;
        }


        //_____________________________________________ Email Verify ________________________________
        /// <summary>
        /// Send Password To E-Mail
        /// </summary>
        /// <param name="Subject"></param>
        /// <param name="file"></param>
        /// <param name="To"></param>
        public static void Send_CodeToEmail(string Code, string To)
        {
            string From = ConfigurationManager.AppSettings["Email"].ToString();
            string Pass = ConfigurationManager.AppSettings["Password"].ToString();
            string Host = ConfigurationManager.AppSettings["Host"].ToString();
            int Port = int.Parse(ConfigurationManager.AppSettings["Port"].ToString());
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(From);
            mail.To.Add(To);
            mail.Subject = "Tyaseer Confirmation Code";

            mail.Body = "Hi , Your Confirmation Code Is : " + Code;
            ///-------------------------------------------------------------------------//
            SmtpClient smtpMail = new SmtpClient();
            smtpMail.EnableSsl = false;
            smtpMail.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpMail.Host = Host;
            smtpMail.Port = Port;

            smtpMail.UseDefaultCredentials = false;
            smtpMail.Credentials = new NetworkCredential(From, Pass);
            ///-------------------------------------------------------------------------//
            smtpMail.Send(mail);
        }
    }
}