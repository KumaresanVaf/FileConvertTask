using FileReaderData;
using Microsoft.AspNetCore.Http;

namespace Filelogicdata
{
    public class Filelogicclass : IFilelogicclass
    {
        private readonly IFileReaderClass _fileReader;

        public Filelogicclass(IFileReaderClass fileReader)
        {
            _fileReader = fileReader;
        }

        public string ProcessFilesAndConvertToExcel()
        {
            return _fileReader.ProcessFilesAndConvertToExcel();
        }

        public string ConvertedExcelToJson()
        {
            return _fileReader.ReadExcelData();
        }

        public string Uploadfile(IFormFile file)
        {
            return _fileReader.Uploadfile(file);
        }
    }
}