using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Schemas
{
    public class JWTSetting
    {
        public TimeSpan Lifetime { get; set; }
        public string? Key { get; set; }
    }
}
