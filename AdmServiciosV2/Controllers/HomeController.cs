using AdmServiciosV2.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace AdmServiciosV2.Controllers
{
    public class HomeController : Controller
    {
        private string baseURL = "https://localhost:44362/";

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ListaCliente()
        {
            if (!UsuarioAutenticado())
            {
                return RedirectToAction("Index", "Token");
            }

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());
            HttpResponseMessage response = httpClient.GetAsync("api/Clientes").Result;
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Index", "Token");
            }
            else
            {
                string data = response.Content.ReadAsStringAsync().Result;
                List<ClienteCLS> clientes = JsonConvert.DeserializeObject<List<ClienteCLS>>(data);

                /*return Json(
                    new
                    {
                        success = true,
                        data = clientes,
                        message = "done"
                    },
                    JsonRequestBehavior.AllowGet
                    );*/
                return View(clientes);
            }

        }

        private bool UsuarioAutenticado()
        {
            return HttpContext.Session["token"] != null;
        }
    }
}