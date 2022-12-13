using Core.Entites.Concrete;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework.Context
{
    public class ContextDb : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=94.73.170.76;Database=u0984114_dbBD0; Uid=u0984114_userBD0;Pwd=AhmeT1966ErzuruM;");
            //Server=94.73.170.76;Database=u0984114_dbBD0; Uid=u0984114_userBD0;Pwd=AhmeT1966ErzuruM;
        }
        public DbSet<User> Users { get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
        public DbSet<MailParameter> MailParameters { get; set; }
        public DbSet<ForgotPassword> ForgotPasswords { get; set; }
        public DbSet<MailTemplate> MailTemplates { get; set; }
        public DbSet<Ogis01> Ogis01 { get; set; }
        public DbSet<Ogis09> Ogis09 { get; set; }
        public DbSet<Ogis10> Ogis10 { get; set; }

    }
}
