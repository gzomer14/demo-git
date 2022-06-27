using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DemoGit.Domain.Extensions;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DemoGit.Domain.Entities;

public class Produto
{
    public Produto()
    {
        Id = ObjectId.GenerateNewId().ToString();
    }

    [BsonId]
    public string? Id { get; set; }

    [Display(Name = "Descrição")]
    [Required(ErrorMessage = "Obrigatório o preenchimento do campo descrição!")]
    public string? Descricao { get; set; }

    [Display(Name = "Preço")]
    [Required(ErrorMessage = "Obrigatório o preenchimento do campo preço!")]
    public double? Preco { get; set; }

    [Display(Name = "Quantidade em Estoque")]
    [Required(ErrorMessage = "Obrigatório o preenchimento do campo quantidade de estoque!")]
    public int? QuantidadeEstoque { get; set; }

    private IFormFile? _arquivoImagem;

    [BsonIgnore]
    [Display(Name = "Imagem do produto")]
    [AllowedExtensions(new string[] { ".jpg", ".png" }, ErrorMessage = "Tipos de arquivo permitidos: .png, .jpg")]
    public IFormFile? ArquivoImagem
    {
        get { return _arquivoImagem; }
        set
        {
            _arquivoImagem = value;

            if (_arquivoImagem is null)
                Imagem = null;
            else
            {
                using (var ms = new MemoryStream())
                {
                    _arquivoImagem.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    Imagem = fileBytes;
                }
            }
        }
    }

    public byte[]? Imagem { get; set; }

    [BsonIgnore]
    public bool IsFavoritado { get; set; }
}
