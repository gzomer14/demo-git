using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DemoGit.Infrastructure.Context.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DemoGit.Presentation.Controllers
{
    [Authorize]
    public class CompraEfetivadaController : Controller
    {
        private readonly ILogger<CompraEfetivadaController> _logger;
        private readonly ICompraEfetivadaRepository _repository;
        private readonly IProdutoRepository _produtoRepository;

        public CompraEfetivadaController(ILogger<CompraEfetivadaController> logger, ICompraEfetivadaRepository repository, IProdutoRepository produtoRepository)
        {
            _logger = logger;
            _repository = repository;
            _produtoRepository = produtoRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CarregarListaCompraEfetivadaIndex()
        {
            var listObj = new List<object>();

            foreach (var compra in _repository.SelectAll())
            {
                var prod = _produtoRepository.SelectById(compra.ProdutoId ?? "");

                listObj.Add(new
                {
                    ProdutoDescricao = prod.Descricao,
                    ProdutoImagem = prod.Imagem,
                    ValorTotal = compra.ValorTotal,
                    ProdutoPreco = prod.Preco,
                    QuantidadeCompra = compra.QuantidadeCompra
                });
            }

            return Json(new { compra = listObj });
        }
    }
}