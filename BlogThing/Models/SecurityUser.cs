using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogThing.Models
{
    public class SecurityUser
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public virtual List<SecurityGroup> Groups { get; set; }
    }
}
