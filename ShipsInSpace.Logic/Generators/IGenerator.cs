using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipsInSpace.Logic.Generators
{
    interface IGenerator<T>
    {
        T Generate();
    }

    interface IGenerator : IGenerator<string>
    {
        
    }
}
