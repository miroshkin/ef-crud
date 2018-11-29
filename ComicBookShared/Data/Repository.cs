using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ComicBookShared.Models;
using System.Data.Entity;

namespace ComicBookShared.Data
{
    public class Repository
    {
        private Context _context = null;

        public Repository(Context context )
        {
            _context = context;
        }

        

        public List<Artist> GetArtists()
        {
            return _context.Artists.OrderBy(a => a.Name).ToList();
        }

        public List<Role> GetRoles()
        {
            return _context.Roles.OrderBy(r => r.Name).ToList();
        }

        public List<Series> GetSeriesList()
        {
            return _context.Series.OrderBy(s => s.Title).ToList();
        }

       

        

        

        

 

        
    }
}
