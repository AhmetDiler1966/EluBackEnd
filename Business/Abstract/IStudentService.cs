using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IStudentService
    {
        IDataResult<Ogis01> GetByOgrNo(string sOgrenciNo);
        IDataResult<List<Student>> GetListDto(string sOgrenciNo);
    }
}
