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
    SAVE_FILE_PATH             TEXT,
    IS_AUTO_LOGIN              TEXT,
    SAVE_FILE_NAME             TEXT
);


INSERT INTO MANAGER_OPTION
( 
    SORT_OPTION, 
    SEARCH_EXCELSHEETS_KEYWORD, 
    SAVE_FILE_PATH,
    IS_AUTO_LOGIN,
    SAVE_FILE_NAME
) 
VALUES 
( 
    'null',
    '교육훈련', 
    'C:\Documents',
    'N',
    'TestData'
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

        public bool SaveOptions(string sortOption, string searchKeyword, string saveFilePath, string saveFileName)
        {

            SQLiteConnection connection = new SQLiteConnection("Data Source=" + Setting.Instance.DbPath);
            connection.Open();
            SQLiteTransaction transaction = connection.BeginTransaction();

            try
            {
                string query = string.Format(@"

UPDATE MANAGER_OPTION

   SET SORT_OPTION                    = '{0}',
       SEARCH_EXCELSHEETS_KEYWORD     = '{1}', 
       SAVE_FILE_PATH                 = '{2}', 
       SAVE_FILE_NAME                 = '{3}'

", sortOption, searchKeyword, saveFilePath, saveFileName);

              


                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.ExecuteNonQuery();
                transaction.Commit();
                Log.Info(query);

                return true;
            }
            catch(Exception ex)
            {
                Log.Error(ex);
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        public bool SelectOptions()
        {
            DataTable optionValues = new DataTable();

            try
            {
                string query = @"

SELECT SORT_OPTION,
       SEARCH_EXCELSHEETS_KEYWORD, 
       SAVE_FILE_PATH,
       IS_AUTO_LOGIN,
       SAVE_FILE_NAME

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
                
                if (optionValues.Rows.Count > 0)
                {
                    DataRow row = optionValues.Rows[0];

                    Setting.Instance.SortOptions = row["SORT_OPTION"].ToString();
                    Setting.Instance.SearchKeywords = row["SEARCH_EXCELSHEETS_KEYWORD"].ToString();
                    Setting.Instance.SaveFilePath = row["SAVE_FILE_PATH"].ToString();
                    Setting.Instance.IsAutoLogin = row["IS_AUTO_LOGIN"].ToString();
                    Setting.Instance.SaveFileName = row["SAVE_FILE_NAME"].ToString();
                }

                Log.Info("설정값을 불러왔습니다.");
                Log.Info(query);
                return true;

            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }

        }

    }
}


