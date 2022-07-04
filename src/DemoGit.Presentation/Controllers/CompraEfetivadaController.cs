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
        private readonly IUsuarioRepository _usuarioRepository;

        public CompraEfetivadaController(ILogger<CompraEfetivadaController> logger, ICompraEfetivadaRepository repository, IProdutoRepository produtoRepository, IUsuarioRepository usuarioRepository)
        {
            _logger = logger;
            _repository = repository;
            _produtoRepository = produtoRepository;
            _usuarioRepository = usuarioRepository;
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
                var usuarioCompra = _usuarioRepository.SelectById(compra.UsuarioId ?? "");

                listObj.Add(new
                {
                    ProdutoDescricao = prod.Descricao,
                    ProdutoImagem = prod.Imagem,
                    UsuarioCompra = usuarioCompra.FullName,
                    ValorTotal = compra.ValorTotal,
                    ProdutoPreco = prod.Preco,
                    QuantidadeCompra = compra.QuantidadeCompra
                });
            }

            return Json(new { compra = listObj });
        }
    }
}