using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Databas.Models
{
    /// <summary>
    /// Класс, предоставляющий файл 
    /// </summary>
    public class File
    {
        /// <summary>
        /// Id файла, под ним файл храниться в  хранилище
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid FileGuid { get; set; }

        /// <summary>
        /// Тип файла
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Path { get; set; }

        /// <summary>
        /// Имя файла
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary/>
        public File()
        {
        }

        /// <summary/>
        public File(string path, string name)
        {
            FileGuid = Guid.NewGuid();
            Path = path;
            Name = name;
        }
    }
}
