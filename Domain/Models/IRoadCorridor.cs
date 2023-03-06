using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public interface IRoadCorridor
    {
        string Id { get; set; }

        string? DisplayName { get; set; }

        string? StatusSeverity { get; set; }

        string? StatusSeverityDescription { get; set; }

        string? Bounds { get; set; }

        string? Envelope { get; set; }

        string? Url { get; set; }

        string RoadStatusMessage { get; }
    }
}
