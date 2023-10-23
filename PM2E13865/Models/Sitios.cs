using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PM2E13865.Models
{
    public class Sitios
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }

        public double latitud { get; set; }

        public double longitud { get; set; }

        public String descripcion { get; set; }

        public String pathImage { get; set; }   

        public Byte[] image { get; set; }

    }
}
