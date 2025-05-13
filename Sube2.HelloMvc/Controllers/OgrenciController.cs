using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sube2.HelloMvc.Models;
using Sube2.HelloMvc.Models.ViewModels;

namespace Sube2.HelloMvc.Controllers
{
    public class OgrenciController : Controller
    {
        private readonly OkulDbContext _context;

        public OgrenciController(OkulDbContext context)
        {
            _context = context;
        }

        public ViewResult Index()//Action
        {
            return View("AnaSayfa");
        }

        public ViewResult OgrenciListe()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetOgrenciList()
        {
            var lst = _context.Ogrenciler.ToList();
            return Json(lst);
        }

        [HttpGet]
        public ViewResult OgrenciEkle()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AddOgrenci(Ogrenci ogr)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Ogrenciler.Add(ogr);
                    int sonuc = _context.SaveChanges();
                    return Json(new { success = sonuc > 0, message = sonuc > 0 ? "Öğrenci başarıyla eklendi." : "Öğrenci eklenirken bir hata oluştu." });
                }
                return Json(new { success = false, message = "Form verileri geçersiz." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
        }

        [HttpPost]
        public ViewResult OgrenciEkle(Ogrenci ogr)
        {
            int sonuc = 0;
            if (ogr != null)
            {
                _context.Ogrenciler.Add(ogr);
                sonuc = _context.SaveChanges();
            }

            if (sonuc > 0)
            {
                TempData["sonuc"] = true;
            }
            else
            {
                TempData["sonuc"] = false;
            }
            return View();
        }

        [HttpGet]
        public IActionResult OgrenciDetay(int id)
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetOgrenci(int id)
        {
            var ogr = _context.Ogrenciler.Find(id);
            if (ogr == null)
            {
                return Json(new { success = false, message = "Öğrenci bulunamadı." });
            }
            return Json(new { success = true, data = ogr });
        }

        [HttpPost]
        public JsonResult UpdateOgrenci(Ogrenci ogr)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Entry(ogr).State = EntityState.Modified;
                    int sonuc = _context.SaveChanges();
                    return Json(new { success = sonuc > 0, message = sonuc > 0 ? "Öğrenci başarıyla güncellendi." : "Güncelleme sırasında bir hata oluştu." });
                }
                return Json(new { success = false, message = "Form verileri geçersiz." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult OgrenciDetay(Ogrenci ogr)
        {
            _context.Entry(ogr).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("OgrenciListe");
        }

        [HttpPost]
        public JsonResult DeleteOgrenci(int id)
        {
            try
            {
                var ogr = _context.Ogrenciler.Find(id);
                if (ogr == null)
                {
                    return Json(new { success = false, message = "Öğrenci bulunamadı." });
                }

                _context.Ogrenciler.Remove(ogr);
                int sonuc = _context.SaveChanges();
                return Json(new { success = sonuc > 0, message = sonuc > 0 ? "Öğrenci başarıyla silindi." : "Silme işlemi sırasında bir hata oluştu." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
        }

        public IActionResult OgrenciSil(int id)
        {
            var ogr = _context.Ogrenciler.Find(id);
            _context.Ogrenciler.Remove(ogr);
            _context.SaveChanges();
            return RedirectToAction("OgrenciListe");
        }
    }
}

//Controller'dan View'e veri taşıma
//1-ViewData: Key-Value Pair Collection. Key'ler mutlaka tekil olmalıdır.Key'ler string, Value'lar object'dir. Type-Safe değildir.
//2-ViewBag: Arka planda ViewData dictionary'sini kullanır. Bu durumda daha önce ViewData'larda kullanılan key'ler kullanılamaz.
//ViewBag'ler dynamic yapılardır ve içindeki türler Runtime sırasında tespit edilir.

//3-Model
//4-TempData**