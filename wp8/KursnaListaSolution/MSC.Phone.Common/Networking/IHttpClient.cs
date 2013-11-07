using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSC.Phone.Common.Networking
{
    public interface IHttpClient
    {
        Task<T> GetJson<T>(string url);
    }
}
