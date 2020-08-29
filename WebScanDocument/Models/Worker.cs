using System;
using System.ComponentModel.DataAnnotations;

namespace WebScanDocument.Models
{
    public class Worker
    {
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Табельный номер")]
        public int PersNumber { get; set; }

        [Required]
        [Display(Name = "Фамилия Имя Отчество")]
        [MaxLength(250, ErrorMessage = "Превышена максимальная длина записи")]
        public string FIO { get; set; }

        [Required]
        [Display(Name = "Пол")]
        [MaxLength(50, ErrorMessage = "Превышена максимальная длина записи")]
        public string Gender { get; set; }

        [Display(Name = "Дата рождения")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Должность")]
        [MaxLength(50, ErrorMessage = "Превышена максимальная длина записи")]
        public string Position { get; set; }
    }
}