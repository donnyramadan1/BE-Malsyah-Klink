using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Domain.Entities
{
    public class FaceData : BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public byte[] FaceEmbedding { get; set; }  // Vektor fitur wajah
        public string FaceImage { get; set; }      // Path gambar atau base64
        public bool IsActive { get; set; }

        public User? User { get; set; }

    }
}
