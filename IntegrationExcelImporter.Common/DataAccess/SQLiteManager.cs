using System;

using System.IO;
using IntegrationExcelImporter.Common.Utility;
using System.Data;
using System.Data.SQLite;
using IntegrationExcelImporter.Common.Model;

namespace IntegrationExcelImporter.Common.DataAccess
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
    SEARCH_EXCELSHEETS_KEYWORD TEXT
);


INSERT INTO USERHEADER ( SORT_OPTION, SEARCH_EXCELSHEETS_KEYWORD ) VALUES ( 'kind_of_education', '교육훈련' );



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

    }
}


