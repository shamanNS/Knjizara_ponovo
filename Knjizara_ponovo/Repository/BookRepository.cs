using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Knjizara_ponovo.Models;
using Knjizara_ponovo.Repository;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace Knjizara_ponovo.Repository
{
    public class BookRepository : IRepository<Book>
    {
        private SqlConnection conn;
        private void LoadConnection()
        {
            string connString = ConfigurationManager.ConnectionStrings["AlephDbContext"].ConnectionString;
            //string connString = ConfigurationManager.ConnectionStrings["HomeDbContext"].ConnectionString;
            conn = new SqlConnection(connString);
        }
        public bool Create(Book book)
        {
            string query = "INSERT INTO Book (BookName, BookPrice, BookGenre, Deleted) VALUES (@BookName, @BookPrice, @BookGenre, @Deleted);";
            query += " SELECT SCOPE_IDENTITY()";        // selektuj id novododatog zapisa nakon upisa u bazu

            LoadConnection();   // inicijaizuj novu konekciju

            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = query;    // ovde dodeljujemo upit koji ce se izvrsiti nad bazom podataka
                cmd.Parameters.AddWithValue("@BookName", book.Name); 
                cmd.Parameters.AddWithValue("@BookPrice", book.Price);
                cmd.Parameters.AddWithValue("@BookGenre", book.Genre.Id);
                cmd.Parameters.AddWithValue("@Deleted", book.isDeleted);

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
            string query = "UPDATE Book SET Deleted = 1 WHERE BookId = @BookId;";

            LoadConnection();   // inicijaizuj novu konekciju

            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = query;    // ovde dodeljujemo upit koji ce se izvrsiti nad bazom podataka
                cmd.Parameters.AddWithValue("@BookId", id);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public IEnumerable<Book> GetAll()
        {
            LoadConnection();
            string query = "SELECT * FROM Book b INNER JOIN Genre g ON b.BookGenre = g.GenreId";
            DataTable dt = new DataTable(); // objekti u
            DataSet ds = new DataSet();     // koje smestam podatke


            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = query;

                SqlDataAdapter dadapter = new SqlDataAdapter();
                dadapter.SelectCommand = cmd;

                // Fill(...) metoda je bitna, jer se prilikom poziva te metode izvrsava upit nad bazom podataka
                dadapter.Fill(ds, "Books"); // naziv tabele u dataset-u
                dt = ds.Tables["Books"];    // formiraj DataTable na osnovu tabele u DataSet-u
                conn.Close();
            }

            List<Book> Books = new List<Book>();

            foreach (DataRow dataRow in dt.Rows)    // izvuci podatke iz svakog reda tj. zapisa tabele
            {
                int bookId = int.Parse(dataRow["BookId"].ToString());
                string bookName = dataRow["BookName"].ToString();
                double bookPrice = double.Parse(dataRow["BookPrice"].ToString());
                bool deleted = bool.Parse(dataRow["Deleted"].ToString());
                int genreId = int.Parse(dataRow["GenreId"].ToString());
                string genreName = dataRow["GenreName"].ToString();

                Genre genre = new Genre() { Id = genreId, Name = genreName };

                Books.Add(new Book() { Id = bookId, Name = bookName, Price = bookPrice, Genre = genre, isDeleted = deleted });
            }

            return Books;
        }

        public Book GetById(int id)
        {
            // PROVERITI JOŠ OVO
            string query = "SELECT * FROM Book b INNER JOIN Genre g ON b.BookGenre = g.GenreId WHERE b.BookId = @BookId";
            LoadConnection();

            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@BookId", id);

                SqlDataAdapter dadapter = new SqlDataAdapter();
                dadapter.SelectCommand = cmd;

                // Fill(...) metoda je bitna, jer se prilikom poziva te metode izvrsava upit nad bazom podataka
                dadapter.Fill(ds, "Book"); // 'Book' je naziv tabele u dataset-u
                dt = ds.Tables["Book"];    // formiraj DataTable na osnovu Book tabele u DataSet-u
                conn.Close();
            }

            Book book = null;

            foreach (DataRow dataRow in dt.Rows)
            {
                int bookId = int.Parse(dataRow["BookId"].ToString());
                string bookName = dataRow["BookName"].ToString();
                double bookPrice = double.Parse(dataRow["BookPrice"].ToString());
                int genreId = int.Parse(dataRow["BookGenre"].ToString());
                string genreName = dataRow["GenreName"].ToString();
                bool Deleted = bool.Parse(dataRow["Deleted"].ToString());

                Genre genre = new Genre() { Id = genreId, Name = genreName };
                book = new Book() { Id = bookId, Name = bookName, Price = bookPrice, Genre = genre, isDeleted = Deleted };
            }

            return book;
        }

        public void Update(Book book)
        {
            string query = "UPDATE Book SET BookName = @BookName, BookPrice = @BookPrice, BookGenre = @BookGenre WHERE BookId = @BookId;";

            LoadConnection();

            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = query;    // ovde dodeljujemo upit koji ce se izvrsiti nad bazom podataka
                cmd.Parameters.AddWithValue("@BookName", book.Name);
                cmd.Parameters.AddWithValue("@BookPrice", book.Price);
                cmd.Parameters.AddWithValue("@BookGenre", book.Genre.Id);
                cmd.Parameters.AddWithValue("@BookId", book.Id);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
}