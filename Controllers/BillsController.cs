using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCExam.Models;

namespace MVCExam.Controllers
{
    public class BillsController : Controller
    {
        private readonly StoreContext _context;

        public BillsController(StoreContext context)
        {
            _context = context;
        }

        // GET: Bills
        public async Task<IActionResult> Index()
        {
            var data = await _context.Bills.Include(i => i.Items).ThenInclude(p => p.Goods).ToListAsync();


            ViewBag.Count = data.Count;
            ViewBag.GrandTotal = data.Sum(i => i.Items.Sum(l => l.ItemTotal));

            //ViewBag.Average = data.Average(i=> i.Items.Sum(l=> l.ItemTotal)) ;

            ViewBag.Average = data.Count > 0 ? data.Average(i => i.Items.Sum(l => l.ItemTotal)) : 0;


            //if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            //{
            //    return PartialView(data);
            //}
            return View(data);
        }

        // GET: Bills/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bill = await _context.Bills.Include(i => i.Items).ThenInclude(p => p.Goods)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bill == null)
            {
                return NotFound();
            }


            return View(bill);
        }

        // GET: Bills/Create
        public IActionResult Create()
        {
            return View(new Bill());
        }

        // POST: Bills/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BillDate,CustomerName,Address,ContactNo, Items")] Bill bill, string command = "")
        {
            if (command == "Add")
            {
               bill.Items.Add(new());
                return View(bill);
            }
            else if (command.Contains("delete"))// delete-3-sdsd-5   ["delete", "3"]
            {
                int idx = int.Parse(command.Split('-')[1]);

                bill.Items.RemoveAt(idx);
                ModelState.Clear();
                return View(bill);
            }


            if (ModelState.IsValid)
            {
                _context.Add(bill);
                await _context.SaveChangesAsync();
                var rows = await _context.Database.ExecuteSqlRawAsync("exec SpInsertBill @p0, @p1, @p2, @p3", bill.BillDate, bill.CustomerName, bill.Address, bill.ContactNo);


                if (rows > 0)
                {
                    bill.Id = _context.Bills.Max(x => x.Id);

                    foreach (var item in bill.Items)
                    {
                        await _context.Database.ExecuteSqlRawAsync("exec SpInsertBillItem @p0, @p1, @p2", bill.Id, item.GoodsId, item.Quantity);
                    }
                }




                return RedirectToAction(nameof(Index));
            }
            return View(bill);
        }

        // GET: Bills/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bill = await _context.Bills.Include(i => i.Items).ThenInclude(p => p.Goods).FirstOrDefaultAsync(i => i.Id == id);
            if (bill == null)
            {
                return NotFound();
            }
            return View(bill);
        }

        // POST: Bills/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BillDate,CustomerName,Address,ContactNo, Items")] Bill bill, string command = "")
        {
            if (command == "Add")
            {
                bill.Items.Add(new());
                return View(bill);
            }
            else if (command.Contains("delete"))// delete-3   ["delete", "3"]
            {
                int idx = int.Parse(command.Split('-')[1]);

                bill.Items.RemoveAt(idx);
                ModelState.Clear();
                return View(bill);
            }
           

            if (id != bill.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(bill);
                    //await _context.SaveChangesAsync();

                    var row = await _context.Database.ExecuteSqlRawAsync("exec SpUpdateBill @p0, @p1, @p2, @p3, @p4", bill.Id, bill.BillDate, bill.CustomerName, bill.Address, bill.ContactNo);


                    foreach (var item in bill.Items)
                    {
                        await _context.Database.ExecuteSqlRawAsync("exec SpInsertBillItem @p0, @p1, @p2", bill.Id, item.GoodsId, item.Quantity);
                    }

                    return RedirectToAction(nameof(Index));

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BillExists(bill.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction(nameof(Index));
            }
            return View(bill);
        }

        // GET: Bills/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bill = await _context.Bills.Include(i => i.Items).ThenInclude(p => p.Goods)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bill == null)
            {
                return NotFound();
            }

            return View(bill);
        }

        // POST: Bills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var bill = await _context.Bills.FindAsync(id);
            //if (bill != null)
            //{
            //    _context.Bills.Remove(bill);
            //}
            await _context.Database.ExecuteSqlAsync($"exec SpDeleteBill {id}");


            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAjax(int id)
        {
            //var invoice = await _context.Invoices.FindAsync(id);
            //if (invoice != null)
            //{             

            //    //_context.Invoices.Remove(invoice);
            //}
            await _context.Database.ExecuteSqlAsync($"exec spDeleteBill {id}");

            //await _context.SaveChangesAsync();

            return Ok();
        }

        private bool BillExists(int id)
        {
            return _context.Bills.Any(e => e.Id == id);
        }
    }
}
