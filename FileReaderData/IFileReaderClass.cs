using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReaderData
{
    public interface IFileReaderClass
    {
        public string ProcessFilesAndConvertToExcel();

        public string ReadExcelData();
        
        public string Uploadfile(IFormFile file);
        
    }
}