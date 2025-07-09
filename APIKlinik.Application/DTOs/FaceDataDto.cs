using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Application.DTOs
{
    public class FaceDataDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FaceImage { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateFaceDataDto
    {
        public int UserId { get; set; }
        public byte[] FaceEmbedding { get; set; }
        public string FaceImage { get; set; }
    }

    public class UpdateFaceDataDto
    {
        public byte[] FaceEmbedding { get; set; }
        public string FaceImage { get; set; }
        public bool IsActive { get; set; }
    }
}
