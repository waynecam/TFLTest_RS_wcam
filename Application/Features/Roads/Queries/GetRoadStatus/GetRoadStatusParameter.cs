using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Roads.Queries.GetRoadStatus
{
    public class GetRoadStatusParameter
    {
        private readonly string _roadCode;
        private readonly string _appid;
        private readonly string _appKey;
        public GetRoadStatusParameter(string roadCode,
            string appId,
            string appKey
            )
        {
            _roadCode = roadCode;
            _appid= appId;
            _appKey = appKey;
        }
        public string RoadCode
        {
            get { return _roadCode; }
        }

        public string AppKey => _appKey;

        public string Appid => _appid;
    }
}
