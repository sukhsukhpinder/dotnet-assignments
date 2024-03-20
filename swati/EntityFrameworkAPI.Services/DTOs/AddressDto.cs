using EntityFrameworkAPI.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace EntityFrameworkAPI.Services.DTOs
{
    [Serializable]
    public class AddressDto : BaseDto<AddressDto, StudentAddress>
    {
        [EmailAddress]
        public int StudentId { get; set; }
        [Required]
        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }


        public string Country { get; set; }

        [DataType(DataType.PostalCode)]
        public string ZipCode { get; set; }
    }

    public class StudentAddressDto
    {
        [XmlElement]
        public List<AddressDto> AddressDto { get; set; }
    }
}
