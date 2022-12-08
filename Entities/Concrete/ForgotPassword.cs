using Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class ForgotPassword : IEntity
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int userId { get; set; }
        public DateTime SendDate { get; set; }
        public bool IsActive { get; set; }
    }
}
