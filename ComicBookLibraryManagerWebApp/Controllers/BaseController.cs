using System.Web.Mvc;
using ComicBookShared.Data;

namespace ComicBookLibraryManagerWebApp.Controllers
{
    public abstract class BaseController : Controller
    {
        private bool _disposed = false;

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                Context.Dispose();
            }

            _disposed = true;

            base.Dispose(disposing);
        }

        protected Context Context {get; private set; }

        public BaseController()
        {
            Context = new Context();
        }

        
    }
}