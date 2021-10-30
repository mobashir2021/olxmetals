using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using tauseefolx.common;
using tauseefolx.Models;

namespace tauseefolx.Controllers
{
    [System.Web.Http.RoutePrefix("api/Appapi")]
    public class AppapiController : ApiController
    {

        olxappEntities db = new olxappEntities();
        
        string tempDocUrl = WebConfigurationManager.AppSettings["DocsUrl"];

        [AllowCrossSiteJson]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage TestData()
        {
            try
            {
                
                string resvalue = "Tested Successfully";
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(resvalue));
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return response;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [AllowCrossSiteJson]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> BuyerPost()
        {
            try
            {
                var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
                //access form data  
                NameValueCollection formData = provider.FormData;
                var Sellitemid = Convert.ToInt32( formData["Sellitemid"]);
                var Buyerid = Convert.ToInt32(formData["Buyerid"]);
                var Finalprice = formData["Finalprice"];
                
                var sellobj = db.SellItems.Where(x => x.SellItemid == Sellitemid && x.Sold == "Not Sold").FirstOrDefault();
                string resvalue = "";
                if(sellobj != null)
                {
                    BuyItem obj = new BuyItem();
                    if(Buyerid != 0)
                    {
                        obj.Buyerid = Buyerid;
                    }
                    
                    obj.SellItemid = Sellitemid;
                    obj.Boughtdate = DateTime.Now;
                    db.BuyItems.Add(obj);
                    db.SaveChanges();

                    sellobj.Sold = "Sold";
                    db.Entry(sellobj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    resvalue = obj.BuyItemid.ToString();
                }
                else
                {
                    resvalue = "This Item already Sold";
                }
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(resvalue));
                return response;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [AllowCrossSiteJson]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetItems(string category = "Metal")
        {
            var products = db.SellItems.Where(x => x.Sold == "Not Sold" && x.Category.ToLower() == category.ToLower()).ToList();
            List<ItemsViewModel> lst = new List<ItemsViewModel>();
            
            foreach (var data in products)
            {
                ItemsViewModel p = new ItemsViewModel();
                p.Sellitemid = data.SellItemid; p.Productname = data.Productname;  p.Productprice = data.Price;
                p.category = data.Category;
                p.Productdesc = data.Productdescription;
                p.Status = data.Sold;
                p.userid = data.Usertableid.Value;
                p.Productimage = tempDocUrl + "/Uploadedimages/" + data.Productimageone;
                lst.Add(p);

            }
            try
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(lst));
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return response;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [AllowCrossSiteJson]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetAds(string userid)
        {
            int tempuserid = Convert.ToInt32(userid);
            var products = db.SellItems.Where(x => x.Usertableid.Value == tempuserid).ToList();
            List<ItemsViewModel> lst = new List<ItemsViewModel>();

            foreach (var data in products)
            {
                ItemsViewModel p = new ItemsViewModel();
                p.Sellitemid = data.SellItemid; p.Productname = data.Productname; p.Productprice = data.Price;
                p.category = data.Category;
                p.Productdesc = data.Productdescription;
                p.Status = data.Sold;
                p.userid = data.Usertableid.Value;
                p.Productimage = tempDocUrl + "/Uploadedimages/" + data.Productimageone;
                lst.Add(p);

            }
            try
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(lst));
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return response;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [AllowCrossSiteJson]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetOrders(string userid)
        {
            int tempuserid = Convert.ToInt32(userid);

            var orders = db.BuyItems.Where(x => x.Buyerid.Value == tempuserid).ToList();
            var products = db.SellItems.Where(x => x.Sold == "Sold").ToList();
            var users = db.Usertables.ToDictionary(x => x.Usertableid, x => x.Username);
            var result = (from o in orders
                          join p in products on o.SellItemid.Value equals p.SellItemid
                          select new { o, p }).ToList();
            List<ItemsViewModel> lst = new List<ItemsViewModel>();

            foreach (var data in result)
            {
                ItemsViewModel p = new ItemsViewModel();
                p.Sellitemid = data.p.SellItemid; p.Productname = data.p.Productname; p.Productprice = data.p.Price;
                p.category = data.p.Category;
                p.Productdesc = data.p.Productdescription;
                p.Status = "";
                p.Sellername = users[data.p.Usertableid.Value];
                p.Productimage = @"http://aade-49-207-225-120.ngrok.io/Uploadedimages/" + data.p.Productimageone;
                lst.Add(p);

            }
            try
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(lst));
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return response;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [AllowCrossSiteJson]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> Signup()
        {
            try
            {
                var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
                //access form data  
                NameValueCollection formData = provider.FormData;
                var username = formData["username"];
                var email = formData["email"];
                var password = formData["password"];
                var mobileno = formData["mobileno"];
                var city = formData["city"];
                var pincode = formData["pincode"];
                var valuedata = formData["valuedata"];
                var lastsellid = Convert.ToInt32(formData["lastsellitemid"].ToString());
                var lastbuyid = Convert.ToInt32(formData["lastbuyitemid"].ToString());

                var existmobileno = db.Usertables.ToList().Where(x => x.Mobileno == mobileno.Trim()).Count();
                var existusername = db.Usertables.ToList().Where(x => x.Username.ToLower() == username.Trim().ToLower()).Count();
                var existemail = db.Usertables.ToList().Where(x => x.Email.ToLower() == email.Trim().ToLower()).Count();
                string resvalue = "";
                
                
                if (existmobileno > 0)
                {
                    resvalue = "Mobileno already exists";
                }
                else if(existusername > 0)
                {
                    resvalue = "Username already exists";
                }
                else if (existemail > 0)
                {
                    resvalue = "Email already exists";
                }
                else
                {
                    Usertable usertable = new Usertable();
                    usertable.City = city;
                    usertable.Email = email;
                    usertable.Mobileno = mobileno;
                    usertable.Password = password;
                    usertable.Username = username;
                    usertable.Zipcode = pincode;
                    db.Usertables.Add(usertable);
                    db.SaveChanges();

                    if(lastsellid != 0)
                    {
                        SellItem obj = db.SellItems.Where(x => x.SellItemid == lastsellid).FirstOrDefault();
                        if(obj != null)
                        {
                            obj.Usertableid = usertable.Usertableid;
                            db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }

                    if(lastbuyid != 0)
                    {
                        BuyItem obj = db.BuyItems.Where(x => x.BuyItemid == lastbuyid).FirstOrDefault();
                        if (obj != null)
                        {
                            obj.Buyerid = usertable.Usertableid;
                            db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }

                    resvalue = usertable.Usertableid.ToString() + "~" + usertable.Username + "~" + usertable.Email + "~"
                        + usertable.Mobileno + "~" + usertable.City + "~" + usertable.Zipcode;
                }

                

                
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(resvalue));
                return response;
            }
            catch(Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }


        [AllowCrossSiteJson]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> Login()
        {
            try
            {
                var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
                //access form data  
                NameValueCollection formData = provider.FormData;
                var username = formData["username"];

                var password = formData["password"];
                
                
                var valuedata = formData["valuedata"];
                var lastsellid = Convert.ToInt32(formData["lastsellitemid"].ToString());
                var lastbuyid = Convert.ToInt32(formData["lastbuyitemid"].ToString());

                var existusername = db.Usertables.ToList().Where(x => x.Username.ToLower().Trim() == username.ToLower().Trim() ||
                x.Email.ToLower().Trim() == username.ToLower().Trim()).Count();
                var existpassword = db.Usertables.ToList().Where(x => x.Password.Trim() == password.Trim()).Count();

                string resvalue = "";


                if (existpassword < 1 || existusername < 1)
                {
                    resvalue = "Wrong Credentials";
                }
                else
                {
                    Usertable usertable = db.Usertables.Where(x => (x.Username.ToLower().Trim() == username.ToLower().Trim() ||
                x.Email.ToLower().Trim() == username.ToLower().Trim()) && x.Password.Trim() == password.Trim()).FirstOrDefault();

                    if (lastsellid != 0 && usertable != null)
                    {
                        SellItem obj = db.SellItems.Where(x => x.SellItemid == lastsellid).FirstOrDefault();
                        if (obj != null)
                        {
                            obj.Usertableid = usertable.Usertableid;
                            db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }

                    if (lastbuyid != 0 && usertable != null)
                    {
                        BuyItem obj = db.BuyItems.Where(x => x.BuyItemid == lastbuyid).FirstOrDefault();
                        if (obj != null)
                        {
                            obj.Buyerid = usertable.Usertableid;
                            db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }

                    resvalue = usertable.Usertableid.ToString() + "~" + usertable.Username + "~" + usertable.Email + "~"
                        + usertable.Mobileno + "~" + usertable.City + "~" + usertable.Zipcode ;
                }




                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(resvalue));
                return response;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [AllowCrossSiteJson]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> TestDataPost()
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
                var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
                //access form data  
                NameValueCollection formData = provider.FormData;
                //access files  
                IList<HttpContent> files = provider.Files;
                var cat = formData["category"];
                var name = formData["name"];
                var desc = formData["desc"];
                var price = formData["price"];
                var userid = Convert.ToInt32(formData["userid"].ToString());
                HttpContent file1 = files[0];
                var thisFileName = file1.Headers.ContentDisposition.FileName.Trim('\"');
                string filename = String.Empty;
                Stream input = await file1.ReadAsStreamAsync();
                string directoryName = String.Empty;
                string URL = String.Empty;
                string tempDocUrl = WebConfigurationManager.AppSettings["DocsUrl"];

                Guid guid = Guid.NewGuid();
                thisFileName = guid.ToString() + ".png";

                
                

                var path = HttpRuntime.AppDomainAppPath;
                directoryName = System.IO.Path.Combine(path, "Uploadedimages");
                filename = System.IO.Path.Combine(directoryName, thisFileName);

                //Deletion exists file  
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }

                string DocsPath = tempDocUrl + "/" + "Uploadedimages" + "/";
                URL = DocsPath + thisFileName;

                


                //Directory.CreateDirectory(@directoryName);  
                using (Stream file = File.OpenWrite(filename))
                {
                    input.CopyTo(file);
                    //close file  
                    file.Close();
                }

                SellItem item = new SellItem();
                item.Createddate = DateTime.Now;
                item.Price = price;
                item.Productname = name;
                if(userid != 0)
                {
                    item.Usertableid = userid;
                }
                //item.Usertableid = Convert.ToInt32(userid);
                item.Sold = "Not Sold";
                item.Productimageone = thisFileName;
                item.Productdescription = desc;
                item.Category = cat;
                db.SellItems.Add(item);
                db.SaveChanges();
                string resvalue = item.SellItemid.ToString();
                var response = Request.CreateResponse(HttpStatusCode.OK);
                //response.Headers.Add("DocsUrl", URL);
                response.Content = new StringContent(JsonConvert.SerializeObject(resvalue));
                return response;
                //before
                //var resss = await Request.Content.ReadAsStreamAsync();
                //Image image = System.Drawing.Image.FromStream(resss);
                //image.Save(HttpContext.Current.Server.MapPath("~/Uploadedimages") + "\\Image.jpg", ImageFormat.Jpeg);
                //string root = HttpContext.Current.Server.MapPath("~/Uploadedimages");
                //var provider = new MultipartFormDataStreamProvider(root);
                //await Request.Content.ReadAsMultipartAsync(provider);



                //// This illustrates how to get the file names.
                //foreach (MultipartFileData file in provider.FileData)
                //{
                //    Trace.WriteLine(file.Headers.ContentDisposition.FileName);
                //    Trace.WriteLine("Server file path: " + file.LocalFileName + ".png");
                //}

            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }
    }
}
