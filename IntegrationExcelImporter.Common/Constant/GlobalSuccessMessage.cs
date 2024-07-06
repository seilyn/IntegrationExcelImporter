using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationExcelImporter.Common.Constant
{
    public static class GlobalSuccessMessage
    {
        public const string SUCCESS_MESSAGE = "작업이 완료되었습니다.";

        public const string SUCCESS_CREATE_MERGED_EXCELDATA = "엑셀 파일 생성이 완료되었습니다.";

        public const string FAILURE_CREATE_MERGED_EXCELDATA = "엑셀 파일 생성에 실패했습니다. 경로가 올바르지 않거나 파일명이 중복되었습니다.";
    }
}
