using System.ComponentModel.DataAnnotations;

namespace ZamawianiePosiłkowOnline.Models
{
    public class Reports
    {
        [Key]
        public int ID { get; set; }
        public string ReportType { get; set; }
        public string UserID {  get; set; }
        [Display(Name = "Czas")]
        public DateTime Time { get; set; }

    }
}