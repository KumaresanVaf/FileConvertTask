using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filelogicdata
{
    public interface IFilelogicclass
    {
        public string ProcessFilesAndConvertToExcel();
        public string ConvertedExcelToJson();
        public string Uploadfile(IFormFile file);
    }
}
