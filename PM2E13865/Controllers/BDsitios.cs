using System;
using System.Collections.Generic;
using System.Text;
using PM2E13865.Models;
using System.Threading.Tasks;
using SQLite;

namespace PM2E13865.Controllers
{
    public class BDsitios
    {

        readonly SQLiteAsyncConnection dbase;

        public BDsitios(string dbpath)
        {
            dbase = new SQLiteAsyncConnection(dbpath);

            //Creacion de las tablas de la base de datos

            dbase.CreateTableAsync<Sitios>();

        }

        #region OperacionesSitio
        //Metodos CRUD - CREATE UPDATE
        public Task<int> insertUpdateSitio(Sitios sitio)
        {
            if (sitio.Id != 0)
            {
                return dbase.UpdateAsync(sitio);
            }
            else
            {
                return dbase.InsertAsync(sitio);
            }
        }

        //Metodos CRUD - READ LIST
        public Task<List<Sitios>> getListSitio()
        {
            return dbase.Table<Sitios>().ToListAsync();
        }

        public Task<Sitios> getSitio(int id)
        {
            return dbase.Table<Sitios>()
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync();
        }

        //Metodos CRUD - DELETE
        public Task<int> deleteSitio(Sitios sitio)
        {
            return dbase.DeleteAsync(sitio);
        }

        #endregion OperacionesSitio

    }
}
