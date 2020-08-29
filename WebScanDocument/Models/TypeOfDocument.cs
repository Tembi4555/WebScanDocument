using System;
using System.ComponentModel.DataAnnotations;

namespace WebScanDocument.Models
{
    public class TypeOfDocument
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Код вида документа")]
        public int DocumentTypeCode { get; set; }

        [Required]
        [Display(Name = "Вид документа")]
        [MaxLength(50, ErrorMessage = "Превышена максимальная длина записи")]
        public string TypeOfDoc { get; set; }
    }
}