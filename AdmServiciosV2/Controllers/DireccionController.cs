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
    public class DireccionController : Controller
    {
        private string baseURL = "https://localhost:44362/";

        // GET: Direccion
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
            HttpResponseMessage response = httpClient.GetAsync("api/Direccions").Result;
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Index", "Token");
            }
            else
            {
                string data = response.Content.ReadAsStringAsync().Result;
                List<DireccionCLS> direcciones = JsonConvert.DeserializeObject<List<DireccionCLS>>(data);

                return View(direcciones);
            }
        }

        public ActionResult DetailDireccion(int id)
        {
            GetInidcadores();

            var item = GetDireccion(id);

            DireccionCLS direccion = new DireccionCLS();

            direccion.Direccion1 = item.Direccion1;
            direccion.IdCliente = item.IdCliente;
            direccion.IdDireccion = item.IdDireccion;
            direccion.IdUbicacion = item.IdUbicacion;

            return View(direccion);
        }

        public ActionResult Guardar()
        {
            GetInidcadores();

            return View();
        }

        [HttpPost]
        public ActionResult Guardar(
            int idDireccion,
            int idUbicacion,
            int IdCliente,
            string Direccion1
            )
        {


            try
            {
                DireccionCLS direccion = new DireccionCLS();
                direccion.IdDireccion = idDireccion;
                direccion.IdUbicacion = idUbicacion;
                direccion.IdCliente = IdCliente;
                direccion.Direccion1 = Direccion1;

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(baseURL);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());

                string direccionJson = JsonConvert.SerializeObject(direccion);
                HttpContent body = new StringContent(direccionJson, Encoding.UTF8, "application/json");


                HttpResponseMessage response = httpClient.PostAsync("api/Direccions", body).Result;
                if (response.IsSuccessStatusCode)
                {
                    /*return Json(
                        new
                        {
                            success = true,
                            message = "El cliente fue creado satisfactoriamente"
                        }, JsonRequestBehavior.AllowGet);*/
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

        private DireccionCLS GetDireccion(int id)
        {

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());

            HttpResponseMessage response = httpClient.GetAsync($"api/Direccions/{id}").Result;
            string data = response.Content.ReadAsStringAsync().Result;
            DireccionCLS direccion = JsonConvert.DeserializeObject<DireccionCLS>(data);

            return direccion;
        }

        public ActionResult Editar(int id)
        {
            GetInidcadores();

            DireccionCLS direccion = new DireccionCLS();

            var item = GetDireccion(id);

            direccion.Direccion1 = item.Direccion1;
            direccion.IdCliente = item.IdCliente;
            direccion.IdDireccion = item.IdDireccion;
            direccion.IdUbicacion = item.IdUbicacion;

            return View(direccion);
        }

        [HttpPost]
        public ActionResult Editar(
            string Direccion1,
            int IdCliente,
            int IdDireccion,
            int IdUbicacion
            )
        {
            try
            {
                DireccionCLS direccion = new DireccionCLS();
                direccion.Direccion1 = Direccion1;
                direccion.IdCliente = IdCliente;
                direccion.IdDireccion = IdDireccion;
                direccion.IdUbicacion = IdUbicacion;

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(baseURL);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());

                string direccionJson = JsonConvert.SerializeObject(direccion);
                HttpContent body = new StringContent(direccionJson, Encoding.UTF8, "application/json");


                HttpResponseMessage response = httpClient.PutAsync($"api/Direccions/{IdDireccion}", body).Result;
                if (response.IsSuccessStatusCode)
                {
                    /*return Json(
                        new
                        {
                            success = true,
                            message = "Cliente modificado satisfactoriamente"
                        }, JsonRequestBehavior.AllowGet);*/
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

            DireccionCLS direccion = new DireccionCLS();

            var item = GetDireccion(id);

            direccion.Direccion1 = item.Direccion1;
            direccion.IdDireccion = item.IdDireccion;
            direccion.IdCliente = item.IdCliente;
            direccion.IdUbicacion = item.IdUbicacion;

            return View(direccion);
        }

        [HttpPost]
        public ActionResult Eliminar(DireccionCLS direccion)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());

            HttpResponseMessage response = httpClient.DeleteAsync($"api/Direccions/{direccion.IdDireccion}").Result;

            if (response.IsSuccessStatusCode)
            {
                /*return Json(
                    new
                    {
                        success = true,
                        message = "El cliente fue eliminado satisfactoriamente"
                    }, JsonRequestBehavior.AllowGet);*/
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