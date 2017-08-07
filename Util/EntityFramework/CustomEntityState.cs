using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.EntityFramework
{
  public enum CustomEntityState
  {
    Unchanged = 0,
    Added = 1,
    Updated = 2,
    Deleted = 3
  }
}
