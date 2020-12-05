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
    public class FacturaController : Controller
    {
        private string baseURL = "https://localhost:44362/";

        // GET: Factura
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
            HttpResponseMessage response = httpClient.GetAsync("api/Facturas").Result;
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Index", "Token");
            }
            else
            {
                string data = response.Content.ReadAsStringAsync().Result;
                List<FacturaCLS> clientes = JsonConvert.DeserializeObject<List<FacturaCLS>>(data);

                return View(clientes);
            }
        }

        public ActionResult DetailFactura(int id)
        {
            GetInidcadores();

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());

            HttpResponseMessage response = httpClient.GetAsync($"api/Facturas/{id}").Result;
            string data = response.Content.ReadAsStringAsync().Result;
            FacturaCLS item = JsonConvert.DeserializeObject<FacturaCLS>(data);

            FacturaCLS factura = new FacturaCLS();

            factura.IdFactura = item.IdFactura;
            factura.Numero = item.Numero;
            factura.IdCliente = item.IdCliente;
            factura.IdDireccion = item.IdDireccion;
            factura.Fecha = item.Fecha;
            factura.Total = item.Total;

            return View(factura);
        }

        public ActionResult Guardar()
        {
            GetInidcadores();

            return View();
        }

        [HttpPost]
        public ActionResult Guardar(string Numero, int IdCliente, int IdDireccion, DateTime Fecha, decimal Total)
        {
            try
            {
                FacturaCLS factura = new FacturaCLS();

                factura.IdFactura = 0;
                factura.Numero = Numero;
                factura.IdCliente = IdCliente;
                factura.IdDireccion = IdDireccion;
                factura.Fecha = Fecha;
                factura.Total = Total;

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(baseURL);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());

                string clienteJson = JsonConvert.SerializeObject(factura);
                HttpContent body = new StringContent(clienteJson, Encoding.UTF8, "application/json");


                HttpResponseMessage response = httpClient.PostAsync("api/Facturas", body).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }


                throw new Exception("Error al guardar");
            }

            catch (Exception ex)
            {
                return Json(
                    new
                    {
                        success = false,
                        message = ex.InnerException
                    }, JsonRequestBehavior.AllowGet);
            }
        }

        private FacturaCLS GetFactura(int id)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());

            HttpResponseMessage response = httpClient.GetAsync($"api/Facturas/{id}").Result;
            string data = response.Content.ReadAsStringAsync().Result;
            FacturaCLS item = JsonConvert.DeserializeObject<FacturaCLS>(data);

            return item;
        }

        public ActionResult Editar(int id)
        {
            GetInidcadores();

            FacturaCLS factura = new FacturaCLS();

            var item = GetFactura(id);

            factura.IdFactura = item.IdFactura;
            factura.Numero = item.Numero;
            factura.IdCliente = item.IdCliente;
            factura.IdDireccion = item.IdDireccion;
            factura.Fecha = item.Fecha;
            factura.Total = item.Total;

            return View(factura);
        }

        [HttpPost]
        public ActionResult Editar(int IdFactura, string Numero, int IdCliente, int IdDireccion, DateTime Fecha, decimal Total)
        {
            try
            {
            FacturaCLS factura = new FacturaCLS();

            factura.IdFactura =IdFactura;
            factura.Numero = Numero;
            factura.IdCliente = IdCliente;
            factura.IdDireccion = IdDireccion;
            factura.Fecha = Fecha;
            factura.Total = Total;

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());

            string clienteJson = JsonConvert.SerializeObject(factura);
            HttpContent body = new StringContent(clienteJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = httpClient.PutAsync($"api/Facturas/{IdFactura}", body).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                throw new Exception("Error al guardar");
            }
            catch (Exception ex)
            {
                return Json(
                    new
                    {
                        success = false,
                        message = ex.InnerException
                    }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Eliminar(int id)
        {
            GetInidcadores();

            FacturaCLS factura = new FacturaCLS();

            var item = GetFactura(id);

            factura.IdFactura = item.IdFactura;
            factura.Numero = item.Numero;
            factura.IdCliente = item.IdCliente;
            factura.IdDireccion = item.IdDireccion;
            factura.Fecha = item.Fecha;
            factura.Total = item.Total;

            return View(factura);
        }

        [HttpPost]
        public ActionResult Eliminar(FacturaCLS oFactura)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());

            HttpResponseMessage response = httpClient.DeleteAsync($"api/Facturas/{oFactura.IdFactura}").Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            throw new Exception("Error al eliminar");
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