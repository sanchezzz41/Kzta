using System.IO;

namespace Kzta.Domain.Files.Models
{
    /// <summary>
    /// Входная модель для файлов
    /// </summary>
    public class FileInfo
    {
        public Stream Content { get; set; }

        public string FileName { get; set; }

        public string Path { get; set; }

        public FileInfo()
        {
            Content = new MemoryStream();
        }
    }
}
