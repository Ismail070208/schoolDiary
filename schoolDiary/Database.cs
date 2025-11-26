using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace schoolDiary
{
    public class Database
    {
        public static string ConnectionString =
        @"Data Source=DESKTOP-743IP50\MSSQLSERVER2022;Initial Catalog=SchoolDiaryDB;Integrated Security=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
