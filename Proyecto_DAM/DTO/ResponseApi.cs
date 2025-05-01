using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_DAM.DTO
{
    public class ResponseApi
    {
        public bool IsSuccess { get; set; }
        public string DisplayMessage { get; set; }
        public List<string> ErrorMessages { get; set; } = new();
        public object Result { get; set; }
    }

}
