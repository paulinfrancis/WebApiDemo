using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace WebApiDemoCommon.Models
{
    [DataContract]
    public class Camera
    {
        [Required]
        [DataMember(IsRequired = true)] 
        public int? Id { get; set; }

        [Required]
        [DataMember(IsRequired = true)] 
        public string Brand { get; set; }

        [Required]
        [DataMember(IsRequired = true)] 
        public string Model { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public decimal? Price { get; set; }

        [Required]
        [DataMember(IsRequired = true)] 
        public string Description { get; set; }
    }
}
