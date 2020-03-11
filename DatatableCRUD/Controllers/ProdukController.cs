using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using DatatableCRUD.Models;

namespace DatatableCRUD.Controllers
{
    public class ProdukController : Controller
    {

        // GET: Produk
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetProduk()
        {
            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                var produks = dc.produks.OrderBy(a => a.nama_produk).ToList();
                return Json(new { data = produks }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Save(int id)
        {
            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                var v = dc.produks.Where(a => a.Idproduk == id).FirstOrDefault();
                return View(v);
            }
        }

       



        [HttpPost]
        public ActionResult Save(produk emp)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                using (MyDatabaseEntities dc = new MyDatabaseEntities())
                {
                    if (emp.Idproduk > 0)
                    {
                        //Edit 
                        var v = dc.produks.Where(a => a.Idproduk == emp.Idproduk).FirstOrDefault();
                        if (v != null)
                        {
                            v.nama_produk = emp.nama_produk;
                            v.harga_produk = emp.harga_produk;
                            v.deskripsi = emp.deskripsi;

                        }
                    }
                    else
                    {
                        //Save
                        dc.produks.Add(emp);
                    }
                    dc.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }

       


        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                var v = dc.produks.Where(a => a.Idproduk == id).FirstOrDefault();
                if (v != null)
                {
                    return View(v);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteEmployee(int id)
        {
            bool status = false;
            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                var v = dc.produks.Where(a => a.Idproduk == id).FirstOrDefault();
                if (v != null)
                {
                    dc.produks.Remove(v);
                    dc.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }


        public ActionResult Order()
        {
            return View();
        }

        public ActionResult GetOrder()
        {
            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                var order =  (  from ep in dc.order
                    join e in dc.produks on ep.Idproduk equals e.Idproduk
                    join t in dc.Employees on ep.EmployeeId equals t.EmployeeID
                    select new
                    {
                        Idorder = ep.Idorder,
                        nama_produk = e.nama_produk,
                        nama_pegawai = t.FirstName,
                        deskripsi = ep.deskripsi 
                    }).ToList();
                return Json(new { data = order }, JsonRequestBehavior.AllowGet);
            }
        }





        [HttpGet]
        public ActionResult ordersave(int id)
        {
            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                ViewBag.CategoryList = GetProduk2();
                ViewBag.EmployeeList = GetEmployee();

                var v = dc.order.Where(a => a.Idorder == id).FirstOrDefault();
                return View(v);
            }
        }





        [HttpPost]
        public ActionResult ordersave(order emp)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                using (MyDatabaseEntities dc = new MyDatabaseEntities())
                {
                    if (emp.Idorder > 0)
                    {
                        //Edit 
                        var v = dc.order.Where(a => a.Idorder == emp.Idorder).FirstOrDefault();
                        if (v != null)
                        {
                            v.Idorder = emp.Idorder;
                            v.Idproduk = emp.Idproduk;
                            v.EmployeeId = emp.EmployeeId;
                            v.deskripsi = emp.deskripsi;

                        }
                    }
                    else
                    {
                        //Save
                        dc.order.Add(emp);
                    }
                    dc.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }

        public List<SelectListItem> GetProduk2()
        {
            
            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                List<SelectListItem> list = new List<SelectListItem>();
                var produks = dc.produks.OrderBy(a => a.nama_produk).ToList();
                foreach (var item in produks)
                {
                    list.Add(new SelectListItem { Value = item.Idproduk.ToString(), Text = item.nama_produk });
                }
                return list;
            }
        }


        public List<SelectListItem> GetEmployee()
        {

            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                List<SelectListItem> list = new List<SelectListItem>();
                var employees = dc.Employees.OrderBy(a => a.EmployeeID).ToList();
                foreach (var item in employees)
                {
                    list.Add(new SelectListItem { Value = item.EmployeeID.ToString(), Text = item.EmailID });
                }
                return list;
            }
        }





        [HttpGet]
        public ActionResult DeleteOrder(int id)
        {
            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                var v = dc.order.Where(a => a.Idorder == id).FirstOrDefault();
                if (v != null)
                {
                    return View(v);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        [ActionName("DeleteOrder")]
        public ActionResult DeleteEmployee2(int id)
        {
            bool status = false;
            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                var v = dc.order.Where(a => a.Idorder == id).FirstOrDefault();
                if (v != null)
                {
                    dc.order.Remove(v);
                    dc.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }

        public ActionResult Export()
        {
            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {

                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Reports/CrystalReport.rpt")));
                rd.SetDataSource(dc.produks.OrderBy(a => a.Idproduk).ToList());
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();
                try
                {
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "aplication/pdf", "Produk_list.pdf");
                }
                catch (Exception e)
                {
                    
                    throw;
                }
            }
        }
        




    }
}