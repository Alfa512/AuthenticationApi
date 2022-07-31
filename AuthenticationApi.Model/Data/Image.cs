using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationApi.Model.Data
{
    [Table("Images")]
    public class Image
    {
        public Int64 Id { get; set; }
        public Guid ImageIdentifier { get; set; }
        public DateTime CreateDate { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string Path { get; set; }
    }
}