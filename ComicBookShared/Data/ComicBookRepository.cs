using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ComicBookShared.Models;

namespace ComicBookShared.Data
{
    public class ComicBookRepository : BaseRepository<ComicBook>
    {
        private Context _context = null;

        public ComicBookRepository(Context context): base(context)
        {
            
        }


        public override IList<ComicBook> GetList()
        {
            return Context.ComicBooks
                .Include(cb => cb.Series)
                .OrderBy(cb => cb.Series.Title)
                .ThenBy(cb => cb.IssueNumber)
                .ToList();
        }

        public  ComicBook Get(int comicBookId)
        {
            return Context.ComicBooks
                .Include(cb => cb.Series)
                .Where(cb => cb.Id == comicBookId)
                .SingleOrDefault();
        }

        public override ComicBook Get(int id, bool includeRelatedEntities = true)
        {
            var comicBooks = Context.ComicBooks.AsQueryable();

            if (includeRelatedEntities)
            {
                comicBooks = comicBooks
                    .Include(cb => cb.Series)
                    .Include(cb => cb.Artists.Select(a => a.Artist))
                    .Include(cb => cb.Artists.Select(a => a.Role));
            }

            return comicBooks
                .Where(cb => cb.Id == id)
                .SingleOrDefault();
        }

        public bool ComicBookSeriesHasIssueNumber(int comicBookId, int comicBookSeriesId, int comicBookIssueNumber)
        {
            return Context.ComicBooks.Any(cb => cb.Id != comicBookId &&
                                                 cb.SeriesId == comicBookSeriesId &&
                                                 cb.IssueNumber == comicBookIssueNumber);
        }

        public bool ComicBookHasArtistRoleCombination(int viewModelComicBookId, int viewModelArtistId, int viewModelRoleId)
        {
            return Context.ComicBookArtists
                .Any(cba => cba.ComicBookId == viewModelComicBookId &&
                            cba.ArtistId == viewModelArtistId &&
                            cba.RoleId == viewModelRoleId);
        }
    }
}
