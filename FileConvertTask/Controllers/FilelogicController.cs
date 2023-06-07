using Filelogicdata;
using Microsoft.AspNetCore.Mvc;

namespace FileConvertTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilelogicController : ControllerBase
    {
        private readonly IFilelogicclass filelogicclass;
        private readonly FileDbContext _fileDbContext;

        public FilelogicController(IFilelogicclass filelogicclass, FileDbContext fileDbContext)
        {
            this.filelogicclass = filelogicclass;
            _fileDbContext = fileDbContext;
        }

        [HttpGet]
        [Route("ConvertToExcel")]
        public IActionResult Get()
        {
            try
            {
                var result = filelogicclass.ProcessFilesAndConvertToExcel();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("ConvertToJSON")]
        public IActionResult ConvertToJSON()
        {
            try
            {
                var result = filelogicclass.ConvertedExcelToJson();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while converting Excel to JSON." + ex.Message);
            }
        }

        [HttpPost]
        [Route("upload")]
        public IActionResult UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            try
            {
                filelogicclass.Uploadfile(file);
                return Ok("File uploaded and converted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while uploading the file: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetData(int id)
        {
            DataModel download = _fileDbContext.DownloadTable.Find(id);
            var path = "https://localhost:7292/" + download.FilePath;
            if (download == null)
            {
                return NotFound();
            }

            return Ok(path);
        }
    }
}