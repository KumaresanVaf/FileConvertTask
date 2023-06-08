using FileConvertTask;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace FileReaderData
{
    public class FileReaderClass : IFileReaderClass
    {
        private IHostingEnvironment _hostingEnvironment;
        private readonly FileDbContext _fileDbContext;

        public FileReaderClass(IHostingEnvironment hostingEnvironment, FileDbContext fileDbContext)
        {
            _hostingEnvironment = hostingEnvironment;
            _fileDbContext = fileDbContext;
        }

        public string ProcessFilesAndConvertToExcel()
        {
            string folderPath = @"\\192.168.0.5\vaf\task\JSON";
            string[] files = Directory.GetFiles(folderPath, "*.json");

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //string startupPath = System.IO.Directory.GetCurrentDirectory();
            //string getfilepath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string saveDirectoryPath = Path.Combine(_hostingEnvironment.WebRootPath, "convert", "JsonToExcel");
            if (File.Exists(saveDirectoryPath))
            {
                Directory.CreateDirectory(saveDirectoryPath);
            }

            string[] convertedFiles = Directory.GetFiles(saveDirectoryPath, "*.xlsx");
            List<string> convertedFileNames = new List<string>(convertedFiles.Select(Path.GetFileNameWithoutExtension));

            try
            {
                foreach (string filePath in files)
                {
                    string fileName = Path.GetFileNameWithoutExtension(filePath);

                    if (convertedFileNames.Contains(fileName))
                    {
                        continue;
                    }

                    try
                    {
                        string jsonContent = System.IO.File.ReadAllText(filePath);
                        var data = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonContent);

                        if (data.Count > 0)
                        {
                            using (ExcelPackage excelPackage = new ExcelPackage())
                            {
                                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("EnrollmentData");

                                int headerRowIndex = 1;
                                int columnIndex = 1;

                                var allKeys = data.SelectMany(dict => dict.Keys).Distinct();//

                                foreach (var key in allKeys)
                                {
                                    worksheet.Cells[headerRowIndex, columnIndex].Value = key;
                                    columnIndex++;
                                }

                                int dataRowIndex = headerRowIndex + 1;
                                foreach (var dict in data)
                                {
                                    columnIndex = 1;
                                    foreach (var key in allKeys)
                                    {
                                        var value = dict.ContainsKey(key) ? dict[key] : "null";

                                        worksheet.Cells[dataRowIndex, columnIndex].Value = value;
                                        columnIndex++;
                                    }
                                    dataRowIndex++;
                                }
                                worksheet.Cells.AutoFitColumns();

                                string excelFilePath = Path.Combine(saveDirectoryPath, fileName + ".xlsx");
                                excelPackage.SaveAs(new FileInfo(excelFilePath));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error converting file '{fileName}': {ex.Message}");
                    }
                }
                return saveDirectoryPath;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error processing files: {ex.Message}");
            }
        }

        public string ReadExcelData()
        {
            var FolderName = @"\\192.168.0.5\vaf\task\EXCEL";
            string[] files = Directory.GetFiles(FolderName, "*.xlsx");
            string saveDirectoryPath = Path.Combine(_hostingEnvironment.WebRootPath, "convert", "ExcelToJson");
            if (File.Exists(saveDirectoryPath))
            {
                Directory.CreateDirectory(saveDirectoryPath);
            }
            string[] convertedfilePath = Directory.GetFiles(saveDirectoryPath, "*.json");
            var convertedFileNames = new List<string>(convertedfilePath.Select(Path.GetFileNameWithoutExtension));

            try
            {
                foreach (string file in files)
                {
                    string Filename = Path.GetFileNameWithoutExtension(file);
                    if (convertedFileNames.Contains(Filename))
                    {
                        continue;
                    }

                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    using (var excelPackage = new ExcelPackage(file))
                    {
                        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];

                        List<string> columnNames = new List<string>();
                        int columnCount = worksheet.Dimension.Columns;
                        for (int column = 1; column <= columnCount; column++)
                        {
                            columnNames.Add(worksheet.Cells[1, column].Value.ToString());
                        }

                        var jsonData = new List<Dictionary<string, object>>();
                        int rowCount = worksheet.Dimension.Rows;
                        for (int row = 2; row <= rowCount; row++)
                        {
                            Dictionary<string, object> rowData = new Dictionary<string, object>();
                            for (int column = 1; column <= columnCount; column++)
                            {
                                string columnName = columnNames[column - 1];
                                object cellValue = worksheet.Cells[row, column].Value;
                                rowData[columnName] = cellValue;
                            }
                            jsonData.Add(rowData);
                        }

                        string json = JsonConvert.SerializeObject(jsonData, Formatting.Indented);
                        string saveFilePath = Path.Combine(saveDirectoryPath, Filename + ".json");
                        System.IO.File.WriteAllText(saveFilePath, json);
                    }
                }
                return saveDirectoryPath;
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading Excel data: " + ex.Message);
            }
        }

        public string Uploadfile(IFormFile file)
        {
            //var database = _fileDbContext.DownloadTable;
            string fileName = Path.GetFileNameWithoutExtension(file.FileName);
            string fileExtension = Path.GetExtension(file.FileName);

            string UploadsavePath = Path.Combine(_hostingEnvironment.WebRootPath, "convert", "UploadFile");
            Directory.CreateDirectory(UploadsavePath);
            string filePath = Path.Combine(UploadsavePath, file.FileName);

            /* string[] uploadFilespath = Directory.GetFiles(UploadsavePath, "*");
             var convertedFileNames = new List<string>(uploadFilespath.Select(Path.GetFileNameWithoutExtension));*/

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            string convertedFileName;
            string convertedFilePath;

            if (fileExtension == ".xlsx")
            {
                string saveDirectoryPath = Path.Combine(_hostingEnvironment.WebRootPath, "convert", "Upload", "ExcelToJson");

                string[] UploadconvertedfilePath = Directory.GetFiles(saveDirectoryPath, "*.json");
                var UploadconvertedFileNames = new List<string>(UploadconvertedfilePath.Select(Path.GetFileNameWithoutExtension));

                if (UploadconvertedFileNames.Contains(fileName))
                {
                    return null;
                }

                if (File.Exists(saveDirectoryPath))
                {
                    Directory.CreateDirectory(saveDirectoryPath);
                }
                string convertedFilePath1 = Path.Combine(saveDirectoryPath, fileName + ".json");

                using (var stream = new FileStream(convertedFilePath1, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var excelPackage = new ExcelPackage(new FileInfo(convertedFilePath1)))
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];

                    List<string> columnNames = new List<string>();
                    int columnCount = worksheet.Dimension.Columns;
                    for (int column = 1; column <= columnCount; column++)
                    {
                        columnNames.Add(worksheet.Cells[1, column].Value.ToString());
                    }

                    List<Dictionary<string, object>> jsonData = new List<Dictionary<string, object>>();
                    int rowCount = worksheet.Dimension.Rows;
                    for (int row = 2; row <= rowCount; row++)
                    {
                        Dictionary<string, object> rowData = new Dictionary<string, object>();
                        for (int column = 1; column <= columnCount; column++)
                        {
                            string columnName = columnNames[column - 1];
                            object cellValue = worksheet.Cells[row, column].Value;
                            rowData[columnName] = cellValue;
                        }
                        jsonData.Add(rowData);
                    }

                    string json = JsonConvert.SerializeObject(jsonData, Formatting.Indented);

                    convertedFileName = fileName + ".json";
                    convertedFilePath = Path.Combine(saveDirectoryPath, convertedFileName);
                    var foldrepath = Path.Combine("convert", "Upload", "ExcelToJson", convertedFileName);
                    var dbpath = foldrepath;
                    System.IO.File.WriteAllText(convertedFilePath, json);

                    /* DataModel download = new DataModel
                     {
                         FileName = convertedFileName,
                         FilePath = dbpath
                     };
                     database.Add(download);
                     _fileDbContext.SaveChanges();*/

                    return convertedFilePath;
                }
            }
            else if (fileExtension == ".json")
            {
                string saveDirectoryPath = Path.Combine(_hostingEnvironment.WebRootPath, "convert", "Upload", "JsonToExcel");
                if (File.Exists(saveDirectoryPath))
                {
                    Directory.CreateDirectory(saveDirectoryPath);
                }
                string convertedFilePath1 = Path.Combine(saveDirectoryPath, fileName + ".xlsx");

                string[] UploadconvertedfilePath = Directory.GetFiles(saveDirectoryPath, "*.xlsx");
                var UploadconvertedFileNames = new List<string>(UploadconvertedfilePath.Select(Path.GetFileNameWithoutExtension));

                if (UploadconvertedFileNames.Contains(fileName))
                {
                    return null;
                }

                using (var stream = new FileStream(convertedFilePath1, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                string jsonContent = System.IO.File.ReadAllText(convertedFilePath1);
                var data = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonContent);

                if (data.Count > 0)
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (ExcelPackage excelPackage = new ExcelPackage())
                    {
                        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("EnrollmentData");

                        int headerRowIndex = 1;
                        int columnIndex = 1;

                        var allKeys = data.SelectMany(dict => dict.Keys).Distinct();

                        foreach (var key in allKeys)
                        {
                            worksheet.Cells[headerRowIndex, columnIndex].Value = key;
                            columnIndex++;
                        }

                        int dataRowIndex = headerRowIndex + 1;
                        foreach (var rowvalue in data)
                        {
                            columnIndex = 1;
                            foreach (var key in allKeys)
                            {
                                var value = rowvalue.ContainsKey(key) ? rowvalue[key] : "null";

                                worksheet.Cells[dataRowIndex, columnIndex].Value = value;
                                columnIndex++;
                            }
                            dataRowIndex++;

                        }
                        worksheet.Cells.AutoFitColumns();

                        convertedFileName = fileName + ".xlsx";
                        convertedFilePath = Path.Combine(saveDirectoryPath, convertedFileName);
                        excelPackage.SaveAs(new FileInfo(convertedFilePath));

                        /* DataModel download = new DataModel
                         {
                             FileName = convertedFileName,
                             FilePath = convertedFilePath
                         };
                         database.Add(download);
                         _fileDbContext.SaveChanges();*/
                    }
                }
                return convertedFilePath1;
            }
            else
            {
                throw new Exception("Unsupported file format. Only .xlsx and .json files are allowed.");
            }
        }
    }
}