using System;
using System.Data.Entity;
using System.IO;
using System.Threading.Tasks;
using Databas;
using File = Databas.Models.File;
using FileInfo = Kzta.Domain.Files.Models.FileInfo;

namespace Kzta.Domain.Files
{
    /// <summary>
    /// Класс для файлов
    /// </summary>
    public class FileService
    {
        private readonly DatabaseContext _context;
        private readonly FileOptions _fileOptions;
        public FileService(DatabaseContext context, FileOptions fileOptions)
        {
            _context = context;
            _fileOptions = fileOptions;
        }

        /// <summary>
        /// Загрузить файл
        /// </summary>
        /// <returns></returns>
        public async Task<Guid> UploadFile(FileInfo fileModel)
        {
            var fileEnt = new File("", fileModel.FileName);
            var path = GetPath(fileEnt.Name.ToString());
            fileEnt.Path = path;
            using (var file = new FileStream(path, FileMode.CreateNew, FileAccess.ReadWrite))
            {
                fileModel.Content.Seek(0, SeekOrigin.Begin);
                fileModel.Content.CopyTo(file);
            }

            _context.Files.Add(fileEnt);
            await _context.SaveChangesAsync();
            return fileEnt.FileGuid;
        }

        public async Task<string> GetPath(Guid fileGuid)
        {
            var file = await _context.Files.SingleOrDefaultAsync(x => x.FileGuid == fileGuid);
            return Path.Combine(_fileOptions.BaseRoot, file.Path);
        }

        private string GetPath(string fileName)
        {
            var path = Path.Combine(_fileOptions.BaseRoot, fileName);
            if (!Directory.Exists(_fileOptions.BaseRoot))
                Directory.CreateDirectory(_fileOptions.BaseRoot);
            var count = 1;
            while (System.IO.File.Exists(path))
            {
                path = Path.Combine(_fileOptions.BaseRoot, $"{count}){fileName}");
                count++;
            }
              
            return path;
        }
    }
}
