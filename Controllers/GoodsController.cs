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
    public class GoodsController : Controller
    {
        private readonly StoreContext _context;
        private readonly IWebHostEnvironment _enc;
        public GoodsController(StoreContext context, IWebHostEnvironment enc)
        {
            _context = context;
            _enc = enc;
        }
       

        // GET: Goods
        public async Task<IActionResult> Index()
        {
            return View(await _context.Goods.ToListAsync());
        }

        // GET: Goods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goods = await _context.Goods
                .FirstOrDefaultAsync(m => m.GoodsId == id);
            if (goods == null)
            {
                return NotFound();
            }

            return View(goods);
        }

        // GET: Goods/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Goods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GoodsId,GoodsName,GoodsImage,Price,ImageUpload")] Goods goods)
        {
            if (goods.ImageUpload != null)
            {



                goods.GoodsImage = "\\Image\\" + goods.ImageUpload.FileName;


                string serverPath = _enc.WebRootPath + goods.GoodsImage;


                using FileStream stream = new FileStream(serverPath, FileMode.Create);


                await goods.ImageUpload.CopyToAsync(stream);


            }


            if (ModelState.IsValid)
            {
                _context.Add(goods);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(goods);
        }

        // GET: Goods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goods = await _context.Goods.FindAsync(id);
            if (goods == null)
            {
                return NotFound();
            }
            return View(goods);
        }

        // POST: Goods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GoodsId,GoodsName,GoodsImage,Price,ImageUpload")] Goods goods)
        {
            if (id != goods.GoodsId)
            {
                return NotFound();
            }
            if (goods.ImageUpload != null)
            {



                goods.GoodsImage = "\\Image\\" + goods.ImageUpload.FileName;


                string serverPath = _enc.WebRootPath + goods.GoodsImage;


                using FileStream stream = new FileStream(serverPath, FileMode.Create);


                await goods.ImageUpload.CopyToAsync(stream);


            }


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(goods);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GoodsExists(goods.GoodsId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(goods);
        }

        // GET: Goods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goods = await _context.Goods
                .FirstOrDefaultAsync(m => m.GoodsId == id);
            if (goods == null)
            {
                return NotFound();
            }

            return View(goods);
        }

        // POST: Goods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var goods = await _context.Goods.FindAsync(id);
            if (goods != null)
            {
                _context.Goods.Remove(goods);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GoodsExists(int id)
        {
            return _context.Goods.Any(e => e.GoodsId == id);
        }
    }
}
