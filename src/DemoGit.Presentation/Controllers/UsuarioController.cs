using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DemoGit.Domain.Entities;
using DemoGit.Infrastructure.Context.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DemoGit.Presentation.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class UsuarioController : Controller
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly IUsuarioRepository _repository;

        public UsuarioController(ILogger<UsuarioController> logger, IUsuarioRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CarregarListaUsuariosIndex()
        {
            return Json(new { usuarios = _repository.SelectAll() });
        }

        public IActionResult Editar(string id)
        {
            return View(_repository.SelectById(id));
        }

        [HttpPost]
        public IActionResult Editar([Bind("Id, Username, Password, Email, FullName, Role")] Usuario model)
        {
            var usuarioCadastrado = _repository.Find(u => u.Username == model.Username && u.Id != model.Id).FirstOrDefault();

            if (usuarioCadastrado is not null)
            {
                ModelState.AddModelError("ErroEditarUsuario", "Usuário já cadastrado no sistema!");
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(model.Role))
            {
                ModelState.AddModelError("ErroEditarUsuario", "Role não informada!");
                return View(model);
            }

            _repository.UpdateToEdit(model);
            return RedirectToAction("Index");
        }

        public IActionResult Deletar(string id)
        {
            return View(_repository.SelectById(id));
        }

        [HttpPost]
        public IActionResult Deletar(Usuario model)
        {
            _repository.DeleteById(model.Id ?? "");

            return RedirectToAction("Index");
        }
    }
}