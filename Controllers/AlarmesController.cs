using System;
using Microsoft.AspNetCore.Mvc;
using ConfiguradorDeComponents.Models;
using ConfiguradorDeComponents.DAL;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ConfiguradorDeComponents.Controllers
{
    public class AlarmesController : Controller
    {
        private AlarmesDAL _alarmesDAL;

        public IActionResult ObterAlarmes()
        {
            _alarmesDAL = new AlarmesDAL();

            return View(_alarmesDAL.ObterAlarmes());
        }

        public IActionResult AdicionarAlarmes()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AdicionarAlarmes(Alarmes alarmeObj)
        {
            try{
                if(ModelState.IsValid){
                    _alarmesDAL = new AlarmesDAL();

                    if (_alarmesDAL.AdicionarAlarme(alarmeObj)){
                        ViewBag.Mensagem = "Alarme cadastrado!";
                    }
                }
                return View();
            }
            catch (Exception){
                return View("ObterAlarmes");
            }
        }

        public IActionResult EditarAlarme(int id)
        {
            _alarmesDAL = new AlarmesDAL();
            return View(_alarmesDAL.ObterAlarmes().Find(a => a.IdAlarme == id));
        }

        [HttpPost]
        public IActionResult EditarAlarme(int id, Alarmes alarmeObj)
        {
            try
            {
                _alarmesDAL = new AlarmesDAL();
                _alarmesDAL.EditarAlarme(alarmeObj);
                return RedirectToAction("ObterAlarmes");
            }
            catch (Exception)
            {
                ViewBag.Mensagem = "Falha ao editar!";
                return View("ObterAlarmes");
            }
        }

        public IActionResult DeletarAlarme(int id)
        {
            try
            {
                _alarmesDAL = new AlarmesDAL();
                if (_alarmesDAL.DeletarAlarme(id))
                {
                    ViewBag.Mensagem = "Alarme excluído!";
                }

                return RedirectToAction("ObterAlarmes");
            }
            catch
            {
                return View("ObterAlarmes");
            }
        }
    }
}