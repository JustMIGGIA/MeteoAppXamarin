using MeteoAppSkeleton.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MeteoAppSkeleton
{
    public class DBManager
    {
        private readonly SQLiteAsyncConnection database;

        public DBManager()
        {
            // collegamento al database
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MeteoAppLocations.db3");
            database = new SQLiteAsyncConnection(dbPath);

            // crea la tabella se non esiste
            database.CreateTableAsync<Location>().Wait();
        }

        /*
         * Ritorna una lista con tutti gli items.
         */
        public Task<List<Location>> GetItemsAsync()
        {
            return database.Table<Location>().ToListAsync();
        }

        /*
         * Query con statement SQL.
         */
        public Task<List<Location>> GetItemsWithWhere(string id)
        {
            return database.QueryAsync<Location>("SELECT * FROM [Location] WHERE [Id] =?", id);
        }

        /*
         * Query con LINQ.
         */
        public Task<Location> GetItemAsync(string id)
        {
            return database.Table<Location>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        /*
         * Salvataggio o update.
         */
        public Task<bool> SaveItemAsync(Location item)
        {
            if (GetItemsWithWhere(item.ID).Result.Count > 0)
            {
                database.UpdateAsync(item);
                return Task.FromResult(false);
            }

            database.InsertAsync(item);
            return Task.FromResult(true);
        }

        /*
         * Cancellazione di un elemento.
         */
        public Task<int> DeleteItemAsync(Location item)
        {
            return database.DeleteAsync(item);
        }
    }

}
