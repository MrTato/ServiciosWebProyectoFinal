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
    public class UbicacionController : Controller
    {
        private string baseURL = "https://localhost:44362/";

        // GET: Ubicacion
        public ActionResult Index()
        {
            if (!UsuarioAutenticado())
            {
                return RedirectToAction("Index", "Token");
            }

            GetInidcadores();

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());
            HttpResponseMessage response = httpClient.GetAsync("api/Ubicacions").Result;
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Index", "Token");
            }
            else
            {
                string data = response.Content.ReadAsStringAsync().Result;
                List<UbicacionCLS> ubicacion = JsonConvert.DeserializeObject<List<UbicacionCLS>>(data);

                return View(ubicacion);
            }
        }
        
        public ActionResult DetailUbicacion(int id)
        {
            GetInidcadores();

            var item = GetUbicacion(id);

            UbicacionCLS ubicaicon = new UbicacionCLS();

            ubicaicon.IdUbicacion = item.IdUbicacion;
            ubicaicon.Nombre = item.Nombre;
            ubicaicon.Tipo = item.Tipo;

            return View(ubicaicon);
        }

        private UbicacionCLS GetUbicacion(int id)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());

            HttpResponseMessage response = httpClient.GetAsync($"api/Ubicacions/{id}").Result;
            string data = response.Content.ReadAsStringAsync().Result;
            UbicacionCLS item = JsonConvert.DeserializeObject<UbicacionCLS>(data);

            return item;
        }

        private bool UsuarioAutenticado()
        {
            return HttpContext.Session["token"] != null;
        }

        private void GetInidcadores()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
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
    }
}