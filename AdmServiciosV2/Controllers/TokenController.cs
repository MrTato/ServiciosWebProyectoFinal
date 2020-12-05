using AdmServiciosV2.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Timers;
using System.Web;
using System.Web.Mvc;

namespace AdmServiciosV2.Controllers
{
    //public static class SessionExtensions
    //{
    //    /// <summary> 
    //    /// Get value. 
    //    /// </summary> 
    //    /// <typeparam name="T"></typeparam> 
    //    /// <param name="session"></param> 
    //    /// <param name="key"></param> 
    //    /// <returns></returns> 
    //    public static T GetDataFromSession<T>(HttpContext context, string key)
    //    {
    //        if (context != null && context.Session != null)
    //        {
    //            context.Session.Abandon();
    //        }

    //        return (T)context.Session[key];
    //    }
    //    /// <summary> 
    //    /// Set value. 
    //    /// </summary> 
    //    /// <typeparam name="T"></typeparam> 
    //    /// <param name="session"></param> 
    //    /// <param name="key"></param> 
    //    /// <param name="value"></param> 
    //    public static void SetDataToSession<T>(HttpContext context, string key, object value)
    //    {
    //        context.Session[key] = value;
    //    }
    //}
    public class TokenController : Controller
    {
        private string baseUrl = "https://localhost:44362/";

        //private HttpContext currentSession = System.Web.HttpContext.Current;


        // GET: Token
        public ActionResult Index()
        {
            if (HttpContext.Session["token"] == null)
            {
                ViewBag.Message = "Presione login para autenticarse";
            }
            else
            {
                ViewBag.Message = "Presione logout para cerrar la sesión";
                return RedirectToAction("Index", "Home");
            }

            //var aTimer = new Timer(60 * 1000); //one hour in milliseconds
            //aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            //aTimer.Start();

            return View();
        }
        //private void OnTimedEvent(object source, ElapsedEventArgs e)
        //{
        //    Token token = RequestToken();
        //    System.Web.HttpContext.Current.Session["token"] = token.AccessToken;
        //}

        public Token RequestToken(HttpContext context)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            Usuario user = new Usuario();
            user.Username = "admin";
            user.Password = "12345";

            string stringData = JsonConvert.SerializeObject(user);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync("api/Token", contentData).Result;
            string stringJWT = response.Content.ReadAsStringAsync().Result;
            Token token = JsonConvert.DeserializeObject<Token>(stringJWT);

            context.Session.Add("token", token.AccessToken);
            context.Session.Add("expiration", DateTime.UtcNow.AddMinutes(1));

            return token;
        }

        public ActionResult Login()
        {
            RequestToken(System.Web.HttpContext.Current);

            ViewBag.Message = "Usuario autenticado";

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Remove("token");

            ViewBag.Message = "Usuario ha salido de la sesion";

            return View("Index");
        }

        private void GetInidcadores()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseUrl);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());

            HttpResponseMessage response = httpClient.GetAsync("api/Indicadores").Result;
            string data = response.Content.ReadAsStringAsync().Result;
            IndicadoresCLS indicadores = JsonConvert.DeserializeObject<IndicadoresCLS>(data);

            ViewBag.TotalFacturas = indicadores.TotalFacturas;
            ViewBag.ServiciosFacturados = indicadores.ServiciosFacturados;
            ViewBag.TotalServicios = indicadores.TotalServicios;
            ViewBag.TotalClientes = indicadores.TotalClientes;
        }

        public void LifeTimeValidator(HttpContext context)
        {
            if (!(DateTime.UtcNow < (DateTime)context.Session["expiration"]))
            {
                RequestToken(context);
            }
        }
    }
}