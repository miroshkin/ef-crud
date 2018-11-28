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

        public IList<ComicBook> GetComicBooks()
        {
            return  _context.ComicBooks
                .Include(cb => cb.Series)
                .OrderBy(cb => cb.Series.Title)
                .ThenBy(cb => cb.IssueNumber)
                .ToList();
        }

        public ComicBook GetComicBook(int comicBookId)
        {
            return _context.ComicBooks
                .Include(cb => cb.Series)
                .Where(cb => cb.Id == comicBookId)
                .SingleOrDefault();
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

        public void AddComicBook(ComicBook comicBook)
        {
            _context.ComicBooks.Add(comicBook);
            _context.SaveChanges();
        }

        public ComicBook GetComicBook(int id, bool includeRelatedEntities = true)
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

        public void UpdateComicBook(ComicBook comicBook)
        {
            _context.Entry(comicBook).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteComicBook(int id)
        {
            var comicBook = new ComicBook(){Id = id};
            _context.Entry(comicBook).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public bool ComicBookSeriesHasIssueNumber(int comicBookId, int comicBookSeriesId, int comicBookIssueNumber)
        {
            return _context.ComicBooks.Any(cb => cb.Id != comicBookId &&
                                                 cb.SeriesId == comicBookSeriesId &&
                                                 cb.IssueNumber == comicBookIssueNumber);
        }

        public void AddComicBookArtist(ComicBookArtist comicBookArtist)
        {
            _context.ComicBookArtists.Add(comicBookArtist);
            _context.SaveChanges();
        }

        public ComicBookArtist GetComicBookArtist(int id)
        {
            return _context.ComicBookArtists
                .Include(cba => cba.Artist)
                .Include(cba => cba.Role)
                .Include(cba => cba.ComicBook.Series)
                .Where(cba => cba.Id == (int)id)
                .SingleOrDefault();
        }

        public void DeleteComicBookArtist(int id)
        {
            var comicBookArtist = new ComicBookArtist() { Id = id };
            _context.Entry(comicBookArtist).State = EntityState.Deleted;
            _context.SaveChanges();
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
