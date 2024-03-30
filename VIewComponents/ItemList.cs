using Microsoft.AspNetCore.Mvc;
using MVCExam.Models;

namespace MVCExam.VIewComponents
{
    public class ItemList : ViewComponent
    {

        public IViewComponentResult Invoke(List<BillItem> data)
        {

            ViewBag.Count = data.Count;
            ViewBag.Total = data.Sum(i => i.ItemTotal);

            return View(data);
        }

    }

   
}
