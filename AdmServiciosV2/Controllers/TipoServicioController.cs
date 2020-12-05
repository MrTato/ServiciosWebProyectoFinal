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
    public class TipoServicioController : Controller
    {
        private string baseURL = "https://localhost:44362/";
        private TokenController tokenController = new TokenController();

        // GET: TipoServicio
        public ActionResult Index()
        {
            if (!UsuarioAutenticado())
            {
                return RedirectToAction("Index", "Token");
            }
            tokenController.LifeTimeValidator(System.Web.HttpContext.Current);

            GetInidcadores();

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());
            HttpResponseMessage response = httpClient.GetAsync("api/TipoServicios").Result;
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Index", "Token");
            }
            else
            {
                string data = response.Content.ReadAsStringAsync().Result;
                List<TipoServicioCLS> tipoServicio = JsonConvert.DeserializeObject<List<TipoServicioCLS>>(data);

                return View(tipoServicio);
            }
        }

        public ActionResult DetailTipoServicio(int id)
        {
            tokenController.LifeTimeValidator(System.Web.HttpContext.Current);

            GetInidcadores();

            var item = GetTipoSevicio(id);

            TipoServicioCLS tipoServicio = new TipoServicioCLS();

            tipoServicio.IdTipoServicio = item.IdTipoServicio;
            tipoServicio.Descripcion = item.Descripcion;
            tipoServicio.Estado = item.Estado;

            return View(tipoServicio);

        }

        private TipoServicioCLS GetTipoSevicio(int id)
        {
            tokenController.LifeTimeValidator(System.Web.HttpContext.Current);

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());

            HttpResponseMessage response = httpClient.GetAsync($"api/TipoServicios/{id}").Result;
            string data = response.Content.ReadAsStringAsync().Result;
            TipoServicioCLS item = JsonConvert.DeserializeObject<TipoServicioCLS>(data);

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