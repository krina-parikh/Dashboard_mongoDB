using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using MongoDB.Driver.Builders;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Data;
using System.Web.Script.Serialization;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Serializers;
//using MongoDB.Bson.Serialization;
using System.Globalization;
using Dashboard.Helpers;
using Dashboard.Models;

namespace Dashboard.Controllers
{
    public class HomeController : Controller
    {
        public static int companyId = 0;
        public static bool flag = false;
        public static int catgryId = 0;
        public static int salesOption = 1;
        public static DateTime startDate = DateTime.Now.AddMonths(-6);
        public static DateTime endDate = DateTime.Now;
        public static int catgCompany;
        public static string cmpnyName = "Company";
        public static decimal tsales, percent; public static int[] cnt = new int [3];
        public ConnectDB obj;
       
        public HomeController()
        {
            obj = new ConnectDB();
        }

        public ActionResult Index()
        {
            
            var compny = (from c in obj.collectionCmpy.AsQueryable() 
                          where(c.COMPANYSTATUS.Equals("Active") && c.ISDELETED.Equals(false)) 
                           select new {c.COMPANYNAME, c.COMPANYID}).ToList();
            
            ViewBag.LIST = new SelectList(compny.AsEnumerable(),"COMPANYID","COMPANYNAME");
            
            return View();
        }

