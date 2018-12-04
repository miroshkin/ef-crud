using ComicBookShared.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ComicBookShared.Data
{
    public class ComicBookArtistRepository : BaseRepository<ComicBookArtist>
    {
        private Context _context = null;

        public ComicBookArtistRepository(Context context): base(context)
        {
            _context = context;
        }

        public override ComicBookArtist Get(int id, bool includeRelatedEntities = true)
        {
            var comicBookArtists = Context.ComicBookArtists.AsQueryable();

            if (includeRelatedEntities)
            {
                comicBookArtists = comicBookArtists
                    .Include(cba => cba.Artist)
                    .Include(cba => cba.Role)
                    .Include(cba => cba.ComicBook.Series);
            }

            return comicBookArtists
                .Where(cba => cba.Id == (int)id)
                .SingleOrDefault();
        }

        public override IList<ComicBookArtist> GetList()
        {
            throw new System.NotImplementedException();
        }
    }
}
