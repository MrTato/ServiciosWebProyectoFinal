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
    public class ClientesController : Controller
    {
        private string baseURL = "https://localhost:44362/";
        private TokenController tokenController = new TokenController();

        // GET: Clientes
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
            HttpResponseMessage response = httpClient.GetAsync("api/Clientes").Result;
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Index", "Token");
            }
            else
            {
                string data = response.Content.ReadAsStringAsync().Result;
                List<ClienteCLS> clientes = JsonConvert.DeserializeObject<List<ClienteCLS>>(data);

                return View(clientes);
            }
        }

        public ActionResult DetailCliente(int id)
        {
            tokenController.LifeTimeValidator(System.Web.HttpContext.Current);

            GetInidcadores();

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());

            HttpResponseMessage response = httpClient.GetAsync($"api/Clientes/{id}").Result;
            string data = response.Content.ReadAsStringAsync().Result;
            ClienteCLS item = JsonConvert.DeserializeObject<ClienteCLS>(data);

            ClienteCLS cliente = new ClienteCLS();

            cliente.IdCliente = item.IdCliente;
            cliente.Nombre = item.Nombre;
            cliente.Apellido = item.Apellido;
            cliente.Telefono = item.Telefono;
            cliente.Tipo = item.Tipo;
            cliente.Estado = item.Estado;

            return View(cliente);
        }

        public ActionResult Guardar()
        {
            tokenController.LifeTimeValidator(System.Web.HttpContext.Current);

            GetInidcadores();
            return View();
        }

        [HttpPost]
        public ActionResult Guardar(string Nombre, string Apellido, string Telefono, string Tipo, string Estado)
        {
            tokenController.LifeTimeValidator(System.Web.HttpContext.Current);

            try
            {
                ClienteCLS cliente = new ClienteCLS();
                cliente.IdCliente = 0;
                cliente.Nombre = Nombre;
                cliente.Apellido = Apellido;
                cliente.Telefono = Telefono;
                cliente.Tipo = Tipo;
                cliente.Estado = Estado;

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(baseURL);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());

                string clienteJson = JsonConvert.SerializeObject(cliente);
                HttpContent body = new StringContent(clienteJson, Encoding.UTF8, "application/json");


                HttpResponseMessage response = httpClient.PostAsync("api/Clientes", body).Result;
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

        private ClienteCLS GetCliente(int id)
        {
            tokenController.LifeTimeValidator(System.Web.HttpContext.Current);

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());

            HttpResponseMessage response = httpClient.GetAsync($"api/Clientes/{id}").Result;
            string data = response.Content.ReadAsStringAsync().Result;
            ClienteCLS clientes = JsonConvert.DeserializeObject<ClienteCLS>(data);

            return clientes;
        }

        public ActionResult Editar(int id)
        {
            tokenController.LifeTimeValidator(System.Web.HttpContext.Current);

            GetInidcadores();

            ClienteCLS cliente = new ClienteCLS();

            var item = GetCliente(id);

            cliente.IdCliente = item.IdCliente;
            cliente.Nombre = item.Nombre;
            cliente.Apellido = item.Apellido;
            cliente.Telefono = item.Telefono;
            cliente.Tipo = item.Tipo;
            cliente.Estado = item.Estado;

            return View(cliente);
        }

        [HttpPost]
        public ActionResult Editar(int IdCliente, string Nombre, string Apellido, string Telefono, string Tipo, string Estado)
        {
            tokenController.LifeTimeValidator(System.Web.HttpContext.Current);

            try
            {
                ClienteCLS cliente = new ClienteCLS();
                cliente.IdCliente = IdCliente;
                cliente.Nombre = Nombre;
                cliente.Apellido = Apellido;
                cliente.Telefono = Telefono;
                cliente.Tipo = Tipo;
                cliente.Estado = Estado;

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(baseURL);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());

                string clienteJson = JsonConvert.SerializeObject(cliente);
                HttpContent body = new StringContent(clienteJson, Encoding.UTF8, "application/json");


                HttpResponseMessage response = httpClient.PutAsync($"api/Clientes/{IdCliente}", body).Result;
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
            tokenController.LifeTimeValidator(System.Web.HttpContext.Current);

            GetInidcadores();

            ClienteCLS cliente = new ClienteCLS();

            var item = GetCliente(id);

            cliente.IdCliente = item.IdCliente;
            cliente.Nombre = item.Nombre;
            cliente.Apellido = item.Apellido;
            cliente.Telefono = item.Telefono;
            cliente.Tipo = item.Tipo;
            cliente.Estado = item.Estado;

            return View(cliente);
        }

        [HttpPost]
        public ActionResult Eliminar(ClienteCLS oCliente)
        {
            tokenController.LifeTimeValidator(System.Web.HttpContext.Current);

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());

            HttpResponseMessage response = httpClient.DeleteAsync($"api/Clientes/{oCliente.IdCliente}").Result;

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