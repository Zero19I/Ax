﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Permiso
    {
        public int PkPermiso_Id { get; set; }
        public Rol FkRol_Id { get; set; }
        public string NombreMenu { get; set; }
        public string FechaRegistro{ get; set; }
    }
}
