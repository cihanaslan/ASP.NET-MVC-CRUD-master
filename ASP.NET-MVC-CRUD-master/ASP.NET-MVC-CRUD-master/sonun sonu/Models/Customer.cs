using System.ComponentModel.DataAnnotations;

namespace sonun_sonu.Models
{
    public class Customer
    {
        public int WM_ID { get; set; }
        public int WM_CODE { get; set; }

        [Required]
        public string WM_NAME { get; set; }

        [Required]
        public string WM_SURNAME { get; set; }

    }
}

