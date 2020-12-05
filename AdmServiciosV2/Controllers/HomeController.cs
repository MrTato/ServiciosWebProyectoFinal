using AdmServiciosV2.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AdmServiciosV2.Controllers
{
    public class HomeController : Controller
    {
        private string baseURL = "https://localhost:44362/";
        private TokenController tokenController = new TokenController();

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
            HttpResponseMessage response = httpClient.GetAsync("api/ListClient").Result;
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Index", "Token");
            }
            else
            {
                string data = response.Content.ReadAsStringAsync().Result;
                List<ListClientCLS> listClient = JsonConvert.DeserializeObject<List<ListClientCLS>>(data);

                return View(listClient);
            }
        }

        public ActionResult DetailInvoice(int id)
        {
            tokenController.LifeTimeValidator(System.Web.HttpContext.Current);

            GetInidcadores();

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());

            HttpResponseMessage response = httpClient.GetAsync($"api/DetailInvoice/{id}").Result;
            string data = response.Content.ReadAsStringAsync().Result;
            DetailInvoiceCLS item = JsonConvert.DeserializeObject<DetailInvoiceCLS>(data);

            DetailInvoiceCLS invoice = new DetailInvoiceCLS();

            invoice.Nombre = item.Nombre;
            invoice.Apellido = item.Apellido;
            invoice.Telefono = item.Telefono;
            invoice.IdFactura = item.IdFactura;
            invoice.Total = item.Total;
            invoice.Fecha = item.Fecha;
            invoice.Cantidad = item.Cantidad;
            invoice.NombreServicio = item.NombreServicio;
            invoice.CostoBase = item.CostoBase;
            invoice.Direccion = item.Direccion;
            invoice.Descripcion = item.Descripcion;

            return View(invoice);
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