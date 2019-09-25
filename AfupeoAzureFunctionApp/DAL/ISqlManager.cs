using System;
using System.Collections.Generic;

namespace DAL
{
    public interface ISqlManager
    {
        string ExecQuery(string text, Dictionary<string, Object> parameters = null);
        string ExecProcedure(string procedure, Dictionary<string, Object> parameters = null);

    }
}