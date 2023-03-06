using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class RoadCorridor
    {

        public string Id { get; set; } = string.Empty;

        public string? DisplayName { get; set; }

        public string? StatusSeverity { get; set; }

        public string? StatusSeverityDescription { get; set; }

        public string? Bounds { get; set; }

        public string? Envelope { get; set; }

        public string? Url { get; set; }


        public string RoadStatusMessage { get
            {
                return $"The status of the {this.DisplayName ?? string.Empty} is as follows {Environment.NewLine}" +
                $"{Indent(3)}The road Status is {this.StatusSeverity ?? string.Empty} {Environment.NewLine}" +
                $"{Indent(3)}The road Status Description is {this.StatusSeverityDescription ?? string.Empty}";
            } }


        //public override string ToString()
        //{
        //    return $"The status of the {this.Id ?? string.Empty } is as follows {Environment.NewLine}" +
        //        $"{Indent(3)}The road Status is {this.StatusSeverity ?? string.Empty } {Environment.NewLine}" +
        //        $"{Indent(3)}The road Status Description is {this.StatusSeverityDescription ?? string.Empty }";
        //}

        private static string Indent(int count)
        {
            return "".PadLeft(count);
        }
    }
}
