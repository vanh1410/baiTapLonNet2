using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThuVienOnline.Models;

namespace ThuVienOnline.Controllers
{
    public class BookController : Controller
    {
        private readonly ThuVienOnLineContext _context;

        public BookController(ThuVienOnLineContext context)
        {
            _context = context;
        }

        // GET: Book
        public async Task<IActionResult> Index()
        {
            return View(await _context.Book.ToListAsync());
        }

        // GET: Book/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Book/Create
        public IActionResult Create()
        {
            ViewData["TheLoaiID"] = new SelectList(_context.TheLoai, "TheLoaiID", "TheLoaiName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookId,TheLoaiID,BookName,TacGia,Anh,GiaSp,NgayPh,SoTrang,Mota")] Book book)
        {
            if (ModelState.IsValid)
            {
                
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TheLoaiID"] = new SelectList(_context.TheLoai, "TheLoaiID", "TheLoaiName", book.TheLoaiID);
            return View(book);
        }

        // GET: Book/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["TheLoaiID"] = new SelectList(_context.TheLoai, "TheLoaiID", "TheLoaiName", book.TheLoaiID);
            return View(book);
        }

        // POST: Book/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookId,TheLoaiID,BookName,TacGia,Anh,GiaSp,NgayPh,SoTrang,Mota")] Book book)
        {
            if (id != book.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookId))
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
            return View(book);
        }

        // GET: Book/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Book.FindAsync(id);
            _context.Book.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("UserId,FullName,Ngaysinh,GioiTinh,Anh,Diachi,Phone,Email,Password")] NguoiDung nguoiDung)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(nguoiDung.FullName) == true ||nguoiDung.GioiTinh == null || string.IsNullOrEmpty(nguoiDung.Email) == true || nguoiDung.Phone == null || nguoiDung.Ngaysinh == null)
                {
                    ModelState.AddModelError("", "Thông tin không được để trống");
                    return View(nguoiDung);
                }
                var checkEmail = _context.NguoiDung.SingleOrDefault(x => x.Email.Trim().ToLower() == nguoiDung.Email.Trim().ToLower());
                if (checkEmail != null)
                {
                    ModelState.AddModelError("", "Địa chỉ Email đã tồn tại");
                    return View(nguoiDung);
                }
                var checkPhone = _context.NguoiDung.SingleOrDefault(x => x.Phone == nguoiDung.Phone);
                if (checkPhone != null)
                {
                    ModelState.AddModelError("", "Số điện thoại đã tồn tại");
                    return View(nguoiDung);
                }

                HttpContext.Session.SetString("MaKh", nguoiDung.UserId.ToString());
                HttpContext.Session.SetString("TenKh", nguoiDung.FullName.ToString());
                HttpContext.Session.SetString("Email", nguoiDung.Email.Trim().ToLower());
                nguoiDung.GioiTinh = true;
                _context.Add(nguoiDung);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();


        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            if (ModelState.IsValid)
            {

                var user = _context.NguoiDung.SingleOrDefault(x => x.Email.Trim().ToLower() == email.Trim().ToLower() && x.Password == password);
                if (user != null)
                {

                    HttpContext.Session.SetString("MaKh", user.UserId.ToString());
                    HttpContext.Session.SetString("TenKh", user.FullName.ToString());
                    HttpContext.Session.SetString("Email", user.Email.Trim().ToLower());
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Đăng Nhập Thất Bại! Kiểm Lại Thông Tin Đăng Nhập");
                    return RedirectToAction("Login");
                }
            }
            return View();
        }

        //Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();//remove session
            return RedirectToAction("Index");
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.BookId == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<string> Upload(IFormFile file)
        {
            if (file != null)
            {
                string fileExtension = Path.GetExtension(file.FileName);
                //rename file when upload to server
                var fileName = DateTime.Now.ToShortTimeString() + fileExtension;
                var filePath = Path.Combine(Directory.GetCurrentDirectory() + "/Upload/Images", fileName);
                var fileLocation = new FileInfo(filePath).ToString();
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    //save file to server
                    await file.CopyToAsync(stream);
                    
                    //save to database
                    //await _context.SaveChangesAsync();
                    return filePath;
                }
            }
            return null;
        }
    }
}
