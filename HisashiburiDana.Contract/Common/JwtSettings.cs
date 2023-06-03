﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Contract.Common
{
    public class JwtSettings
    {
        public string SecretKey { get; set; }

        public string Audience { get; set; }

        public string Issuer { get; set; }

        public int ExpiryMinutes { get; set; }
    }
}
