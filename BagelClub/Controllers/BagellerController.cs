using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BagelClub.Models;
using BagelClub.Services;
using BagelClub.ViewModels;
using Laughlin.Common.Extensions;

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
			var item = id == 0 ? new Bageller() : _bagellerService.FetchByBagellerId(id);
			if (item == null) return RedirectToAction("Index");

			var model = new BagellerEditModel {Item = item, Locations = GetLocationSelectList(item)};
			return View(model);
		}
		[HttpPost]
		public ActionResult Edit(int id, BagellerEditModel model, FormCollection collection)
		{
			if (ModelState.IsValid)
			{
				model.Item = _bagellerService.FetchByBagellerId(id);
				TryUpdateModel(model);
				
				_bagellerService.Save(model.Item);

				return RedirectToAction("Index");
			}
			model.Locations = GetLocationSelectList(model.Item);
			return View(model);
		}

		private static IEnumerable<SelectListItem> GetLocationSelectList(Bageller item)
		{
			var items = (from BagelShopType location in Enum.GetValues(typeof (BagelShopType))
			                          select new SelectListItem
			                                 	{
			                                 		Text = location.GetName(),
			                                 		Value = ((int) location).ToString(),
			                                 		Selected = (int) location == item.PurchaseLocation.LocationId
			                                 	}).ToList();
			items.Insert(0, new SelectListItem {Text = "No Preference", Value = "0"});
			return items;
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
