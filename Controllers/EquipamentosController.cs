using System;
using Microsoft.AspNetCore.Mvc;
using ConfiguradorDeComponents.Models;
using ConfiguradorDeComponents.DAL;

namespace ConfiguradorDeComponents.Controllers
{
    public class EquipamentosController : Controller
    {

        private EquipamentosDAL _equipamentosDAL;

        public IActionResult ObterEquipamentos()
        {
            _equipamentosDAL = new EquipamentosDAL();

            return View(_equipamentosDAL.ObterEquipamentos());
        }

        public IActionResult AdicionarEquipamento()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AdicionarEquipamento(Equipamentos equipamentoObj)
        {
            try{
                if(ModelState.IsValid){
                    _equipamentosDAL = new EquipamentosDAL();

                    if (_equipamentosDAL.AdicionarEquipamento(equipamentoObj)){
                        ViewBag.Mensagem = "Equipamento cadastrado!";
                    }
                }
                return View();
            }
            catch (Exception){
                return View("ObterEquipamentos");
            }
        }

        public IActionResult EditarEquipamento(int id)
        {
            _equipamentosDAL = new EquipamentosDAL();
            return View(_equipamentosDAL.ObterEquipamentos().Find(e => e.Id == id));
        }

        [HttpPost]
        public IActionResult EditarEquipamento(int id, Equipamentos equipamentoObj)
        {
            try
            {
                _equipamentosDAL = new EquipamentosDAL();
                _equipamentosDAL.EditarEquipamento(equipamentoObj);
                return RedirectToAction("ObterEquipamentos");
            }
            catch (Exception)
            {
                ViewBag.Mensagem = "Falha ao editar!";
                return View("ObterEquipamentos");
            }
        }

        public IActionResult DeletarEquipamento(int id)
        {
            try
            {
                _equipamentosDAL = new EquipamentosDAL();
                if (_equipamentosDAL.DeletarEquipamento(id))
                {
                    ViewBag.Mensagem = "Equipamento excluído!";
                }

                return RedirectToAction("ObterEquipamentos");
            }
            catch
            {
                return View("ObterEquipamentos");
            }
        }
    }
}