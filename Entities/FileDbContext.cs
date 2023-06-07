using Microsoft.EntityFrameworkCore;

namespace FileConvertTask
{
    public class FileDbContext:DbContext
    {
        public FileDbContext(DbContextOptions<FileDbContext> options) : base(options) { }

        public DbSet<DataModel> DownloadTable { get; set; }
    }
}
