using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationExcelImporter.Common.Model
{
    public class Setting
    {
        private static readonly Setting _instance = new Setting();
        public static Setting Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// DB 경로
        /// </summary>
        private string _dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "manager_option.db");
        public string DbPath
        {
            get { return _dbPath; }
            set
            {
                _dbPath = value;
            }
        }
    }
}
