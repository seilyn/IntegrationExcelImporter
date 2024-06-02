using System;
using System.Collections.Generic;
using System.IO;
using IntegrationExcelImporter.Model;
using OfficeOpenXml;

public class ExcelManager
{
    public List<EduPlanGridInfo> ReadExcelToEduPlanGridInfo()
    {
        List<EduPlanGridInfo> eduPlanList = new List<EduPlanGridInfo>();
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (var package = new ExcelPackage(new FileInfo(@"Z:\3. IntegrationExcelImporter\IntegrationExcelImporter\IntegrationExcelImporter\TestData\05. 신사업개발팀.xlsx")))
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets[2];
            if (worksheet == null)
            {
                throw new Exception("Worksheet not found.");
            }

            // 컬럼 AA는 27번째 임.
            const int endColumn = 27; 

            // 7행부터 시작 한다.
            for (var rowNumber = 7; rowNumber <= worksheet.Dimension.End.Row; rowNumber++)
            {
                var row = worksheet.Cells[rowNumber, 1, rowNumber, endColumn];
                EduPlanGridInfo eduPlan = new EduPlanGridInfo
                {
                    KindOfEducation = worksheet.Cells[rowNumber, 1].Text,
                    EducationName = worksheet.Cells[rowNumber, 2].Text,
                    EducationDay = worksheet.Cells[rowNumber, 3].Text,
                    EducationTime = worksheet.Cells[rowNumber, 4].Text,
                    EducationAgency = worksheet.Cells[rowNumber, 5].Text,
                    Team = worksheet.Cells[rowNumber, 6].Text,
                    Position = worksheet.Cells[rowNumber, 7].Text,
                    Name = worksheet.Cells[rowNumber, 8].Text,
                    TuitionFee = worksheet.Cells[rowNumber, 9].Text,
                    TravelFee = worksheet.Cells[rowNumber, 10].Text,
                    SumOfFee = worksheet.Cells[rowNumber, 11].Text,
                    Janaury = worksheet.Cells[rowNumber, 12].Text,
                    Faburary = worksheet.Cells[rowNumber, 13].Text,
                    March = worksheet.Cells[rowNumber, 14].Text,
                    April = worksheet.Cells[rowNumber, 15].Text,
                    May = worksheet.Cells[rowNumber, 16].Text,
                    June = worksheet.Cells[rowNumber, 17].Text,
                    July = worksheet.Cells[rowNumber, 18].Text,
                    August = worksheet.Cells[rowNumber, 19].Text,
                    September = worksheet.Cells[rowNumber, 20].Text,
                    October = worksheet.Cells[rowNumber, 21].Text,
                    November = worksheet.Cells[rowNumber, 22].Text,
                    December = worksheet.Cells[rowNumber, 23].Text,
                    Spot = worksheet.Cells[rowNumber,24].Text,
                    Reason = worksheet.Cells[rowNumber, 25].Text,
                    Planning = worksheet.Cells[rowNumber, 26].Text,
                };
                eduPlanList.Add(eduPlan);
            }
        }

        return eduPlanList;
    }
}
