using System;

using System.IO;
using IntegrationExcelImporter.Common.Utility;
using System.Data;
using System.Data.SQLite;
using IntegrationExcelImporter.Core.Model;

namespace IntegrationExcelImporter.Core.DataAccess
{
    public class SQLiteManager
    {
        /// <summary>
        /// Singleton으로 선언
        /// </summary>
        private static SQLiteManager _instance;
        public static SQLiteManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SQLiteManager();
                }

                return _instance;
            }
        }

        /// <summary>
        /// 생성자 
        /// </summary>
        private SQLiteManager()
        {

        }

        /// <summary>
        /// DB 경로
        /// </summary>
        /// <returns></returns>
        public string GetConnectionString()
        {
            return Setting.Instance.DbPath;
        }

        /// <summary>
        /// DB 존재 유무 체크
        /// </summary>
        /// <returns></returns>
        public bool DbExists()
        {
            return File.Exists(Setting.Instance.DbPath);
        }
        /// <summary>
        /// 컬럼을 생성 합니다.
        /// </summary>
        public bool CreateTableAndColumn()
        {
            try
            {
                string query = @"

CREATE TABLE MANAGER_OPTION
(
    UID                        INTEGER PRIMARY KEY AUTOINCREMENT,
    SORT_OPTION                TEXT,
    SEARCH_EXCELSHEETS_KEYWORD TEXT,
    SAVE_FILE_PATH             TEXT
);


INSERT INTO MANAGER_OPTION
( 
    SORT_OPTION, SEARCH_EXCELSHEETS_KEYWORD, SAVE_FILE_PATH 
) 
VALUES 
( 
    'kind_of_education', '교육훈련', 'C:\Documents' 
);



";
                SQLiteConnection connection = new SQLiteConnection("Data Source=" + Setting.Instance.DbPath);
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }

        /// <summary>
        /// DB 연결이 되었는지 체크합니다.
        /// </summary>
        /// <returns></returns>
        public bool IsDbConnected()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + Setting.Instance.DbPath))
                {
                    connection.Open();
                    connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }

        public bool SaveOptions(string sortOption, string searchKeyword, string saveFilePath)
        {
            try
            {
                string query = string.Format(@"

UPDATE MANAGER_OPTION

   SET SORT_OPTION                    = '{0}',
       SEARCH_EXCELSHEETS_KEYWORD     = '{1}', 
       SAVE_FILE_PATH                 = '{2}'

", sortOption, searchKeyword, saveFilePath);

                SQLiteConnection connection = new SQLiteConnection("Data Source=" + Setting.Instance.DbPath);
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.ExecuteNonQuery();

                return true;
            }
            catch(Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }

        public DataTable SelectOptions()
        {
            DataTable optionValues = new DataTable();

            try
            {
                string query = @"

SELECT SORT_OPTION, SEARCH_EXCELSHEETS_KEYWORD, SAVE_FILE_PATH
FROM MANAGER_OPTION;";

                using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + Setting.Instance.DbPath))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                        {
                            adapter.Fill(optionValues);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }

            return optionValues;
        }

    }
}


