using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebScanDocument.Models;

namespace WebScanDocument.Controllers
{
    /// <summary>
    /// Работа с документами
    /// </summary>
    public class ServiceController : Controller
    {
        public DbDocContext db = new DbDocContext();

        #region Работа с видами документов
        [HttpGet]
        public ActionResult TypeOfDocuments()
        {
            ViewBag.TypeOfDocuments = db.TypeOfDocuments;
            return View();
        }

        /// <summary>
        /// Добавление видов документа с последующим их отображением
        /// </summary>
        [HttpPost]
        public ActionResult TypeOfDocuments(TypeOfDocument tod)
        {
            if (ModelState.IsValid)
            {
                db.TypeOfDocuments.Add(tod);
                db.SaveChanges();
            }
            ViewBag.TypeOfDocuments = db.TypeOfDocuments;
            return View(tod);
        }

        /// <summary>
        /// Удаление вида документа по id
        /// </summary>
        public ActionResult DeleteTypeOfDocument(int id)
        {
            TypeOfDocument tod = db.TypeOfDocuments.Find(id);
            db.TypeOfDocuments.Remove(tod);
            db.SaveChanges();
            return RedirectToAction("TypeOfDocuments");
        }

        /// <summary>
        /// Редактирование
        /// </summary>
        [HttpGet]
        public ActionResult EditTypeOfDocument(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            TypeOfDocument tod = db.TypeOfDocuments.Find(id);
            if (tod != null)
            {
                return View(tod);
            }
            return RedirectToAction("TypeOfDocuments");
        }

        [HttpPost]
        public ActionResult EditTypeOfDocument(TypeOfDocument tod)
        {
            db.Entry(tod).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("TypeOfDocuments");
        }
        #endregion

        #region Редактирование документов
        /// <summary>
        /// Просмотр списка документов
        /// </summary>
        public ActionResult ListDocuments()
        {
            var docs = db.ListOfDocuments.Include(a => a.Worker)
                .Include(a => a.TypeOfDocument)
                .ToList();

            List<Worker> workers = db.Workers.ToList();
            workers.Insert(0, new Worker { FIO = "Все", Id = 0 });
            ViewBag.Workers = new SelectList(workers, "Id", "FIO");

            List<TypeOfDocument> tods = db.TypeOfDocuments.ToList();
            tods.Insert(0, new TypeOfDocument { DocumentTypeCode = 1, Id = 0 });
            ViewBag.TypeOfDocuments = new SelectList(tods, "Id", "DocumentTypeCode");

            return View(docs.ToList());
        }
         /// <summary>
         /// Создание нового документа
         /// </summary>
        [HttpGet]
        public ActionResult CreateDocument()
        {
            SelectList workers = new SelectList(db.Workers, "Id", "FIO");
            ViewBag.Workers = workers;
            SelectList tods = new SelectList(db.TypeOfDocuments, "Id", "DocumentTypeCode");
            ViewBag.TypeOfDocuments = tods;

            return View();
        }

        [HttpPost]
        public ActionResult CreateDocument(ListOfDocument listOfDoc)
        {
            listOfDoc.EditingDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.ListOfDocuments.Add(listOfDoc);
                db.SaveChanges();
                return RedirectToAction("ListDocuments");
            }

            SelectList workers = new SelectList(db.Workers, "Id", "FIO");
            ViewBag.Workers = workers;
            SelectList tods = new SelectList(db.TypeOfDocuments, "Id", "DocumentTypeCode");
            ViewBag.TypeOfDocuments = tods;

            return View(listOfDoc);
        }

        /// <summary>
        /// Редактировать документ
        /// </summary>
        [HttpGet]
        public ActionResult EditDocument(int id)
        {
            ListOfDocument listOfDoc = db.ListOfDocuments.Find(id);

            SelectList workers = new SelectList(db.Workers, "Id", "FIO");
            ViewBag.Workers = workers;
            SelectList tods = new SelectList(db.TypeOfDocuments, "Id", "DocumentTypeCode");
            ViewBag.TypeOfDocuments = tods;

            return View(listOfDoc);
        }

        [HttpPost]
        public ActionResult EditDocument(ListOfDocument listOfDoc)
        {
            listOfDoc.EditingDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Entry(listOfDoc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ListDocuments");
            }

            SelectList workers = new SelectList(db.Workers, "Id", "FIO");
            ViewBag.Workers = workers;
            SelectList tods = new SelectList(db.TypeOfDocuments, "Id", "DocumentTypeCode");
            ViewBag.TypeOfDocuments = tods;

            return View(listOfDoc);
        }

