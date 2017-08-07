using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.EntityFramework
{
    public interface IObjectWithState
    {
        CustomEntityState EntityState { get; set; }
    }
}
