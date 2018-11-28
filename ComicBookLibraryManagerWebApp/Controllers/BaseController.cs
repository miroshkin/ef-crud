using System.Web.Mvc;
using ComicBookShared.Data;

namespace ComicBookLibraryManagerWebApp.Controllers
{
    public abstract class BaseController : Controller
    {
        private Context _context = null;

        private bool _disposed = false;

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _context.Dispose();
            }

            _disposed = true;

            base.Dispose(disposing);
        }

        protected Repository Repository {get; private set; }

        public BaseController()
        {
            _context = new Context();
            Repository = new Repository(_context);
        }

        
    }
}