        /// <summary>
        /// Детали документа
        /// </summary>
        public ActionResult DetailsDocument(int id)
        {
            ListOfDocument listOfDoc = db.ListOfDocuments.Find(id);

            if (listOfDoc != null)
            {
                // Получаем сотрудника
                var worker = db.Workers.Where(m => m.Id == listOfDoc.WorkerId);
                if (worker.Count() > 0)
                    listOfDoc.Worker = worker.First();

                // Получаем вид документа
                var tods = db.TypeOfDocuments.Where(m => m.Id == listOfDoc.TypeOfDocumentId);
                if (tods.Count() > 0)
                    listOfDoc.TypeOfDocument = tods.First();

                return View("DetailsDocument", listOfDoc);
            }
            return View("ListDocuments");
        }

        /// <summary>
        /// Удаление документа
        /// </summary>
        public ActionResult DeleteDocument(int id)
        {
            var countId = db.RegisterOfDocPages.Where(r => r.ListOfDocumentId == id).Count();
            var rodp = db.RegisterOfDocPages.Where(r => r.ListOfDocumentId == id).ToList();
            var dir = AppDomain.CurrentDomain.BaseDirectory + "\\Files\\";
            string[] files = new string[countId];
            int i = 0;
            foreach (RegisterOfDocPage scanName in rodp)
            {
                files[i] = scanName.ScanName;
                i++;
            }

            DirectoryInfo dir1 = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "\\Files\\");
            int j = 0;
            for (int k = 0; k < countId; k++)
            {
                foreach (FileInfo file in dir1.GetFiles())
                {
                    if (file.Name == files[k])
                        file.Delete();
                    j++;
                }
            }
            

            ListOfDocument listOfDoc = db.ListOfDocuments.Find(id);

            db.ListOfDocuments.Remove(listOfDoc);
            db.SaveChanges();

