using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NewStage2VK.DataAccess
{
    /// <summary>
    /// Класс для административных ф-ций БД
    /// </summary>
    public class LocalDb
    {
        private string connectionString;
        private string masterConnectionString;
        private string outputFolder;
        private string dbName;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="connectionString">Строка подключения</param>
        /// <param name="outputFolder">Директория, содержащая файл БД</param>
        public LocalDb(string connectionString, string outputFolder)
        {
            this.connectionString = connectionString;
            this.outputFolder =  outputFolder;

            SqlConnectionStringBuilder cnStringBuilder = new SqlConnectionStringBuilder(connectionString);
            SqlConnectionStringBuilder cnMasterStringBuilder = new SqlConnectionStringBuilder();
            cnMasterStringBuilder.InitialCatalog = "master";
            cnMasterStringBuilder.DataSource = cnStringBuilder.DataSource;
            cnMasterStringBuilder.IntegratedSecurity = cnStringBuilder.IntegratedSecurity;
            masterConnectionString = cnMasterStringBuilder.ConnectionString;

            this.dbName = cnStringBuilder.InitialCatalog;
        }

        /// <summary>
        /// Выполнение sql-скрипта, содержащего команды миграций БД
        /// </summary>
        /// <param name="filename">Имя файла sql-скрипта</param>
        /// <returns>False, если файл скрипта не найден</returns>
        public bool ApplyMigration(string filename)
        {
            string sqlFile = Path.Combine(outputFolder, filename);
            if (!File.Exists(sqlFile))
            {
                return false;
            }

            string sql = File.ReadAllText(sqlFile);

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }

            return true;
        }

        /// <summary>
        /// Выполнить инициализацию БД. При необходимости создать новую БД
        /// </summary>
        /// <param name="deleteIfExists">Флаг, указывающий, удалять ли БД, если она уже существует</param>
        public void InitLocalDB(bool deleteIfExists = false)
        {
            try
            {
                string mdfFilename = dbName + ".mdf";
                string dbFileName = Path.Combine(outputFolder, mdfFilename);
                string logFileName = Path.Combine(outputFolder, string.Format("{0}_log.ldf", dbName));
                // Create Data Directory If It Doesn't Already Exist.
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }

                // If the file exists, and we want to delete old data, remove it here and create a new database.
                if (File.Exists(dbFileName) && deleteIfExists)
                {
                    if (File.Exists(logFileName)) File.Delete(logFileName);
                    File.Delete(dbFileName);
                    CreateDatabase(dbFileName);
                }
                // If the database does not already exist, create it.
                else if (!File.Exists(dbFileName))
                {
                    CreateDatabase(dbFileName);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Отсоеденить файл БД от "движка"
        /// </summary>
        /// <returns>True - успех</returns>
        public bool DetachDatabase()
        {
            try
            {
                using (var connection = new SqlConnection(masterConnectionString))
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = string.Format("exec sp_detach_db '{0}'", dbName);
                    cmd.ExecuteNonQuery();

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Создать файл БД
        /// </summary>
        /// <param name="dbFileName">Имя файла БД</param>
        /// <returns>True - успех</returns>
        private bool CreateDatabase(string dbFileName)
        {
            try
            {
                using (var connection = new SqlConnection(masterConnectionString))
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();


                    DetachDatabase();

                    cmd.CommandText = String.Format("CREATE DATABASE {0} ON (NAME = N'{0}', FILENAME = '{1}')", dbName, dbFileName);
                    cmd.ExecuteNonQuery();
                }

                if (File.Exists(dbFileName)) return true;
                else return false;
            }
            catch
            {
                throw;
            }
        }
    }
}
