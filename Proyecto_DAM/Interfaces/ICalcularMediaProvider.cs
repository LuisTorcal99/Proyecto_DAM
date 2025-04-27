using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_DAM.Interfaces
{
    public interface ICalcularMediaProvider 
    {
        Task<string> CalcularMedia(int idAsignatura);
        Task<string> CalcularFaltas(int IdAsignatura);
    }
}
