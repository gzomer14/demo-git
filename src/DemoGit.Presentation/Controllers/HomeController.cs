using System.Diagnostics;
using DemoGit.Infrastructure.Context.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DemoGit.Presentation.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IResourceRepository _resourceRepository;

    public HomeController(ILogger<HomeController> logger, IResourceRepository resourceRepository)
    {
        _logger = logger;
        _resourceRepository = resourceRepository;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}