            return RedirectToAction("ListDocuments");
        }

        /// <summary>
        /// Поиск документа по названию, серии или номеру
        /// </summary>

        [HttpPost]
        public ActionResult DocSearch(string name)
        {
            var listOfDoc = db.ListOfDocuments.Where(a => a.NumderDocument.ToString().Contains(name)
                || a.Id.ToString().Contains(name)
                || a.NameDocument.Contains(name)
                || a.SeriesDocument.Contains(name)).ToList();
            if (listOfDoc.Count <= 0)
            {
                return HttpNotFound("Ничего не найдено");
            }
            return PartialView(listOfDoc);
        }

        /// <summary>
        /// Формирование pdf документа
        /// </summary>
        public ActionResult ConcatenateDocument(int id)
        {
            var countId = db.RegisterOfDocPages.Where(r => r.ListOfDocumentId == id).Count();
            var rodp = db.RegisterOfDocPages.Where(r => r.ListOfDocumentId == id).ToList();
            var dir = AppDomain.CurrentDomain.BaseDirectory + "\\Files\\";
            string[] files = new string[countId];
            string[] filesFullName = new string[countId];
            int i = 0;
            foreach (RegisterOfDocPage scanName in rodp)
            {
                files[i] = scanName.ScanName;
                filesFullName[i] = dir + files[i];
                i++;
            }

            var filenameForResult = AppDomain.CurrentDomain.BaseDirectory + "\\Files\\result.pdf";
            // Open the output document
            PdfDocument outputDocument = new PdfDocument();
            // Iterate files
            foreach (string file in filesFullName)
            {
                // Open the document to import pages from it.
                PdfDocument inputDocument = PdfReader.Open(file, PdfDocumentOpenMode.Import);
                // Iterate pages
                int count = inputDocument.PageCount;
                for (int idx = 0; idx < count; idx++)
                {
                    // Get the page from the external document...
                    PdfPage page = inputDocument.Pages[idx];
                    // ...and add it to the output document.
                    outputDocument.AddPage(page);
                }
            }
            // Save the document...
            string filename = filenameForResult;
            outputDocument.Save(filename);
            // ...and start a viewer.
            //Process.Start(filename);
            string contentType = "application/pdf";
            return File(filename, contentType, filename);
        }

        #endregion

        #region Работа со страницами документа
        /// <summary>
        /// Просмотр всех страниц документа
        /// </summary>
        [HttpGet]
        public ActionResult DocumentPages(int id)
        {
            ListOfDocument listOfDoc= db.ListOfDocuments.Where(m => m.Id == id).FirstOrDefault();

            var rodp = db.RegisterOfDocPages.Include(r => r.ListOfDocument)
                .Where(r => r.ListOfDocumentId == listOfDoc.Id);
            List<ListOfDocument> lod = db.ListOfDocuments.ToList();
            return View(rodp.ToList());
        }

        [HttpPost]
        public ActionResult DocumentPages(bool actual, string action, int id)
        {
            IEnumerable<RegisterOfDocPage> allRodp = null;
            if (action == "Фильтр")
            {
                if (actual == false)
                {
                    allRodp = from rodp in db.RegisterOfDocPages/*.Include(r => r.ListOfDocument)*/
                              where rodp.Relevance == false && rodp.ListOfDocumentId == id
                              select rodp;
                }
                if (actual == true)
                {
                    allRodp = from rodp in db.RegisterOfDocPages
                              where rodp.Relevance == true && rodp.ListOfDocumentId == id
                              select rodp;
                }
            }
            else if (action == "Сбросить")
            {
                return RedirectToAction($"DocumentPages/{id}");
            }

            return View(allRodp.ToList());
        }

        [HttpGet]
        public ActionResult CreatePage(int id)
        {
            ListOfDocument listOfDoc = db.ListOfDocuments.Where(l => l.Id == id).FirstOrDefault();
            return View();
        }

        [HttpPost]
        public ActionResult CreatePage(RegisterOfDocPage rodp, int id, HttpPostedFileBase error)
        {
            ListOfDocument listOfDoc = db.ListOfDocuments.Where(l => l.Id == id).FirstOrDefault();

            DateTime current = DateTime.Now;

            //если получен файл
            if (error != null)
                {
                    // Получаем расширение
                    string ext = error.FileName.Substring(error.FileName.LastIndexOf('.'));
                    // сохраняем файл по определенному пути на сервере
                    string path = current.ToString("dd.mm.yyyy hh:mm:ss").Replace(":", "_").Replace("/", ".") + ext;
                    error.SaveAs(Server.MapPath("~/Files/" + path));
                    rodp.ScanName = path;
                }

             rodp.ListOfDocumentId = listOfDoc.Id;
             db.RegisterOfDocPages.Add(rodp);
             db.SaveChanges();

             return RedirectToAction($"DocumentPages/{rodp.ListOfDocumentId}");
        }

        /// <summary>
        /// Загрузка файла
        /// </summary>
        public ActionResult Download(int id)
        {
            RegisterOfDocPage rodp = db.RegisterOfDocPages.Find(id);
            if (rodp != null)
            {
                string filename = Server.MapPath("~/Files/" + rodp.ScanName);
                string contentType = "application/pdf";

                string ext = filename.Substring(filename.LastIndexOf('.'));
                switch (ext)
                {
                    case "pdf":
                        contentType = "application/pdf";
                        break;
                }

                return File(filename, contentType, filename);
            }
            return Content("Файл не найден");
        }

        [HttpGet]
        public ActionResult EditPage(int id)
        {
            RegisterOfDocPage rodp = db.RegisterOfDocPages.Find(id);

            return View(rodp);
        }

        [HttpPost]
        public ActionResult EditPage(RegisterOfDocPage rodp, HttpPostedFileBase error/*, int id*/)
        {
                DateTime current = DateTime.Now;
                //если получен файл
                if (error != null)
                {
                    // Получаем расширение
                    string ext = error.FileName.Substring(error.FileName.LastIndexOf('.'));
                    // сохраняем файл по определенному пути на сервере
                    string path = current.ToString("dd.mm.yyyy hh:mm:ss").Replace(":", "_")
                        .Replace("/", ".") + ext;
                    error.SaveAs(Server.MapPath("~/Files/" + path));
                    rodp.ScanName = path;
                }

                db.Entry(rodp).State = EntityState.Modified;                
                db.SaveChanges();
               
                return RedirectToAction($"DocumentPages/{rodp.ListOfDocumentId}");
        }

        public ActionResult DeletePage(int id)
        {
            RegisterOfDocPage rListDoc = db.RegisterOfDocPages.Where(r => r.Id == id).FirstOrDefault();
            ListOfDocument listOfDoc = db.ListOfDocuments.Where(l => l.Id == rListDoc
                .ListOfDocumentId).FirstOrDefault();
            RegisterOfDocPage rodp = db.RegisterOfDocPages.Find(id);

            db.RegisterOfDocPages.Remove(rodp);
            db.SaveChanges();

            DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "\\Files\\");
            string fileName = rodp.ScanName;
            foreach (FileInfo file in dir.GetFiles())
            {
                if (file.Name == fileName)
                    file.Delete();
            }

            return RedirectToAction($"ListDocuments/{listOfDoc.Id}");
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}