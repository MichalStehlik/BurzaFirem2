using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace BurzaFirem2.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [Column(TypeName = "datetime2")]
        public DateTime Created { get; set; } = DateTime.Now;
        [Column(TypeName = "datetime2")]
        public DateTime Updated { get; set; } = DateTime.Now;
    }
}
