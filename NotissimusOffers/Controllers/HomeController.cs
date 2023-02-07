using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NotissimusOffers.Context;
using NotissimusOffers.Models;
using NotissimusOffers.Services;
using System.Diagnostics;

namespace NotissimusOffers.Controllers
{
	public class HomeController : Controller
	{
		private readonly ApplicationContext _db;
		private readonly IConfiguration _config;
		private readonly XmlHttpRequestService _requestService;

		public HomeController(ApplicationContext context, IConfiguration config, XmlHttpRequestService requestService)
		{
			_db = context;
			_config = config;
			_requestService = requestService;
		}

		public IActionResult Index()
		{
			string source = _config.GetValue<string>("OffersSource");
			int offerId = _config.GetValue<int>("OfferId");

			var ymlCatalog = _requestService.sendRequest<YmlCatalog>(source);
			if (ymlCatalog == null) 
				throw new ArgumentNullException(nameof(ymlCatalog));

			Offer? offer = ymlCatalog.Shop.Offers.Find(o => o.OfferId == offerId);

			if (offer == null)
				throw new ArgumentException(nameof(offer));

			if(_db.Offers.Where(o => o.OfferId == offerId).FirstOrDefault() == null)
			{
				_db.Offers.Add(offer);
				_db.SaveChanges();
			}

			return View(offer);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}