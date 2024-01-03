using System.ComponentModel.DataAnnotations;

namespace PGPInformation.Models
{
    public class SubmitViewModel
    {
        [DataType(DataType.MultilineText)]
        public string? Input { get; set; }  
    }
}
