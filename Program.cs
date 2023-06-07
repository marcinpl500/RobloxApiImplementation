using System;
using System.Collections.Generic;
using System.Linq;

namespace RobloxApiImplementation
{
    class Program
    {
        static void Main(string[] args)
        {
            string Cookie = "cookie";
            var Pg = new RobloxGroupCalls();
            Console.WriteLine(Pg.GetAuditLogV1(Cookie, 10818500, "ChangeRank", 333108214).Result.data.FirstOrDefault().actionType);
        }
    }
}
