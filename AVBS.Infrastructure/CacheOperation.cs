using System.Text;
using System.Web;
using System.Web.Caching;

namespace AVBS.Infrastructure {

    // TODO convert it <T> class notation
    public static class CacheOperation {

        public static object Get (string key ) {
            return HttpContext.Current.Cache.Get( key );
        }

        public static object Remove (string key ) {
            return HttpContext.Current.Cache.Remove( key ); ;
        }

        public static void Insert ( string key, object value ) {
            HttpContext.Current.Cache.Insert( key, value, null,
                   Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration,
                   CacheItemPriority.High, null );
        }
    }
}
