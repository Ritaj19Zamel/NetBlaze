using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlaze.Domain.DatabaseObjects.CommonInterfaces
{
    public interface IDatabaseView
    {
        string CreateOrReplaceCommand { get; }
    }
}
