using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFLTestApp.App
{
    public interface IRoadApp
    {
        Task<int> Run(string[] args);
    }
}
