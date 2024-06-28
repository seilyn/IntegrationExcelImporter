using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using IntegrationExcelImporter.Common;
using IntegrationExcelImporter.Common.Utility;
using IntegrationExcelImporter.Core.Model;
using OfficeOpenXml;
using OfficeOpenXml.Style;

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
                    if (sheet.Name.Contains(Setting.Instance.SearchKeywords))
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

        public void WriteMergedDataToNewSheet(ObservableCollection<Plan> mergedPlans, string outputFilePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new FileInfo(outputFilePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("통합");
        
                // 교육훈련계획서 제목
                var title = worksheet.Cells["A1:Z2"];
                title.Merge = true;
                title.Value = "9. 교육훈련계획서";
                title.Style.Font.Name = "맑은 고딕(제목)";                
                title.Style.Font.Size = 20;
                title.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                title.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                
                title.Style.Border.Top.Style = ExcelBorderStyle.Medium;
                title.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                title.Style.Border.Left.Style = ExcelBorderStyle.Medium;
                title.Style.Border.Right.Style = ExcelBorderStyle.Medium;                
                title.Style.Border.Top.Color.SetColor(Color.Black);
                title.Style.Border.Bottom.Color.SetColor(Color.Black);
                title.Style.Border.Left.Color.SetColor(Color.Black);
                title.Style.Border.Right.Color.SetColor(Color.Black);

                // 교육 종류
                var educationKind = worksheet.Cells["A3:A4"];
                educationKind.Style.Fill.PatternType = ExcelFillStyle.Solid;
                educationKind.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                educationKind.Merge = true;
                educationKind.Value = "교육\n종류";
                educationKind.Style.WrapText = true;
                educationKind.Style.Font.Name = "맑은 고딕";
                educationKind.Style.Font.Bold = true;
                educationKind.Style.Font.Size = 10;
                educationKind.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                educationKind.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                educationKind.Style.Border.Top.Style = ExcelBorderStyle.Medium;
                educationKind.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                educationKind.Style.Border.Left.Style = ExcelBorderStyle.Medium;
                educationKind.Style.Border.Right.Style = ExcelBorderStyle.Medium;
                educationKind.Style.Border.Top.Color.SetColor(Color.Black);
                educationKind.Style.Border.Bottom.Color.SetColor(Color.Black);
                educationKind.Style.Border.Left.Color.SetColor(Color.Black);
                educationKind.Style.Border.Right.Color.SetColor(Color.Black);
                // 교육과정명
                var educationName = worksheet.Cells["B3:B4"];
                educationName.Style.Fill.PatternType = ExcelFillStyle.Solid;
                educationName.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                educationName.Merge = true;
                educationName.Value = "교 육 과 정 명";
                educationName.Style.Font.Name = "맑은 고딕";
                educationName.Style.Font.Bold = true;
                educationName.Style.Font.Size = 10;
                educationName.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                educationName.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                educationName.Style.Border.Top.Style = ExcelBorderStyle.Medium;
                educationName.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                educationName.Style.Border.Left.Style = ExcelBorderStyle.Medium;
                educationName.Style.Border.Right.Style = ExcelBorderStyle.Medium;
                educationName.Style.Border.Top.Color.SetColor(Color.Black);
                educationName.Style.Border.Bottom.Color.SetColor(Color.Black);
                educationName.Style.Border.Left.Color.SetColor(Color.Black);
                educationName.Style.Border.Right.Color.SetColor(Color.Black);

                // 교육일수
                var educationDay = worksheet.Cells["C3:C4"];
                educationDay.Style.Fill.PatternType = ExcelFillStyle.Solid;
                educationDay.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                educationDay.Merge = true;
                educationDay.Value = "교육\n일수";
                educationDay.Style.WrapText = true;
                educationDay.Style.Font.Name = "맑은 고딕";
                educationDay.Style.Font.Bold = true;
                educationDay.Style.Font.Size = 10;
                educationDay.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                educationDay.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                educationDay.Style.Border.Top.Style = ExcelBorderStyle.Medium;
                educationDay.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                educationDay.Style.Border.Left.Style = ExcelBorderStyle.Medium;
                educationDay.Style.Border.Right.Style = ExcelBorderStyle.Medium;
                educationDay.Style.Border.Top.Color.SetColor(Color.Black);
                educationDay.Style.Border.Bottom.Color.SetColor(Color.Black);
                educationDay.Style.Border.Left.Color.SetColor(Color.Black);
                educationDay.Style.Border.Right.Color.SetColor(Color.Black);

                // 교육시간
                var educationTime = worksheet.Cells["D3:D4"];
                educationTime.Style.Fill.PatternType = ExcelFillStyle.Solid;
                educationTime.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                educationTime.Merge = true;
                educationTime.Style.WrapText = true;
                educationTime.Value = "교육\n시간";
                educationTime.Style.Font.Name = "맑은 고딕";
                educationTime.Style.Font.Bold = true;
                educationTime.Style.Font.Size = 10;
                educationTime.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                educationTime.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                educationTime.Style.Border.Top.Style = ExcelBorderStyle.Medium;
                educationTime.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                educationTime.Style.Border.Left.Style = ExcelBorderStyle.Medium;
                educationTime.Style.Border.Right.Style = ExcelBorderStyle.Medium;
                educationTime.Style.Border.Top.Color.SetColor(Color.Black);
                educationTime.Style.Border.Bottom.Color.SetColor(Color.Black);
                educationTime.Style.Border.Left.Color.SetColor(Color.Black);
                educationTime.Style.Border.Right.Color.SetColor(Color.Black);

                // 교육기관
                var educationAgency = worksheet.Cells["E3:E4"];
                educationAgency.Style.Fill.PatternType = ExcelFillStyle.Solid;
                educationAgency.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                educationAgency.Merge = true;
                educationAgency.Value = "교육기관";
                educationAgency.Style.Font.Name = "맑은 고딕";
                educationAgency.Style.Font.Bold = true;
                educationAgency.Style.Font.Size = 10;
                educationAgency.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                educationAgency.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                educationAgency.Style.Border.Top.Style = ExcelBorderStyle.Medium;
                educationAgency.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                educationAgency.Style.Border.Left.Style = ExcelBorderStyle.Medium;
                educationAgency.Style.Border.Right.Style = ExcelBorderStyle.Medium;
                educationAgency.Style.Border.Top.Color.SetColor(Color.Black);
                educationAgency.Style.Border.Bottom.Color.SetColor(Color.Black);
                educationAgency.Style.Border.Left.Color.SetColor(Color.Black);
                educationAgency.Style.Border.Right.Color.SetColor(Color.Black);

                // 수강대상자
                var educationTarget = worksheet.Cells["F3:I3"];
                educationTarget.Style.Fill.PatternType = ExcelFillStyle.Solid;
                educationTarget.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                educationTarget.Merge = true;
                educationTarget.Value = "수 강 대 상 자";
                educationTarget.Style.Font.Name = "맑은 고딕";
                educationTarget.Style.Font.Bold = true;
                educationTarget.Style.Font.Size = 10;
                educationTarget.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                educationTarget.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                educationTarget.Style.Border.Top.Style = ExcelBorderStyle.Medium;
                educationTarget.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                educationTarget.Style.Border.Left.Style = ExcelBorderStyle.Medium;
                educationTarget.Style.Border.Right.Style = ExcelBorderStyle.Medium;
                educationTarget.Style.Border.Top.Color.SetColor(Color.Black);
                educationTarget.Style.Border.Bottom.Color.SetColor(Color.Black);
                educationTarget.Style.Border.Left.Color.SetColor(Color.Black);
                educationTarget.Style.Border.Right.Color.SetColor(Color.Black);

                // 소요예산
                var budget = worksheet.Cells["J3:L3"];
                budget.Style.Fill.PatternType = ExcelFillStyle.Solid;
                budget.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                budget.Merge = true;
                budget.Value = "소요예산(단위:천원)";
                budget.Style.Font.Name = "맑은 고딕";
                budget.Style.Font.Bold = true;
                budget.Style.Font.Size = 10;
                budget.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                budget.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                budget.Style.Border.Top.Style = ExcelBorderStyle.Medium;
                budget.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                budget.Style.Border.Left.Style = ExcelBorderStyle.Medium;
                budget.Style.Border.Right.Style = ExcelBorderStyle.Medium;
                budget.Style.Border.Top.Color.SetColor(Color.Black);
                budget.Style.Border.Bottom.Color.SetColor(Color.Black);
                budget.Style.Border.Left.Color.SetColor(Color.Black);
                budget.Style.Border.Right.Color.SetColor(Color.Black);

                // 소속팀
                var team = worksheet.Cells["F4"];
                team.Style.Fill.PatternType = ExcelFillStyle.Solid;
                team.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                team.Value = "소속팀";
                team.Style.Font.Name = "맑은 고딕";
                team.Style.Font.Bold = true;
                team.Style.Font.Size = 10;
                team.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                team.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                team.Style.Border.Top.Style = ExcelBorderStyle.Medium;
                team.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                team.Style.Border.Left.Style = ExcelBorderStyle.Medium;
                team.Style.Border.Right.Style = ExcelBorderStyle.Medium;
                team.Style.Border.Top.Color.SetColor(Color.Black);
                team.Style.Border.Bottom.Color.SetColor(Color.Black);
                team.Style.Border.Left.Color.SetColor(Color.Black);
                team.Style.Border.Right.Color.SetColor(Color.Black);

                // 직위
                var position = worksheet.Cells["G4"];
                position.Style.Fill.PatternType = ExcelFillStyle.Solid;
                position.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                position.Value = "직위";
                position.Style.Font.Name = "맑은 고딕";
                position.Style.Font.Bold = true;
                position.Style.Font.Size = 10;
                position.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                position.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                position.Style.Border.Top.Style = ExcelBorderStyle.Medium;
                position.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                position.Style.Border.Left.Style = ExcelBorderStyle.Medium;
                position.Style.Border.Right.Style = ExcelBorderStyle.Medium;
                position.Style.Border.Top.Color.SetColor(Color.Black);
                position.Style.Border.Bottom.Color.SetColor(Color.Black);
                position.Style.Border.Left.Color.SetColor(Color.Black);
                position.Style.Border.Right.Color.SetColor(Color.Black);

                // 사번
                var num = worksheet.Cells["H4"];
                num.Style.Fill.PatternType = ExcelFillStyle.Solid;
                num.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                num.Value = "사번";
                num.Style.Font.Name = "맑은 고딕";
                num.Style.Font.Bold = true;
                num.Style.Font.Size = 10;
                num.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                num.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                num.Style.Border.Top.Style = ExcelBorderStyle.Medium;
                num.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                num.Style.Border.Left.Style = ExcelBorderStyle.Medium;
                num.Style.Border.Right.Style = ExcelBorderStyle.Medium;
                num.Style.Border.Top.Color.SetColor(Color.Black);
                num.Style.Border.Bottom.Color.SetColor(Color.Black);
                num.Style.Border.Left.Color.SetColor(Color.Black);
                num.Style.Border.Right.Color.SetColor(Color.Black);

                // 성명
                var name = worksheet.Cells["I4"];
                name.Style.Fill.PatternType = ExcelFillStyle.Solid;
                name.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                name.Value = "성명";
                name.Style.Font.Name = "맑은 고딕";
                name.Style.Font.Bold = true;
                name.Style.Font.Size = 10;
                name.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                name.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                name.Style.Border.Top.Style = ExcelBorderStyle.Medium;
                name.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                name.Style.Border.Left.Style = ExcelBorderStyle.Medium;
                name.Style.Border.Right.Style = ExcelBorderStyle.Medium;
                name.Style.Border.Top.Color.SetColor(Color.Black);
                name.Style.Border.Bottom.Color.SetColor(Color.Black);
                name.Style.Border.Left.Color.SetColor(Color.Black);
                name.Style.Border.Right.Color.SetColor(Color.Black);

                // 수강료
                var educationFee = worksheet.Cells["J4"];
                educationFee.Style.Fill.PatternType = ExcelFillStyle.Solid;
                educationFee.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                educationFee.Value = "수강료";
                educationFee.Style.Font.Name = "맑은 고딕";
                educationFee.Style.Font.Bold = true;
                educationFee.Style.Font.Size = 10;
                educationFee.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                educationFee.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                educationFee.Style.Border.Top.Style = ExcelBorderStyle.Medium;
                educationFee.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                educationFee.Style.Border.Left.Style = ExcelBorderStyle.Medium;
                educationFee.Style.Border.Right.Style = ExcelBorderStyle.Medium;
                educationFee.Style.Border.Top.Color.SetColor(Color.Black);
                educationFee.Style.Border.Bottom.Color.SetColor(Color.Black);
                educationFee.Style.Border.Left.Color.SetColor(Color.Black);
                educationFee.Style.Border.Right.Color.SetColor(Color.Black);

                // 출장비
                var travelFee = worksheet.Cells["K4"];
                travelFee.Style.Fill.PatternType = ExcelFillStyle.Solid;
                travelFee.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                travelFee.Value = "출장비";
                travelFee.Style.Font.Name = "맑은 고딕";
                travelFee.Style.Font.Bold = true;
                travelFee.Style.Font.Size = 10;
                travelFee.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                travelFee.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                travelFee.Style.Border.Top.Style = ExcelBorderStyle.Medium;
                travelFee.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                travelFee.Style.Border.Left.Style = ExcelBorderStyle.Medium;
                travelFee.Style.Border.Right.Style = ExcelBorderStyle.Medium;
                travelFee.Style.Border.Top.Color.SetColor(Color.Black);
                travelFee.Style.Border.Bottom.Color.SetColor(Color.Black);
                travelFee.Style.Border.Left.Color.SetColor(Color.Black);
                travelFee.Style.Border.Right.Color.SetColor(Color.Black);

                // 계
                var sum = worksheet.Cells["L4"];
                sum.Style.Fill.PatternType = ExcelFillStyle.Solid;
                sum.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                sum.Value = "계";
                sum.Style.Font.Name = "맑은 고딕";
                sum.Style.Font.Bold = true;
                sum.Style.Font.Size = 10;
                sum.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sum.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                sum.Style.Border.Top.Style = ExcelBorderStyle.Medium;
                sum.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                sum.Style.Border.Left.Style = ExcelBorderStyle.Medium;
                sum.Style.Border.Right.Style = ExcelBorderStyle.Medium;
                sum.Style.Border.Top.Color.SetColor(Color.Black);
                sum.Style.Border.Bottom.Color.SetColor(Color.Black);
                sum.Style.Border.Left.Color.SetColor(Color.Black);
                sum.Style.Border.Right.Color.SetColor(Color.Black);

                // 월별일정
                var month = worksheet.Cells["M3:X3"];
                month.Style.Fill.PatternType = ExcelFillStyle.Solid;
                month.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                month.Merge = true;
                month.Value = "월 별 일 정";
                month.Style.Font.Name = "맑은 고딕";
                month.Style.Font.Bold = true;
                month.Style.Font.Size = 10;
                month.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                month.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                month.Style.Border.Top.Style = ExcelBorderStyle.Medium;
                month.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                month.Style.Border.Left.Style = ExcelBorderStyle.Medium;
                month.Style.Border.Right.Style = ExcelBorderStyle.Medium;
                month.Style.Border.Top.Color.SetColor(Color.Black);
                month.Style.Border.Bottom.Color.SetColor(Color.Black);
                month.Style.Border.Left.Color.SetColor(Color.Black);
                month.Style.Border.Right.Color.SetColor(Color.Black);


                // 1~12월
                string[] months = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
                string[] columns = { "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X" };

                for (int i = 0; i < months.Length; i++)
                {
                    var cell = worksheet.Cells[$"{columns[i]}4"];
                    cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    cell.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                    cell.Value = months[i];
                    cell.Style.Font.Name = "맑은 고딕";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 10;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell.Style.Border.Top.Style = ExcelBorderStyle.Medium;
                    cell.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    cell.Style.Border.Left.Style = ExcelBorderStyle.Medium;
                    cell.Style.Border.Right.Style = ExcelBorderStyle.Medium;
                    cell.Style.Border.Top.Color.SetColor(Color.Black);
                    cell.Style.Border.Bottom.Color.SetColor(Color.Black);
                    cell.Style.Border.Left.Color.SetColor(Color.Black);
                    cell.Style.Border.Right.Color.SetColor(Color.Black);
                }

                // 교육장소

                var spot = worksheet.Cells["Y3:Y4"];
                spot.Style.Fill.PatternType = ExcelFillStyle.Solid;
                spot.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                spot.Merge = true;
                spot.Style.WrapText = true;
                spot.Value = "교육\n장소";
                spot.Style.Font.Name = "맑은 고딕";
                spot.Style.Font.Bold = true;
                spot.Style.Font.Size = 10;
                spot.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                spot.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                spot.Style.Border.Top.Style = ExcelBorderStyle.Medium;
                spot.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                spot.Style.Border.Left.Style = ExcelBorderStyle.Medium;
                spot.Style.Border.Right.Style = ExcelBorderStyle.Medium;
                spot.Style.Border.Top.Color.SetColor(Color.Black);
                spot.Style.Border.Bottom.Color.SetColor(Color.Black);
                spot.Style.Border.Left.Color.SetColor(Color.Black);
                spot.Style.Border.Right.Color.SetColor(Color.Black);

             

                // 비고
                var description = worksheet.Cells["Z3:Z4"];
                description.Style.Fill.PatternType = ExcelFillStyle.Solid;
                description.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                description.Merge = true;
                description.Value = "비고";
                description.Style.Font.Name = "맑은 고딕";
                description.Style.Font.Bold = true;
                description.Style.Font.Size = 10;
                description.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                description.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                description.Style.Border.Top.Style = ExcelBorderStyle.Medium;
                description.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                description.Style.Border.Left.Style = ExcelBorderStyle.Medium;
                description.Style.Border.Right.Style = ExcelBorderStyle.Medium;
                description.Style.Border.Top.Color.SetColor(Color.Black);
                description.Style.Border.Bottom.Color.SetColor(Color.Black);
                description.Style.Border.Left.Color.SetColor(Color.Black);
                description.Style.Border.Right.Color.SetColor(Color.Black);

                // Add data
                for (int i = 0; i < mergedPlans.Count; i++)
                {
                    var plan = mergedPlans[i];
                    worksheet.Cells[i + 5, 1].Value = plan.KindOfEducation;
                    worksheet.Cells[i + 5, 2].Value = plan.EducationName;
                    worksheet.Cells[i + 5, 3].Value = plan.EducationDay;
                    worksheet.Cells[i + 5, 4].Value = plan.EducationTime;
                    worksheet.Cells[i + 5, 5].Value = plan.EducationAgency;
                    worksheet.Cells[i + 5, 6].Value = plan.Team;
                    worksheet.Cells[i + 5, 7].Value = plan.Position;
                    worksheet.Cells[i + 5, 8].Value = plan.Name;
                    worksheet.Cells[i + 5, 9].Value = plan.TuitionFee;
                    worksheet.Cells[i + 5, 10].Value = plan.TravelFee;
                    worksheet.Cells[i + 5, 11].Value = plan.SumOfFee;
                    worksheet.Cells[i + 5, 12].Value = plan.Janaury;
                    worksheet.Cells[i + 5, 13].Value = plan.Faburary;
                    worksheet.Cells[i + 5, 14].Value = plan.March;
                    worksheet.Cells[i + 5, 15].Value = plan.April;
                    worksheet.Cells[i + 5, 16].Value = plan.May;
                    worksheet.Cells[i + 5, 17].Value = plan.June;
                    worksheet.Cells[i + 5, 18].Value = plan.July;
                    worksheet.Cells[i + 5, 19].Value = plan.August;
                    worksheet.Cells[i + 5, 20].Value = plan.September;
                    worksheet.Cells[i + 5, 21].Value = plan.October;
                    worksheet.Cells[i + 5, 22].Value = plan.November;
                    worksheet.Cells[i + 5, 23].Value = plan.December;
                    worksheet.Cells[i + 5, 24].Value = plan.Spot;
                    worksheet.Cells[i + 5, 25].Value = plan.Reason;
                    worksheet.Cells[i + 5, 26].Value = plan.Planning;

                    for (int j = 12; j <= 23; j++)
                    {
                        worksheet.Cells[i + 5, j].Style.WrapText = true;
                    }
                }



                package.Save();
            }
        }
    }
}
