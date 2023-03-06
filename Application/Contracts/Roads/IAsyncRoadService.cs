using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Roads
{
    public interface IAsyncRoadService
    {

        //Probably considered unusual to return a collection when a single entity would suffice
        //but I do recall a design pattern article suggesting returning an empty collection and not null
        //in the absence of a return result - to infer expected behaviour from erroneous behaviour
        //Alternatively the Null Object Design pattern might have been used - but this might have required type checking down the line
        Task<ICollection<IRoadCorridor>> GetRoadCorridorAsync(string roadId,
            string appid, string appkey);
    }
}
