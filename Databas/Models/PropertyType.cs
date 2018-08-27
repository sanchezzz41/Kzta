using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Databas.Models
{
    /// <summary>
    /// Тип имущества
    /// </summary>
    public class PropertyType : INotifyPropertyChanged
    {
        private string _description;
        private string _name;
        private Guid _propertyTypeGuid;

        /// <summary>
        /// Id типа имущества
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid PropertyTypeGuid
        {
            get => _propertyTypeGuid;
            set
            {
                _propertyTypeGuid = value;
                OnPropertyChanged();
            }

        }

        /// <summary>
        /// Название типа имущества
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Описание
        /// </summary>
        [MaxLength(500)]
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }


        /// <summary>
        /// Имущество данного типа
        /// </summary>
        public virtual ICollection<Detail> Details { get; set; }

        /// <inheritdoc />
        public PropertyType()
        {
            PropertyTypeGuid = Guid.NewGuid();
        }

        /// <inheritdoc />
        public PropertyType(string name, string description)
        {
            PropertyTypeGuid = Guid.NewGuid();
            Name = name;
            Description = description;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}