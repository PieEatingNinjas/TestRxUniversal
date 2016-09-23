using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using TestRxUniversal.Model;

namespace TestRxUniversal.DataService
{
    public static class SearchDataService
    {
        internal static IObservable<IEnumerable<Result>> Search(string qry)
        {
            return Observable.FromAsync(() => SearchAsync(qry));
        }

        internal static async Task<IEnumerable<Result>> SearchAsync(string qry)
        {
            if (qry.Length < 3)
                return null;

            try
            {
                string uri = string.Format(
                    "http://freeimages.pictures/api/user/{0}/?keyword={1}&format=json",
                    App.USERID, qry);

                string jsonResult = string.Empty;

                using (var httpClient = new HttpClient())
                {
                    jsonResult = await httpClient.GetStringAsync(uri);
                }

                var searchResult = JsonConvert.DeserializeObject<SearchResultRootobject>(jsonResult);
                return searchResult.sources.SelectMany(a => a.result).Take(20);
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal static async Task<IEnumerable<Result>> SearchAsync(string qry, CancellationToken token)
        {
            if (qry.Length < 3)
                return null;

            try
            {
                string uri = string.Format(
                    "http://freeimages.pictures/api/user/{0}/?keyword={1}&format=json",
                    App.USERID, qry);

                string jsonResult = string.Empty;

                using (var httpClient = new HttpClient())
                {
                    token.ThrowIfCancellationRequested();
                    jsonResult = await httpClient.GetStringAsync(uri);
                    token.ThrowIfCancellationRequested();
                }

                var searchResult = JsonConvert.DeserializeObject<SearchResultRootobject>(jsonResult);
                return searchResult.sources.SelectMany(a => a.result).Take(20);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
