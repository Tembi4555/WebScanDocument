using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebScanDocument.Models;
using System.Data.Entity;

namespace WebScanDocument.Controllers
{
    /// <summary>
    /// Работа с сотрудниками
    /// </summary>
    public class WorkerController : Controller
    {
        public DbDocContext db = new DbDocContext();

        /// <summary>
        /// Список всех работников
        /// </summary>
        [HttpGet]
        public ActionResult ListWorker()
        {
            return View(db.Workers);
        }

        /// <summary>
        /// Создать работника
        /// </summary>
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Worker worker)
        {
            if (ModelState.IsValid)
            {
                db.Workers.Add(worker);
                db.SaveChanges();
                return RedirectToAction("ListWorker");
            }

            return View(worker);
        }

        /// <summary>
        /// Изменение данных о работнике
        /// </summary>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Worker worker = db.Workers.Find(id);

            return View(worker);
        }

        [HttpPost]
        public ActionResult Edit(Worker worker)
        {
            if (ModelState.IsValid)
            {
                db.Entry(worker).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ListWorker");
            }

            return View(worker);
        }

        /// <summary>
        /// Удаление работника
        /// </summary>
        /// 
        public ActionResult Delete(int id)
        {
            Worker worker = db.Workers.Find(id);
            db.Workers.Remove(worker);
            db.SaveChanges();
            return RedirectToAction("ListWorker");
        }

        /// <summary>
        /// Поиск по табельному номеру и ФИО сотрудника
        /// </summary>

        [HttpPost]
        public ActionResult Search(string name)
        {

            var allWorkers = db.Workers.Where(a => a.PersNumber.ToString().Contains(name) || a.FIO.Contains(name)).ToList();
            if (allWorkers.Count <= 0)
            {
                return HttpNotFound("Ничего не найдено");
            }
            return PartialView(allWorkers);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}