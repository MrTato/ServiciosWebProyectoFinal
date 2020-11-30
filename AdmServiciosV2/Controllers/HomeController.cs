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

                return View(clientes);
            }

        }

        public ActionResult Guardar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Guardar(string Nombre, string Apellido, string Telefono, string Tipo, string Estado)
        {


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
                    /*return Json(
                        new
                        {
                            success = true,
                            message = "El cliente fue creado satisfactoriamente"
                        }, JsonRequestBehavior.AllowGet);*/
                    return RedirectToAction("Lista");
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
                    /*return Json(
                        new
                        {
                            success = true,
                            message = "Cliente modificado satisfactoriamente"
                        }, JsonRequestBehavior.AllowGet);*/
                    return RedirectToAction("ListaCliente");
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
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());

            HttpResponseMessage response = httpClient.DeleteAsync($"api/Clientes/{oCliente.IdCliente}").Result;

            if (response.IsSuccessStatusCode)
            {
                /*return Json(
                    new
                    {
                        success = true,
                        message = "El cliente fue eliminado satisfactoriamente"
                    }, JsonRequestBehavior.AllowGet);*/
                return RedirectToAction("Lista");
            }

            throw new Exception("Error al eliminar");
        }

        private bool UsuarioAutenticado()
        {
            return HttpContext.Session["token"] != null;
        }
    }
}