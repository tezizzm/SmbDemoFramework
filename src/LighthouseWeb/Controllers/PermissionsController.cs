using System.Web.Mvc;

namespace LighthouseWeb.Controllers
{
    public class PermissionsController : Controller
    {
        private readonly ISmbService _smbService;

        public PermissionsController(ISmbService smbService)
        {
            _smbService = smbService;
        }

        // GET: Permissions
        public ActionResult Index()
        {
            var permissions = _smbService.GetFolderPermissions();
            return View(permissions);
        }
    }
}
