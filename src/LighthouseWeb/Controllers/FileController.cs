using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LighthouseWeb.Controllers
{
    public class FileController : Controller
    {
        private readonly ISmbService _smbService;
        private bool _disposed;

        public FileController(ISmbService smbService)
        {
            _smbService = smbService;
        }

        // GET: Operations
        public ActionResult Index()
        {
            var files = _smbService.GetFiles();
            return View(files);
        }

        // GET: Operations/Details/5
        public async Task<ActionResult> Details(string name)
        {
            var file = await _smbService.GetFile(name);
            if (file?.Name == null)
            {
                return HttpNotFound();
            }
            return View(file);
        }

        // GET: Operations/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: Operations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase file)
        {
            try
            {
                _smbService.CreateFile(file);

                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Operations/Delete/5
        public async Task<ActionResult> Delete(string name)
        {
            var file =await _smbService.GetDeleteModel(name);
            if (file?.Name == null)
            {
                return HttpNotFound();
            }
            return View(file);
        }

        // POST: Operations/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string name, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                _smbService.DeleteFile(name);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _smbService.Dispose();
            }

            _disposed = true;

            base.Dispose(disposing);
        }
    }
}