using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChequePrint.Models
{
    public class ChequeModel
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100, ErrorMessage = "Name should be maximum 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [DataType(DataType.Currency, ErrorMessage = "Only numbers are allowed")]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Only amount with maximum 2 decimal places is allowed")]
        [Range(0, 999999999999.99, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public string Amount { get; set; }
    }
}