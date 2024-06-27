using IntegrationExcelImporter.Core.DataAccess;
using IntegrationExcelImporter.Common.Utility;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationExcelImporter.Core.Init
{
    public class SQLiteInitalize
    {
        /// <summary>
        /// DB 생성
        /// </summary>
        public SQLiteInitalize()
        {
            if (!SQLiteManager.Instance.DbExists())
            {
                try
                {
                    SQLiteConnection.CreateFile(SQLiteManager.Instance.GetConnectionString());
                    if (SQLiteManager.Instance.CreateTableAndColumn())
                    {
                        Log.Info("Database Created");
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }
            else
            {
                Log.Info("Database exists");
            }
        }
    }
}
