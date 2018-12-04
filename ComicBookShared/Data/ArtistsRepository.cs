using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComicBookShared.Models;

namespace ComicBookShared.Data
{
    public class ArtistsRepository : BaseRepository<Artist>
    {
        private Context _context = null;

        public ArtistsRepository(Context context): base(context)
        {
            _context = context;
        }

        public override Artist Get(int id, bool includeRelatedEntities = true)
        {
            var artist = Context.Artists.AsQueryable();

            if (includeRelatedEntities)
            {
                artist = artist
                    .Include(s => s.ComicBooks.Select(a => a.ComicBook.Series))
                    .Include(s => s.ComicBooks.Select(a => a.Role));
            }

            return artist
                .Where(cb => cb.Id == id)
                .SingleOrDefault();
        }

        public override IList<Artist> GetList()
        {
            return Context.Artists.ToList();
        }

        public bool ArtistHasName(int artistId, string name)
        {
            return Context.Artists.Any(a => a.Id != artistId && a.Name == name);
        }
    }
}
