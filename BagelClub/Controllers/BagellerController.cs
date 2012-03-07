using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BagelClub.Models;
using BagelClub.Services;
using BagelClub.ViewModels;
using Raven.Client.Document;

namespace BagelClub.Controllers
{
	public class BagellerController : Controller
	{
		private readonly IBagellerService _bagellerService;
		public BagellerController(IBagellerService bagellerService)
		{
			_bagellerService = bagellerService;
		}
		
		public ActionResult Index()
		{
			var items = _bagellerService.FetchAll();
			return View(items);
		}

		public ActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public ActionResult Create(FormCollection collection)
		{
			try
			{
				// TODO: Add insert logic here

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}
		
		public ActionResult Edit(int id)
		{
			var item = _bagellerService.FetchByBagellerId(id);
			if (item == null) return RedirectToAction("Index");

			return View(new BagellerEditModel {Item = item});
		}
		[HttpPost]
		public ActionResult Edit(int id, BagellerEditModel model, FormCollection collection)
		{
			if (ModelState.IsValid)
			{
				/*var documentStore = new DocumentStore { ConnectionStringName = "RavenDB" };
				documentStore.Initialize();
				using (var session = documentStore.OpenSession())
				{
					var item = session.Load<Bageller>(model.Item.Id);
					item.Name = model.Item.Name;
					session.SaveChanges();
				}*/
				model.Item = _bagellerService.FetchByBagellerId(id);
				TryUpdateModel(model);
				
				_bagellerService.Save(model.Item);

				return RedirectToAction("Index");
			}
			return View(model);
		}

		//
		// GET: /Bageller/Delete/5
 
		public ActionResult Delete(int id)
		{
			return View();
		}

		//
		// POST: /Bageller/Delete/5

		[HttpPost]
		public ActionResult Delete(int id, FormCollection collection)
		{
			try
			{
				// TODO: Add delete logic here
 
				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}
	}
}
