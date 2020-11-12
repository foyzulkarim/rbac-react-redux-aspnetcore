using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AuthWebApplication.Models
{
    public interface IIndex
    {
        void BuildIndices(ModelBuilder builder);
    }
}
