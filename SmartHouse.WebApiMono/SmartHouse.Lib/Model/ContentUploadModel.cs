using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
    public class ContentUploadModel
    {
        [Required]
        public string ContentBase64 { get; set; }
        [Required]
        public string Application { get; set; }
        [Required]
        public ContentCategoryEnum? ContentCategoryEnum { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string[] Tags { get; set; }
    }
}
