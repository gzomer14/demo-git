using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DemoGit.Domain.Entities;
using DemoGit.Infrastructure.Context.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DemoGit.Presentation.Controllers;

public class ProdutoController : Controller
{
    private readonly ILogger<ProdutoController> _logger;
    private readonly IProdutoRepository _repository;

    public ProdutoController(ILogger<ProdutoController> logger, IProdutoRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Inserir()
    {
        return View();
    }

    public IActionResult CarregarListaProdutosIndex()
    {
        return Json(new { produtos = _repository.SelectAll() });
    }

    [HttpPost]
    public IActionResult Inserir(
                                    [Bind("Descricao, Preco, QuantidadeEstoque, ArquivoImagem")]
                                    Produto model
                                )
    {
        if (ModelState.IsValid)
        {
            _repository.Create(model);

            return RedirectToAction("Index");
        }

        return View(model);
    }

    public IActionResult Editar(string id)
    {
        return View(_repository.SelectById(id));
    }

    [HttpPost]
    public IActionResult Editar(Produto model)
    {
        if (ModelState.IsValid)
        {
            _repository.Update(model);

            return RedirectToAction("Index");
        }

        return View(model);
    }

    public IActionResult Deletar(string id)
    {
        return View(_repository.SelectById(id));
    }

    [HttpPost]
    public IActionResult Deletar(Produto model)
    {
        _repository.DeleteById(model.Id);

        return RedirectToAction("Index");
    }
}
