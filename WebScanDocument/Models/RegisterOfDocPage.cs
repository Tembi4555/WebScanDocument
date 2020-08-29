using System;
using System.ComponentModel.DataAnnotations;

namespace WebScanDocument.Models
{
    public class RegisterOfDocPage
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Номер страницы")]
        public int PageNumber { get; set; }

        [Required]
        [Display(Name = "Актуальность")]
        public bool Relevance { get; set; }

        [Display(Name = "Период актуальности – начало")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime StartRelevance { get; set; }

        [Display(Name = "Период актуальности – окончание")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime StopRelevance { get; set; }

        [Required]
        [Display(Name = "Скан-образ  страницы")]
        [MaxLength(150, ErrorMessage = "Превышена максимальная длина записи")]
        public string ScanName { get; set; }

        [Display(Name = "ID документа")]
        public int? ListOfDocumentId { get; set; }
        public ListOfDocument ListOfDocument { get; set; }
    }
}