        public JsonResult Report()
        {
            DateTime today = DateTime.Now;
            DateTime previous = DateTime.Now.AddDays(-1);
            
                var cal = (from s in obj.collectionSI.AsQueryable()
                           where (s.INVOICEDATE >= previous.Date && s.INVOICEDATE <= today.Date)
                              select new {s.INVOICEDATE,s.INVOICEAMOUNT,s.CUSTOMERID}).ToList();
            
            if(companyId>0){
                cal = (from s in obj.collectionSI.AsQueryable()
                          where (s.INVOICEDATE >= previous && s.COMPANYID==companyId)
                              select new {s.INVOICEDATE,s.INVOICEAMOUNT,s.CUSTOMERID}).ToList();
            }
            var query = (from c in cal group c by new {c.INVOICEDATE.Date} into g
                         select new {INVOICEAMOUNT = g.Sum(z=>z.INVOICEAMOUNT), g.Key.Date}).ToList();

            decimal todaySales = query.Where(z=>z.Date == today).Sum(z=>z.INVOICEAMOUNT);
            decimal presales= query.Where(z=>z.Date == previous).Sum(z=>z.INVOICEAMOUNT);
            decimal t = (todaySales - presales)/presales*100;
            percent = Math.Round(t, 2);
            int[] tday=new int[2];
            tday[0] = cal.Where(z=>z.INVOICEDATE == today).Count();
            tday[1] = cal.Where(z=>z.INVOICEDATE == today).Select(z => z.CUSTOMERID).Distinct().Count();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SalesType(string option)
        {
            salesOption = Convert.ToInt16(option);
            return Json(salesOption, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CategoryList(int cmpny)
        {
            catgCompany = cmpny;
            var catgry = (from c in obj.collectionCtgy.AsQueryable()
                           where (c.ISSECONDARY.Equals(false) && c.ISDELETED.Equals(false) && c.COMPANYID == cmpny) 
                           select new { c.CATEGORYNAME, c.ITEMCATEGORYID }).ToList();
            ViewBag.Ctgry = new SelectList(catgry.AsEnumerable(),"ITEMCATEGORYID","CATEGORYNAME");
            return Json(catgry, JsonRequestBehavior.AllowGet);
        }


        public PartialViewResult Partial(string name)
        {
            if (name.Equals("Sales"))
            {
                if (salesOption == 1 || salesOption == 3)
                    ViewBag.option = "Sales";
                else
                    ViewBag.option = "Weekly";
            }
            else
                return PartialView("TopItems");
           return PartialView("SubIndex");
        }

        public ActionResult DropdownList(string id)
        {
            string comp = Convert.ToString(id);
            companyId = Convert.ToInt32(comp);
            return Json(companyId, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CategoryDList(string id)
        {
            string str = Convert.ToString(id);
            catgryId = Convert.ToInt32(str);
            flag = true;
            return Json(companyId, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult getDate(string sd, string ed)
        {
            startDate = Convert.ToDateTime(sd);
            endDate = Convert.ToDateTime(ed);
            string dd = "01 January, 2001";
            DateTime xyz = Convert.ToDateTime(dd).Date;
           // salesOtpion = Convert.ToInt32(option);
            return Json(startDate, JsonRequestBehavior.AllowGet);
        }

        public Dictionary<DateTime,decimal> MonthlySales()
        {
            startDate = new DateTime(startDate.Year, startDate.Month, 1);
           
            endDate = new DateTime(endDate.Year, endDate.Month, (DateTime.DaysInMonth(endDate.Year, endDate.Month)));

            int yr = endDate.Year;
            var mapping = new Dictionary<DateTime, decimal>();
            var data = from r in obj.collectionSI.AsQueryable()
                       where (r.INVOICEDATE >= startDate && r.INVOICEDATE <= endDate)
                       select new { r.INVOICEID, r.INVOICEDATE, r.INVOICEAMOUNT, r.CUSTOMERID };

            if (companyId != 0)
            {
                data = from r in obj.collectionSI.AsQueryable()
                       where (r.COMPANYID == companyId && r.INVOICEDATE >= startDate && r.INVOICEDATE <= endDate)
                       select new { r.INVOICEID, r.INVOICEDATE, r.INVOICEAMOUNT, r.CUSTOMERID };
            }
            tsales = data.Sum(z => z.INVOICEAMOUNT);
            cnt[0] = data.Select(z => z.CUSTOMERID).Distinct().Count();
            cnt[1] = data.Count();
            foreach (var x in data)
            {
               var y = new DateTime(x.INVOICEDATE.Year, x.INVOICEDATE.Month, 1);
                yr = (x.INVOICEDATE.Month * 10000) + x.INVOICEDATE.Year;
                if (mapping.ContainsKey(y))
                {
                    mapping[y] = mapping[y] + x.INVOICEAMOUNT;
                }
                else
                {
                    mapping.Add(y, x.INVOICEAMOUNT);
                }
            }

            return mapping;
        }

        public JsonResult DailySales()
        {
            var sales = new Dictionary<DateTime, decimal>();
            if(salesOption == 3){
               var result = MonthlySales();
               var d = from r in result select new { INVOICEDATE = r.Key, INVOICEAMOUNT = r.Value };
                return Json(d.ToList(), JsonRequestBehavior.AllowGet);
            }
            var query = (from r in obj.collectionSI.AsQueryable()
                         where (r.INVOICEDATE >= startDate && r.INVOICEDATE <= endDate)
                         orderby r.INVOICEDATE
                         select new { r.INVOICEAMOUNT, r.INVOICEDATE, r.COMPANYID, r.CUSTOMERID, r.INVOICEID }).ToList();

            if (companyId > 0)
            {
                var tst = (from r in query
                           where r.COMPANYID == companyId
                           group r by new { r.INVOICEDATE.Date, r.COMPANYID } into g
                           select new { INVOICEDATE = g.Key.Date, INVOICEAMOUNT = g.Sum(z => z.INVOICEAMOUNT), g.Key.COMPANYID }).ToList();
                tsales = tst.Sum(z => z.INVOICEAMOUNT);
                cnt[0] = query.Where(z=>z.COMPANYID == companyId).Select(z => z.CUSTOMERID).Distinct().Count();
                cnt[1] = query.Where(z => z.COMPANYID == companyId).Count();
                return Json(tst, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var test = (from q in query
                            group q by new { q.COMPANYID, q.INVOICEDATE.Date } into g
                            orderby g.Key.Date
                            select new { g.Key.COMPANYID, INVOICEDATE = g.Key.Date, INVOICEAMOUNT = g.Sum(z => z.INVOICEAMOUNT) }).ToList();
                tsales = test.Sum(z => z.INVOICEAMOUNT);
                cnt[0] = query.Select(z => z.CUSTOMERID).Distinct().Count();
                cnt[1] = query.Count();
                return Json(test, JsonRequestBehavior.AllowGet);
            }
        }

        [OutputCache(Duration= 36000)]
        public JsonResult OutStandingBalance()
        {
            var invoice = (from s in obj.collectionSI.AsQueryable()
                           select new { s.INVOICEID, s.INVOICEAMOUNT, s.COMPANYID}).ToList();

            var Y = from i in invoice let amt = Convert.ToDouble(i.INVOICEAMOUNT) where i.INVOICEAMOUNT > 0 select i;
          
            var test = (from si in Y
                        join pt in obj.payment.AsQueryable() on si.INVOICEID equals pt.INVOICEID
                        select new {si.INVOICEAMOUNT, si.COMPANYID, pt.AMOUNT }).ToList();

            var test_1 = (from ts in test
                          join c in obj.collectionCmpy.AsQueryable() on ts.COMPANYID equals c.COMPANYID
                          group ts by new { ts.COMPANYID, c.COMPANYNAME } into grp
                          orderby grp.Key.COMPANYID
                         select new { grp.Key.COMPANYID, Value = (grp.Sum(z=>z.INVOICEAMOUNT) + grp.Sum(z=>z.AMOUNT)), grp.Key.COMPANYNAME}).ToList();
        
            return Json(test_1, JsonRequestBehavior.AllowGet);
        }

        public ActionResult withCategory()
        {
            DateTime eD = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddYears(-1);
            //SalesInvoice
            var detail = (from d in obj.collectionSI.AsQueryable()
                          orderby d.INVOICEID descending
                          where (d.COMPANYID == catgCompany && d.INVOICEDATE >= eD)
                          select new { d.INVOICEID, d.COMPANYID }).Take(2000).ToList();
            
            var items = (from i in obj.collectionItem.AsQueryable()
                         where i.ITEMCATEGORYID == catgryId
                         select new { i.ITEMCATEGORYID, i.ITEMNAME, i.ITEMID }).ToList();

            var x = (from w in obj.collectionDI.AsQueryable()
                    orderby w.INVOICEID descending
                    select new { w.ITEMID, w.ORDERQUANTITY, w.ACTUALPRICE, w.INVOICEID }).Take(10000).ToList();
            var invoice = (from d in detail 
                           join si in x on d.INVOICEID equals si.INVOICEID
                           join i in items on si.ITEMID equals i.ITEMID
                           select new { si.ITEMID, si.ORDERQUANTITY, si.INVOICEID, si.ACTUALPRICE, i.ITEMNAME }).ToList();

            var category = (from c in obj.collectionCtgy.AsQueryable() where c.ITEMCATEGORYID == catgryId
                            select new { c.CATEGORYNAME, c.ITEMCATEGORYID }).ToList();

            var gsum = from q in invoice
                         group q by new {q.ITEMID, q.ITEMNAME } into g
                         select new { TOTALQuantity = g.Sum(t => t.ORDERQUANTITY), g.Key.ITEMID, g.Key.ITEMNAME, SALEs = g.Sum(s => s.ACTUALPRICE) };

            var data = (from r in gsum from z in category orderby r.TOTALQuantity descending
                           select new {r.ITEMNAME, r.TOTALQuantity, r.SALEs, z.CATEGORYNAME}).Take(10);

            return Json(data.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult withOutCategory()
        {
            var invoice = (from i in obj.collectionDI.AsQueryable()
                           where i.ORDERQUANTITY > 0 
                           orderby i.INVOICEITEMID descending 
                           select new { i.ITEMID, i.ORDERQUANTITY, i.INVOICEID, i.ACTUALPRICE }).Take(5000).ToList();
       
            var mongo = (from s in invoice
                         group s by new { s.ITEMID} into g
                         select new { g.Key.ITEMID, TOTALQuantity = g.Sum(z => z.ORDERQUANTITY), 
                             SALEs = g.Sum(z => z.ACTUALPRICE) }).ToList();
            var test = (from m in mongo
                       join i in obj.collectionItem.AsQueryable() on m.ITEMID equals i.ITEMID
                           join c in obj.collectionCtgy.AsQueryable() on i.ITEMCATEGORYID equals c.ITEMCATEGORYID
                           orderby m.TOTALQuantity descending
                           select new {c.CATEGORYNAME, i.ITEMNAME, m.SALEs, m.TOTALQuantity }).Take(10).ToList();
         
            return Json(test, JsonRequestBehavior.AllowGet);
        }

        public JsonResult WeeklySales()
        {
            var compny = (from c in obj.collectionCmpy.AsQueryable()
                           where (c.COMPANYSTATUS.Equals("Active") && c.ISDELETED.Equals(false))
                           select new { c.COMPANYNAME, c.COMPANYID }).ToList();
            var dateRange = new Dictionary<string, DateTime>();     //week start-date
            var end = new Dictionary<string, DateTime>();           //Week end-date
            var mapping = new Dictionary<string, decimal>();        //weekly sales amt
            var data = (from r in obj.collectionSI.AsQueryable()
                       where (r.INVOICEDATE >= startDate && r.INVOICEDATE <= endDate)
                       orderby r.INVOICEDATE
                       select new { r.INVOICEDATE, r.INVOICEAMOUNT, r.COMPANYID, r.CUSTOMERID }).ToList();
            var temp = data;
            if (companyId > 0)
            {
                data = (from r in temp
                           where (r.COMPANYID == companyId)
                           orderby r.INVOICEDATE
                       select new { r.INVOICEDATE, r.INVOICEAMOUNT, r.COMPANYID, r.CUSTOMERID }).ToList();
            }
            tsales = data.Sum(z => z.INVOICEAMOUNT);
            cnt[0] = data.Select(z => z.CUSTOMERID).Distinct().Count();
            cnt[1] = data.Count();
            int n = data.Count();
            int count = 0;
            decimal amount = 0;
            var day = startDate.AddDays(6);
                foreach( var x in data)
                {
                    n--;
                    DateTime d = x.INVOICEDATE.Date;
                    if (d <= day)
                    {
                        amount += x.INVOICEAMOUNT;
                    }
                    else
                    {
                        count++;
                        string str = "Week" + " " + Convert.ToString(count);
                        mapping.Add(str, amount);
                        dateRange.Add(str,day.AddDays(-6));
                        end.Add(str,day);
                        amount = 0;
                        day = day.AddDays(7);
                        if(d<=day)
                            amount += x.INVOICEAMOUNT;
                    }
                    if (n == 0)
                    {
                        count++;
                        string str = "Week" + " " + Convert.ToString(count);
                        mapping.Add(str, amount);
                        dateRange.Add(str, day.AddDays(-6));
                        end.Add(str, day);
                    }
                        
                }
               
            var final = (from m in mapping join d in dateRange on m.Key equals d.Key join e in end on m.Key equals e.Key
                        select new { m.Key, m.Value, Start = d.Value, End = e.Value });
            return Json(final.ToList(), JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration= 36000, VaryByParam = "cmpy")]
        public JsonResult customerDue(int cmpy)
        {
            var invoice = (from c in obj.collectionSI.AsQueryable() where (c.COMPANYID == cmpy )
                          select new {c.INVOICEID, c.CUSTOMERID, c.INVOICEAMOUNT }).ToList();

            var payment = (from p in obj.payment.AsQueryable() where p.INVOICEID > 0
                            select new { AMOUNT = (p.AMOUNT + p.DISCOUNT * (-1)), p.INVOICEID }).ToList();

            var pay = (from x in payment
                       let amount = x.AMOUNT
                       where amount < 0
                       group x by new { x.INVOICEID} into g
                       select new {g.Key.INVOICEID, AMOUNT = g.Sum(a=>a.AMOUNT)}).ToList();

            var Jquery = (from i in invoice
                          let d = i.INVOICEAMOUNT
                          join p in pay on i.INVOICEID equals p.INVOICEID
                          where d > 0
                          select new {i.CUSTOMERID, p.AMOUNT, i.INVOICEAMOUNT, DUE = (p.AMOUNT + i.INVOICEAMOUNT) }); 

            var Gquery = ( from j in Jquery
                        group j by new { j.CUSTOMERID } into g
                        select new { g.Key.CUSTOMERID, Paid = g.Sum( z => z.AMOUNT ), INVOICEAMOUNT = g.Sum( z => z.INVOICEAMOUNT ), 
                         UnPaid = g.Sum( z=>z.DUE )}).ToList();

            var query = (from c in Gquery
                        join
                        cs in obj.customer.AsQueryable() on c.CUSTOMERID equals cs.CUSTOMERID
                        select new {c.CUSTOMERID, cs.CUSTOMERNAME, c.INVOICEAMOUNT, Paid = (c.Paid * -1), c.UnPaid, cs.CUSTOMERCODE }).ToList();

            //InvoiceAmount sum
            ViewBag.Company = cmpy;
            return Json(query,JsonRequestBehavior.AllowGet);
        }

        public JsonResult CustomerInvoice(int id, int cmpy)
        {
            ViewBag.Company = cmpy;
            
            var sales = (from si in obj.collectionSI.AsQueryable() 
                        where (si.CUSTOMERID == id && si.COMPANYID == cmpy) 
                        select new {si.INVOICEID, si.INVOICENUMBER, si.INVOICEAMOUNT,si.PAYMENTSTATUS, si.INVOICEDATE}).ToList();

            var pay = (from p in obj.payment.AsQueryable() where (p.CUSTOMERID == id && p.INVOICEID > 0)
                               select new {p.AMOUNT, p.INVOICEID }).ToList();
            var data = (from x in pay
                       let amt = x.AMOUNT
                       where amt < 0
                       group x by new { x.INVOICEID } into g
                       select new { g.Key.INVOICEID, AMOUNT = g.Sum(z => z.AMOUNT) }).ToList();

            var payment = (from p in data
                           join s in sales on p.INVOICEID equals s.INVOICEID
                           let inv = s.INVOICEAMOUNT where (inv > 0)
                           select new { s.INVOICENUMBER, s.INVOICEID, s.INVOICEAMOUNT, s.PAYMENTSTATUS, p.AMOUNT, s.INVOICEDATE }).ToList();

            var print = (from a in payment
                         select new { a.INVOICENUMBER, a.INVOICEAMOUNT, a.PAYMENTSTATUS, 
                             UnPaid = (a.AMOUNT + a.INVOICEAMOUNT), a.INVOICEID, a.INVOICEDATE }).ToList();
            
            return Json(print, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 36000, VaryByParam = "c")]
        public ActionResult DrillDown(int c, string str)
        {
            cmpnyName = str;
            ViewBag.companyId = c;
            return View("display");
        }

        public JsonResult getClientName(int id)
        {
            var name = (from c in obj.customer.AsQueryable() where c.CUSTOMERID == id select new { c.CUSTOMERNAME }).ToList();
            return Json(name, JsonRequestBehavior.AllowGet);
        }

        public JsonResult salesSummary()
        {
            int x = 3;
            return Json(x, JsonRequestBehavior.AllowGet);
        }
    }
}                           
