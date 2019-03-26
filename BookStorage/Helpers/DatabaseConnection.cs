using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using NLog;
using BookStorage.Models;
using System.Threading.Tasks;
using System.Data;

namespace BookStorage.Helpers
{
    public class DatabaseConnection
    {
        public Logger logger;
        private SqlConnection Connection { get; set; }

        public DatabaseConnection()
        {
            logger = LogManager.GetCurrentClassLogger();
            try
            {
                Connection = new SqlConnection();
                Connection.ConnectionString = ConfigurationManager.ConnectionStrings["BookStorage.Properties.Settings.BookStorageDBConnectionString"].ConnectionString;
                Connection.Open();
            }
            catch(Exception ex)
            {
                logger.Error("Database connection error: " + ex.Message);
            }
        }

        public async Task<List<Book>>getDBRecords(int startRecord,string searchedPhrase="")
        {
            List<Book> Records = new List<Book>();

            SqlCommand command;
            try
            {
                if (String.IsNullOrEmpty(searchedPhrase))
                    command = new SqlCommand("SELECT * FROM Books ORDER BY OwnershipDate OFFSET " + startRecord + " rows FETCH next 10 rows only", Connection);
                else
                {
                    byte[] bytesSearch = Encoding.UTF8.GetBytes(searchedPhrase);
                    command = new SqlCommand("SELECT * FROM Books WHERE CONVERT(VARCHAR(MAX),Title) LIKE '%" + searchedPhrase + "%' OR CONVERT(VARCHAR(MAX),Author) LIKE '%" + searchedPhrase + "%' ORDER BY OwnershipDate OFFSET " + startRecord + " rows FETCH next 13 rows only", Connection);
                }

                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    Book book = new Book();
                    book.Id = reader.GetInt32(0);
                    byte[] binaryTitle = (byte[])reader["Title"];
                    book.Title = Encoding.UTF8.GetString(binaryTitle);
                    byte[] binaryAuthor = (byte[])reader["Author"];
                    book.Author = Encoding.UTF8.GetString(binaryAuthor);
                    book.OwnershipDate = reader.GetDateTime(3);
                    byte[] binaryLocation = (byte[])reader["Location"];
                    book.Location = Encoding.UTF8.GetString(binaryLocation);
                    book.Isbn = reader.GetString(5);
                    Records.Add(book);
                }
                reader.Close();
            }
            catch(Exception ex)
            {
                logger.Error("Database retrieve records error: " + ex.Message);
            }
            return Records;
        }

        public int getRecordCount(string searchedPhrase = "")
        {
            int count = 0;
            try
            {
                SqlCommand command;
                if (String.IsNullOrEmpty(searchedPhrase))
                {
                    command = new SqlCommand("SELECT count(*) FROM Books", Connection);
                }
                else
                {
                    byte[] bytesSearch = Encoding.UTF8.GetBytes(searchedPhrase);
                    command = new SqlCommand("SELECT count(*) FROM Books WHERE CONVERT(VARCHAR(MAX),Title) LIKE '%" + searchedPhrase + "%' OR CONVERT(VARCHAR(MAX),Author) LIKE '%" + searchedPhrase + "%'", Connection);
                }
                count= (int)command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                logger.Error("Database record count: " + ex.Message);
            }
            return count;
        }

        public int getOverduedRecordCount(string searchedPhrase = "")
        {
            int count = 0;
            try
            {
                DateTime date = DateTime.Now.AddDays(-183);
                SqlCommand command;
                if (String.IsNullOrEmpty(searchedPhrase))
                {
                    command = new SqlCommand("SELECT count(*) FROM Books WHERE OwnershipDate<=@Date", Connection);
                }
                else
                {
                    byte[] bytesSearch = Encoding.UTF8.GetBytes(searchedPhrase);
                    command = new SqlCommand("SELECT count(*) FROM Books WHERE CONVERT(VARCHAR(MAX),Title) LIKE '%" + searchedPhrase + "%' OR CONVERT(VARCHAR(MAX),Author) LIKE '%" + searchedPhrase + "%' AND OwnershipDate<=@Date", Connection);
                }
                command.Parameters.Add("@Date", SqlDbType.DateTime).Value = date;
                count = (int)command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                logger.Error("Database record count: " + ex.Message);
            }
            return count;
        }

        public void addBookToDB(Book book)
        {
            try
            {
                byte[] bytesTitle = Encoding.UTF8.GetBytes(book.Title);
                byte[] bytesAuthor= Encoding.UTF8.GetBytes(book.Author);
                byte[] bytesLocation = Encoding.UTF8.GetBytes(book.Location);
                SqlCommand command = new SqlCommand("INSERT INTO Books (Title,Author,OwnershipDate,Location,Isbn) VALUES (@Title, @Author, @OwnershipDate, @Location, @Isbn)",Connection);
                command.Parameters.Add("@Title", SqlDbType.VarBinary).Value = bytesTitle;
                command.Parameters.Add("@Author", SqlDbType.VarBinary).Value = bytesAuthor;
                command.Parameters.Add("@Location", SqlDbType.VarBinary).Value = bytesLocation;
                command.Parameters.Add("@Isbn", SqlDbType.VarChar).Value = book.Isbn;
                command.Parameters.Add("@OwnershipDate", SqlDbType.DateTime).Value = book.OwnershipDate;
                command.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                logger.Error("Database insert error: " + ex.Message);
            }
        }

        public void updateBookInDB(Book book)
        {
            try
            {
                byte[] bytesTitle = Encoding.UTF8.GetBytes(book.Title);
                byte[] bytesAuthor = Encoding.UTF8.GetBytes(book.Author);
                byte[] bytesLocation = Encoding.UTF8.GetBytes(book.Location);
                SqlCommand command = new SqlCommand("UPDATE Books SET Title=@Title, Author=@Author, OwnershipDate=@OwnershipDate, Location=@Location, Isbn=@Isbn WHERE Id=@Id", Connection);
                command.Parameters.Add("@Title", SqlDbType.VarBinary).Value = bytesTitle;
                command.Parameters.Add("@Author", SqlDbType.VarBinary).Value = bytesAuthor;
                command.Parameters.Add("@Location", SqlDbType.VarBinary).Value = bytesLocation;
                command.Parameters.Add("@Isbn", SqlDbType.VarChar).Value = book.Isbn;
                command.Parameters.Add("@OwnershipDate", SqlDbType.DateTime).Value = book.OwnershipDate;
                command.Parameters.Add("@Id", SqlDbType.Int).Value = book.Id;
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logger.Error("Database update error: " + ex.Message);
            }
        }

        public void removeBookFromDB(Book book)
        {
            try
            {
                SqlCommand command = new SqlCommand("DELETE FROM Books WHERE Id=@Id", Connection);
                command.Parameters.Add("@Id", SqlDbType.Int).Value = book.Id;
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logger.Error("Database update error: " + ex.Message);
            }
        }

        public void fillDB()
        {
            for(int i=0;i<31;i++)
            {
                Book book = new Book();
                book.Title = "Title" + i.ToString();
                book.Author = "Author" + i.ToString();
                book.OwnershipDate = DateTime.Now.AddDays(i);
                book.Location = "Jaworzno";
                book.Isbn = i.ToString();
                addBookToDB(book);
            }
        }
    }
}
