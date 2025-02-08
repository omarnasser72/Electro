using Electro.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electro.Core.Entities
{
    public class User : IdentityUser, IEntity
    {
        [Required]
        public string Name { get; set; }

        [NotMapped]
        [Required]
        public string Password { get; set; }

        public DateOnly Birthdate { get; set; }
    }
}
