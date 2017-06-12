using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Knjizara_ponovo.Models;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace Knjizara_ponovo.Repository
{
    public class GenreRepository : IRepository<Genre>
    {
        private SqlConnection conn;
        private void LoadConnection()
        {
            string connString = ConfigurationManager.ConnectionStrings["AlephDbContext"].ConnectionString;
            //string connString = ConfigurationManager.ConnectionStrings["HomeDbContext"].ConnectionString;
            conn = new SqlConnection(connString);
        }
        public bool Create(Genre genre)
        {
            string query = "INSERT INTO Genre (GenreName) VALUES (@GenreName);";
            query += " SELECT SCOPE_IDENTITY()";        // selektuj id novododatog zapisa nakon upisa u bazu

            LoadConnection();   // inicijaizuj novu konekciju

            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = query;    // ovde dodeljujemo upit koji ce se izvrsiti nad bazom podataka
                cmd.Parameters.AddWithValue("@GenreName", genre.Name);


                conn.Open();
                var newFormedId = cmd.ExecuteScalar();   // izvrsi upit nad bazom, vraca id novododatog zapisa
                conn.Close();

                if (newFormedId != null)
                {
                    return true;    // upis uspesan, generisan novi id
                }
            }
            return false;
        }

        public void Delete(int id)
        {
            //string query = "DELETE FROM Genre WHERE GenreId == (@GenreId);";

            /*ili
             
             string query = "UPDATE Genre SET Deleted = 1 WHERE GenreId = @GenreId;";
            */
            //Loadconnection();

            //using (SqlCommand cmd = conn.CreateCommand())
            //{
            //    cmd.CommandText = query;    // ovde dodeljujemo upit koji ce se izvrsiti nad bazom podataka
            //    cmd.Parameters.AddWithValue("@GenreId", id);  // stitimo od SQL Injection napada
           //     conn.Open();
            //    cmd.ExecuteNonQuery();
            //    conn.Close();
            //}
            throw new NotImplementedException();
        }

        public IEnumerable<Genre> GetAll()
        {
            LoadConnection();
            string query = "SELECT * FROM Genre;";
            DataTable dt = new DataTable(); // objekti u
            DataSet ds = new DataSet();     // koje smestam podatke


            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = query;

                SqlDataAdapter dadapter = new SqlDataAdapter();
                dadapter.SelectCommand = cmd;


                dadapter.Fill(ds, "Genres"); // naziv tabele u dataset-u
                dt = ds.Tables["Genres"];    // formiraj DataTable na osnovu tabele u DataSet-u
                conn.Close();
            }

            List<Genre> Genres = new List<Genre>();

            foreach (DataRow dataRow in dt.Rows)    // izvuci podatke iz svakog reda tj. zapisa tabele
            {
                int genreId = int.Parse(dataRow["GenreId"].ToString());
                string genreName = dataRow["GenreName"].ToString();
                

                Genre genre = new Genre() { Id = genreId, Name = genreName };

                Genres.Add(genre);
            }

            return Genres;
        }

        public Genre GetById(int id)
        {
            string query = "SELECT * FROM Genre g WHERE g.GenreId = @GenreId;";
            LoadConnection();   // inicijaizuj novu konekciju

            DataTable dt = new DataTable(); // objekti u 
            DataSet ds = new DataSet();     // koje smestam podatke

            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@GenreId", id);

                SqlDataAdapter dadapter = new SqlDataAdapter();
                dadapter.SelectCommand = cmd;

                // Fill(...) metoda je bitna, jer se prilikom poziva te metode izvrsava upit nad bazom podataka
                dadapter.Fill(ds, "Genre"); // 'Genre' je naziv tabele u dataset-u
                dt = ds.Tables["Genre"];    // formiraj DataTable na osnovu ProductCategory tabele u DataSet-u
                conn.Close();
            }

            Genre genre = null;

            foreach (DataRow dataRow in dt.Rows)            // izvuci podatke iz svakog reda tj. zapisa tabele
            {
                int genreId = int.Parse(dataRow["GenreId"].ToString());
                string genreName = dataRow["GenreName"].ToString();

                genre = new Genre() { Id = genreId, Name = genreName };
            }

            return genre;
        }

        public void Update(Genre genre)
        {
            string query = "UPDATE Genre SET GenreName = @GenreName WHERE GenreId = @GenreId;";

            LoadConnection();

            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = query;    // ovde dodeljujemo upit koji ce se izvrsiti nad bazom podataka
                cmd.Parameters.AddWithValue("@GenreName", genre.Name);
                cmd.Parameters.AddWithValue("@GenreId", genre.Id);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
}