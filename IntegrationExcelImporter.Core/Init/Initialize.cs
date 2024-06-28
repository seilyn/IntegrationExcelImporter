using IntegrationExcelImporter.Core.DataAccess;
using IntegrationExcelImporter.Common.Utility;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegrationExcelImporter.Core.Model;

namespace IntegrationExcelImporter.Core.Init
{
    public class Initialize
    {
        /// <summary>
        /// DB 생성
        /// </summary>
        public Initialize()
        {
            
            /// DB 이니셜라이저
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

            // SQLite 에 있는 값 정보 가지고 오기.
            if (SQLiteManager.Instance.SelectOptions())
            {
                Log.Info("SettingValue initialize completed");
            }
            else
            {
                Log.Error("SettingValue initialize failed");
            }
        }
    }
}
