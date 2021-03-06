﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfoAPI.Models
{
    public class CiudadDto
    {
        //Sql server, sqlite,postgress,ibm
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumeroDePuntosDeInteres { get { return PointOfInterest.Count; } }

        public ICollection<PointOfInterestDto> PointOfInterest { get; set; } = new List<PointOfInterestDto>();
    }
}
