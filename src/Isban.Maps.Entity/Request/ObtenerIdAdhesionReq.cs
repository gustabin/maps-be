﻿using Isban.Maps.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Isban.Maps.Entity.Request
{
    [DataContract]
    public class ObtenerIdAdhesionReq : EntityBase
    {
        [DataMember]
        public long IdCuentaPdc { get; set; } 

    }
}
