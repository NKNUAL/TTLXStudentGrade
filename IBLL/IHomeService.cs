using IBLL.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLL
{
    public interface IHomeService : IDependency
    {

        List<Module> GetMenuModule();

    }
}
