using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using IntegrationExcelImporter.Common;
using IntegrationExcelImporter.Common.Utility;
using IntegrationExcelImporter.Core.Model;
using OfficeOpenXml;

namespace IntegrationExcelImporter.Core.DataAccess
{
    public class ExcelManager
    {

        /// <summary>
        /// Singleton으로 선언
        /// </summary>
        private static ExcelManager _instance;
        public static ExcelManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ExcelManager();
                }

                return _instance;
            }
        }

        /// <summary>
        /// 생성자 
        /// </summary>
        private ExcelManager()
        {

        }

        public List<Plan> ReadEachExcelData(string filePath)
        {
            List<Plan> eduPlanList = new List<Plan>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new FileInfo(Path.GetFullPath(filePath).Trim())))
            {
                ExcelWorksheet worksheet = null;

                foreach (var sheet in package.Workbook.Worksheets)
                {
                    if (sheet.Name.Contains("교육훈련"))
                    {
                        worksheet = sheet;
                        break;
                    }
                }

                if (worksheet == null)
                {
                    Log.Error("파일을 찾을 수 없습니다 : " + filePath);
                    throw new Exception("Worksheet not found.");
                }

                const int endColumn = 27;

                for (var rowNumber = 7; rowNumber <= worksheet.Dimension.End.Row; rowNumber++)
                {
                    if (string.IsNullOrWhiteSpace(worksheet.Cells[rowNumber, 1].Text))
                    {
                        break;
                    }

                    bool hasData = false;

                    for (int column = 1; column <= endColumn; column++)
                    {
                        if (!string.IsNullOrWhiteSpace(worksheet.Cells[rowNumber, column].Text))
                        {
                            hasData = true;
                            break;
                        }
                    }

                    if (!hasData) break;

                    if (worksheet.Cells[rowNumber, 12].Text == "0.0")
                    {
                        continue;
                    }

                    var row = worksheet.Cells[rowNumber, 1, rowNumber, endColumn];
                    Plan eduPlan = new Plan
                    {
                        KindOfEducation = worksheet.Cells[rowNumber, 1].Text,
                        EducationName = worksheet.Cells[rowNumber, 2].Text,
                        EducationDay = worksheet.Cells[rowNumber, 4].Text,
                        EducationTime = worksheet.Cells[rowNumber, 5].Text,
                        EducationAgency = worksheet.Cells[rowNumber, 6].Text,
                        Team = worksheet.Cells[rowNumber, 7].Text,
                        Position = worksheet.Cells[rowNumber, 8].Text,
                        Name = worksheet.Cells[rowNumber, 9].Text,
                        TuitionFee = worksheet.Cells[rowNumber, 10].Text,
                        TravelFee = worksheet.Cells[rowNumber, 11].Text,
                        SumOfFee = worksheet.Cells[rowNumber, 12].Text,
                        Janaury = worksheet.Cells[rowNumber, 13].Text,
                        Faburary = worksheet.Cells[rowNumber, 14].Text,
                        March = worksheet.Cells[rowNumber, 15].Text,
                        April = worksheet.Cells[rowNumber, 16].Text,
                        May = worksheet.Cells[rowNumber, 17].Text,
                        June = worksheet.Cells[rowNumber, 18].Text,
                        July = worksheet.Cells[rowNumber, 19].Text,
                        August = worksheet.Cells[rowNumber, 20].Text,
                        September = worksheet.Cells[rowNumber, 21].Text,
                        October = worksheet.Cells[rowNumber, 22].Text,
                        November = worksheet.Cells[rowNumber, 23].Text,
                        December = worksheet.Cells[rowNumber, 24].Text,
                        Spot = worksheet.Cells[rowNumber, 25].Text,
                        Reason = worksheet.Cells[rowNumber, 26].Text,
                        Planning = worksheet.Cells[rowNumber, 27].Text,
                    };
                    eduPlanList.Add(eduPlan);
                }
            }

            return eduPlanList;
        }

        public List<Plan> MergeAllFileData(ObservableCollection<Files> fileList)
        {
            HashSet<Plan> uniquePlanSet = new HashSet<Plan>();

            foreach (Files file in fileList)
            {
                List<Plan> plans = ReadEachExcelData(file.FilePath);

                foreach (var plan in plans)
                {
                    uniquePlanSet.Add(plan); 
                }
            }

            return uniquePlanSet.ToList(); 
        }
    }
}
