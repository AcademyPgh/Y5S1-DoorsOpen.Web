using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogThing.Models
{
    public class SecurityGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<SecurityUser> Users { get; set; }
    }
}
