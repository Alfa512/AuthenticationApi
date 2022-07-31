using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationApi.Model.Data
{
    [Table("Configuration")]
    public class Configuration
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool IsActive { get; set; }
    }
}