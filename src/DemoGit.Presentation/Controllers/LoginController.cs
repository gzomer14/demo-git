using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using DemoGit.Domain.Entities;
using DemoGit.Infrastructure.Context.Interfaces;
using DemoGit.Security.AES;
using DemoGit.Security.Argon2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DemoGit.Presentation.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IUsuarioRepository _repository;

        public LoginController(ILogger<LoginController> logger, IUsuarioRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated ?? false)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public IActionResult Login(Usuario model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var usuarioCadastrado = _repository.SelectByUsername(model.Username);

                    if (usuarioCadastrado is null)
                    {
                        ModelState.AddModelError("ErroLoginUsuario", "Usuário não cadastrado!");
                        return View("Index", model);
                    }

                    if (!Argon2Utils.VerifyHash(model.Password!, usuarioCadastrado.HashPassword!))
                    {
                        ModelState.AddModelError("ErroLoginUsuario", "Senha incorreta!");
                        return View("Index", model);
                    }

                    SignIn(usuarioCadastrado);

                    return RedirectToAction("Index", "Home");
                }

                return View("Index", model);
            }
            catch (Exception)
            {
                ModelState.AddModelError("ErroLoginUsuario", "Erro inesperado, favor entrar em contato com suporte do sistema!");
                return View("Index", model);
            }
        }

        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateUser(Usuario model)
        {
            if (ModelState.IsValid)
            {
                var usuarioCadastrado = _repository.SelectByUsername(model.Username);

                if (usuarioCadastrado is not null)
                {
                    ModelState.AddModelError("ErroCreateUserUsuario", "Usuário já cadastrado no sistema!");
                    return View(model);
                }

                if (String.IsNullOrWhiteSpace(model.Password))
                {
                    ModelState.AddModelError("ErroCreateUserUsuario", "Insira uma senha válida!");
                    return View(model);
                }

                _repository.Create(model, string.Concat(HttpContext.Request.Scheme, "://", HttpContext.Request.Host.Value));
                return (User.Identity?.IsAuthenticated ?? false) ? RedirectToAction("Index", "Usuario") : RedirectToAction("Index", "Login");
            }

            return View(model);
        }

        private async void SignIn(Usuario model)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Username!),
                new Claim("FullName", model.FullName!),
                new Claim(ClaimTypes.Role, model.Role!),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                //AllowRefresh = <bool>,
                // Refreshing the authentication session should be allowed.

                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                //IsPersistent = true,
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        public IActionResult LogOff()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();

            return RedirectToAction("Index");
        }

        public IActionResult EsqueciSenha()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EsqueciSenha([Bind("Username")] Usuario model)
        {
            if (string.IsNullOrWhiteSpace(model.Username))
            {
                ModelState.AddModelError("ErroLoginEsqueciSenha", "Informe um usuário válido!");
                return View(model);
            }

            var user = _repository.SelectByUsername(model.Username);
            if (user is null)
            {
                ModelState.AddModelError("ErroLoginEsqueciSenha", "Usuário inexistente!");
                return View(model);
            }

            _repository.EnviarEmailEsqueciSenha(user, string.Concat(HttpContext.Request.Scheme, "://", HttpContext.Request.Host.Value));
            return RedirectToAction("SucessoEsqueciSenha");
        }

        public IActionResult SucessoEsqueciSenha()
        {
            return View();
        }

        public IActionResult RedefinirSenha(string user)
        {
            var findUser = _repository.SelectByUsername(AesUtils.Decrypt(HttpUtility.UrlDecode(user)));
            if (findUser is null)
                return RedirectToAction("Index");

            return View(findUser);
        }

        [HttpPost]
        public IActionResult RedefinirSenha([Bind("Username, Password")] Usuario model)
        {
            if (ModelState.IsValid)
            {
                var user = _repository.SelectByUsername(model.Username!);
                _repository.Update(user, model.Password!);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public IActionResult AcessoNegado()
        {
            return View();
        }
    }
}