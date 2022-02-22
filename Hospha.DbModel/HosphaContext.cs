using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospha.DbModel
{
    public class HosphaContext : IdentityDbContext<User>
    {
        public HosphaContext(DbContextOptions<HosphaContext> options)
            : base(options)
        {
        }
    }
}
