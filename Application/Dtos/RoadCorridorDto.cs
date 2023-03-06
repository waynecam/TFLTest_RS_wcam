using AutoMapper.Configuration.Annotations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class RoadCorridorDto
    {

        public string Id { get; set; }

        public string? DisplayName { get; set; }

        public string? StatusSeverity { get; set; }

        public string? StatusSeverityDescription { get; set; }

        public string? Bounds { get; set; }

        public string? Envelope { get; set; }

        public string? Url { get; set; }

        public string? RoadStatusMessage { get; set; }
       
    }
}
