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
    public class DetalleFacturaController : Controller
    {
        private string baseURL = "https://localhost:44362/";

        public ActionResult ListaDetalleFactura()
        {
            if (!UsuarioAutenticado())
            {
                return RedirectToAction("Index", "Token");
            }

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());
            HttpResponseMessage response = httpClient.GetAsync("api/DetalleFacturas").Result;
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Index", "Token");
            }
            else
            {
                string data = response.Content.ReadAsStringAsync().Result;
                List<DetalleFacturaCLS> detalleFacturas = JsonConvert.DeserializeObject<List<DetalleFacturaCLS>>(data);

                return View(detalleFacturas);
            }

        }

        public ActionResult Guardar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Guardar(
            int idDetalleFactura,
            int IdFactura,
            int IdServicio,
            int Cantidad,
            Nullable<bool> Entregado
            )
        {


            try
            {
                DetalleFacturaCLS detalleFactura = new DetalleFacturaCLS();
                detalleFactura.IdDetalleFactura = idDetalleFactura;
                detalleFactura.IdFactura = IdFactura;
                detalleFactura.IdServicio = IdServicio;
                detalleFactura.Cantidad = Cantidad;
                detalleFactura.Entregado = Entregado;

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(baseURL);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());

                string detalleFacturaJson = JsonConvert.SerializeObject(detalleFactura);
                HttpContent body = new StringContent(detalleFacturaJson, Encoding.UTF8, "application/json");


                HttpResponseMessage response = httpClient.PostAsync("api/DetalleFacturas", body).Result;
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

        private DetalleFacturaCLS GetDetalleFactura(int id)
        {

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());

            HttpResponseMessage response = httpClient.GetAsync($"api/DetalleFacturas/{id}").Result;
            string data = response.Content.ReadAsStringAsync().Result;
            DetalleFacturaCLS detalleFactura = JsonConvert.DeserializeObject<DetalleFacturaCLS>(data);

            return detalleFactura;
        }

        public ActionResult Editar(int id)
        {
            DetalleFacturaCLS detalleFactura = new DetalleFacturaCLS();

            var item = GetDetalleFactura(id);

            detalleFactura.IdDetalleFactura = item.IdDetalleFactura;
            detalleFactura.IdFactura = item.IdFactura;
            detalleFactura.IdServicio = item.IdServicio;
            detalleFactura.Cantidad = item.Cantidad;
            detalleFactura.Entregado = item.Entregado;

            return View(detalleFactura);
        }

        [HttpPost]
        public ActionResult Editar(
            int IdDetalleFactura,
            int IdFactura,
            int IdServicio,
            int Cantidad,
            Nullable<bool> Entregado
            )
        {
            try
            {
                DetalleFacturaCLS detalleFactura = new DetalleFacturaCLS();
                detalleFactura.IdDetalleFactura = IdDetalleFactura;
                detalleFactura.IdFactura = IdFactura;
                detalleFactura.IdServicio = IdServicio;
                detalleFactura.Cantidad = Cantidad;
                detalleFactura.Entregado = Entregado;

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(baseURL);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());

                string detalleFacturaJson = JsonConvert.SerializeObject(detalleFactura);
                HttpContent body = new StringContent(detalleFacturaJson, Encoding.UTF8, "application/json");


                HttpResponseMessage response = httpClient.PutAsync($"api/DetalleFacturas/{IdDetalleFactura}", body).Result;
                if (response.IsSuccessStatusCode)
                {
                    /*return Json(
                        new
                        {
                            success = true,
                            message = "Cliente modificado satisfactoriamente"
                        }, JsonRequestBehavior.AllowGet);*/
                    return RedirectToAction("ListaDetalleFactura");
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
            DetalleFacturaCLS detalleFactura = new DetalleFacturaCLS();

            var item = GetDetalleFactura(id);

            detalleFactura.IdDetalleFactura = item.IdDetalleFactura;
            detalleFactura.IdFactura = item.IdFactura;
            detalleFactura.IdServicio = item.IdServicio;
            detalleFactura.Cantidad = item.Cantidad;
            detalleFactura.Entregado = item.Entregado;

            return View(detalleFactura);
        }

        [HttpPost]
        public ActionResult Eliminar(DetalleFacturaCLS detalleFactura)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());

            HttpResponseMessage response = httpClient.DeleteAsync($"api/DetalleFacturas/{detalleFactura.IdDetalleFactura}").Result;

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