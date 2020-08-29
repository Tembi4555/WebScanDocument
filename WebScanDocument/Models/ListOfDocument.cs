using System;
using System.ComponentModel.DataAnnotations;

namespace WebScanDocument.Models
{
    public class ListOfDocument
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Название документа")]
        [MaxLength(250, ErrorMessage = "Превышена максимальная длина записи")]
        public string NameDocument { get; set; }

        [Required]
        [Display(Name = "Серия документа")]
        [MaxLength(50, ErrorMessage = "Превышена максимальная длина записи")]
        public string SeriesDocument { get; set; }

        [Required]
        [Display(Name = "Номер документа")]
        public int NumderDocument { get; set; }

        [Required]
        [Display(Name = "Кем выдан")]
        [MaxLength(150, ErrorMessage = "Превышена максимальная длина записи")]
        public string IssuedBy { get; set; }

        [Display(Name = "Когда выдан")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime WhenIssued { get; set; }

        [Display(Name = "Срок действия – начало")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime StartValidity { get; set; }

        [Display(Name = "Срок действия – окончание")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime StopValidity { get; set; }

        [Required]
        [Display(Name = "Количество страниц")]
        public int NumberOfPages { get; set; }

        [Display(Name = "Дата редактирования")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime EditingDate { get; set; }

        [Display(Name = "Табельный номер работника")]
        public int? WorkerId { get; set; }

        public Worker Worker { get; set; }

        [Display(Name = "Вид длокумента")]
        public int? TypeOfDocumentId { get; set; }

        public TypeOfDocument TypeOfDocument { get; set; }
    }
}