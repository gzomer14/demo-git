using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DemoGit.Domain.Entities;
using DemoGit.Infrastructure.Context.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DemoGit.Presentation.Controllers;

public class ProdutoController : Controller
{
    private readonly ILogger<ProdutoController> _logger;
    private readonly IProdutoRepository _repository;
    private readonly ICompraEfetivadaRepository _compraEfetivadaRepository;

    public ProdutoController(ILogger<ProdutoController> logger, IProdutoRepository repository, ICompraEfetivadaRepository compraEfetivadaRepository)
    {
        _logger = logger;
        _repository = repository;
        _compraEfetivadaRepository = compraEfetivadaRepository;
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
        _repository.DeleteById(model.Id ?? "");

        return RedirectToAction("Index");
    }

    public IActionResult Comprar(string id)
    {
        var produto = _repository.SelectById(id);

        ViewBag.CompraProdutoDescricao = produto.Descricao;
        ViewBag.CompraProdutoPreco = produto.Preco;
        ViewBag.CompraProdutoQuantidadeEstoque = produto.QuantidadeEstoque;

        return View(new CompraEfetivada { ProdutoId = produto.Id });
    }

    [HttpPost]
    public IActionResult Comprar(CompraEfetivada model)
    {
        var produto = _repository.SelectById(model.ProdutoId ?? "");

        if (produto.QuantidadeEstoque < model.QuantidadeCompra)
        {
            ModelState.AddModelError("CompraNaoPermitida", "Este produto não possui mais estoque para sua compra!");
            return View(model);
        }

        //Por algum motivo desconhecido o Id estava sendo copiado a partir do ProdutoId
        model.Id = ObjectId.GenerateNewId().ToString();
        model.ValorTotal = (produto.Preco ?? 0) * model.QuantidadeCompra;
        _compraEfetivadaRepository.Create(model);

        produto.QuantidadeEstoque -= model.QuantidadeCompra;
        _repository.Update(produto);

        return RedirectToAction("Index");
    }
}
