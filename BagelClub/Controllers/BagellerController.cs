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

		public ActionResult Edit(int id)
		{
			var item = id == 0 ? new Bageller() : _bagellerService.FetchByBagellerId(id);
			if (item == null) return RedirectToAction("Index");

			var model = new BagellerEditModel
			            	{
			            		Item = item,
			            		Locations = GetLocationSelectList(item),
			            		ChoiceLocations = GetLocationSelectList()
			            	};
			return View(model);
		}
		[HttpPost]
		public ActionResult Edit(int id, BagellerEditModel model, FormCollection collection)
		{
			if (ModelState.IsValid)
			{
				model.Item = _bagellerService.FetchByBagellerId(id);
				TryUpdateModel(model);
				
				var item = _bagellerService.Save(model.Item);
				if (id == 0)
				{
					new MailController().WelcomeEmail(new WelcomeEmailModel {Bageller = item}).Deliver();
					return RedirectToAction("Index", "Home");
				}

				return RedirectToAction("Index");
			}
			model.Locations = GetLocationSelectList(model.Item);
			model.Locations = GetLocationSelectList();
			return View(model);
		}

		private static IEnumerable<SelectListItem> GetLocationSelectList(Bageller item = null)
		{
			var items = (from BagelShopType location in Enum.GetValues(typeof (BagelShopType))
			                          select new SelectListItem
			                                 	{
			                                 		Text = location.GetName(),
			                                 		Value = ((int) location).ToString(),
			                                 		Selected = item != null && item.PurchaseLocation != null && (int) location == item.PurchaseLocation.LocationId
			                                 	}).ToList();
			if (item != null)
				items.Insert(0, new SelectListItem {Text = "No Preference", Value = "0"});
			return items;
		}

		public ActionResult Delete(int id)
		{
			var item = _bagellerService.Delete(id);
			TempData["Message"] = "{0} deleted".FormatWith(item.Name);
			return RedirectToAction("Index");
		}

		public ActionResult SkipWeek(int id)
		{
			_bagellerService.ResetNextPurchaseDates(id);
			return RedirectToAction("Index");
		}
	}
}
