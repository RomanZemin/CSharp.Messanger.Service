using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Messanger.Application.DTOs
{
    public class UploadDTO
    {
        [Required]
        public int RoomId { get; set; }
        [Required]
        public IFormFile File { get; set; }
    }
}
