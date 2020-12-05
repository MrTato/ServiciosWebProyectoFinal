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
    public class ServicioController : Controller
    {
        private string baseURL = "https://localhost:44362/";
        private TokenController tokenController = new TokenController();

        // GET: Servicio
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
            HttpResponseMessage response = httpClient.GetAsync("api/Servicios").Result;
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Index", "Token");
            }
            else
            {
                string data = response.Content.ReadAsStringAsync().Result;
                List<ServicioCLS> servicios = JsonConvert.DeserializeObject<List<ServicioCLS>>(data);

                return View(servicios);
            }
        }

        public ActionResult DetailServicio(int id)
        {
            tokenController.LifeTimeValidator(System.Web.HttpContext.Current);

            GetInidcadores();

            var item = GetServicio(id);

            ServicioCLS servicio = new ServicioCLS();

            servicio.IdServicio = item.IdServicio;
            servicio.IdTipoServicio = item.IdTipoServicio;
            servicio.Nombre = item.Nombre;
            servicio.CostoBase = item.CostoBase;

            return View(servicio);
        }

        public ActionResult Guardar()
        {
            tokenController.LifeTimeValidator(System.Web.HttpContext.Current);

            GetInidcadores();

            return View();
        }

        [HttpPost]
        public ActionResult Guardar(
            int IdServicio,
            int IdTipoServicio,
            string Nombre,
            Nullable<decimal> CostoBase
            )
        {

            tokenController.LifeTimeValidator(System.Web.HttpContext.Current);

            try
            {
                ServicioCLS servicio = new ServicioCLS();
                servicio.IdServicio = IdServicio;
                servicio.IdTipoServicio = IdTipoServicio;
                servicio.Nombre = Nombre;
                servicio.CostoBase = CostoBase;

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(baseURL);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());

                string servicioJson = JsonConvert.SerializeObject(servicio);
                HttpContent body = new StringContent(servicioJson, Encoding.UTF8, "application/json");


                HttpResponseMessage response = httpClient.PostAsync("api/Servicios", body).Result;
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

        private ServicioCLS GetServicio(int id)
        {
            tokenController.LifeTimeValidator(System.Web.HttpContext.Current);

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());

            HttpResponseMessage response = httpClient.GetAsync($"api/Servicios/{id}").Result;
            string data = response.Content.ReadAsStringAsync().Result;
            ServicioCLS servicio = JsonConvert.DeserializeObject<ServicioCLS>(data);

            return servicio;
        }

        public ActionResult Editar(int id)
        {
            tokenController.LifeTimeValidator(System.Web.HttpContext.Current);

            GetInidcadores();

            ServicioCLS servicio = new ServicioCLS();

            var item = GetServicio(id);

            servicio.IdServicio = item.IdServicio;
            servicio.IdTipoServicio = item.IdTipoServicio;
            servicio.Nombre = item.Nombre;
            servicio.CostoBase = item.CostoBase;

            return View(servicio);
        }

        [HttpPost]
        public ActionResult Editar(
            int IdServicio,
            int IdTipoServicio,
            string Nombre,
            Nullable<decimal> CostoBase
            )
        {
            try
            {
                tokenController.LifeTimeValidator(System.Web.HttpContext.Current);

                ServicioCLS servicio = new ServicioCLS();
                servicio.IdServicio = IdServicio;
                servicio.IdTipoServicio = IdTipoServicio;
                servicio.Nombre = Nombre;
                servicio.CostoBase = CostoBase;

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(baseURL);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());

                string servicioJson = JsonConvert.SerializeObject(servicio);
                HttpContent body = new StringContent(servicioJson, Encoding.UTF8, "application/json");


                HttpResponseMessage response = httpClient.PutAsync($"api/Servicios/{IdServicio}", body).Result;
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
            tokenController.LifeTimeValidator(System.Web.HttpContext.Current);

            GetInidcadores();

            ServicioCLS servicio = new ServicioCLS();

            var item = GetServicio(id);

            servicio.IdServicio = item.IdServicio;
            servicio.IdTipoServicio = item.IdTipoServicio;
            servicio.Nombre = item.Nombre;
            servicio.CostoBase = item.CostoBase;

            return View(servicio);
        }

        [HttpPost]
        public ActionResult Eliminar(ServicioCLS servicio)
        {
            tokenController.LifeTimeValidator(System.Web.HttpContext.Current);

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());

            HttpResponseMessage response = httpClient.DeleteAsync($"api/Servicios/{servicio.IdServicio}").Result;

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