using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ComicBookShared.Models;

namespace ComicBookShared.Data
{
    public class ComicBookRepository
    {
        private Context _context = null;

        public ComicBookRepository(Context context)
        {
            _context = context;
        }


        public IList<ComicBook> GetList()
        {
            return _context.ComicBooks
                .Include(cb => cb.Series)
                .OrderBy(cb => cb.Series.Title)
                .ThenBy(cb => cb.IssueNumber)
                .ToList();
        }

        public ComicBook Get(int comicBookId)
        {
            return _context.ComicBooks
                .Include(cb => cb.Series)
                .Where(cb => cb.Id == comicBookId)
                .SingleOrDefault();
        }

        public void Add(ComicBook comicBook)
        {
            _context.ComicBooks.Add(comicBook);
            _context.SaveChanges();
        }

        public void Update(ComicBook comicBook)
        {
            _context.Entry(comicBook).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var comicBook = new ComicBook() { Id = id };
            _context.Entry(comicBook).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public ComicBook Get(int id, bool includeRelatedEntities = true)
        {
            var comicBooks = _context.ComicBooks.AsQueryable();

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
            return _context.ComicBooks.Any(cb => cb.Id != comicBookId &&
                                                 cb.SeriesId == comicBookSeriesId &&
                                                 cb.IssueNumber == comicBookIssueNumber);
        }

        public bool ComicBookHasArtistRoleCombination(int viewModelComicBookId, int viewModelArtistId, int viewModelRoleId)
        {
            return _context.ComicBookArtists
                .Any(cba => cba.ComicBookId == viewModelComicBookId &&
                            cba.ArtistId == viewModelArtistId &&
                            cba.RoleId == viewModelRoleId);
        }
    }
}
