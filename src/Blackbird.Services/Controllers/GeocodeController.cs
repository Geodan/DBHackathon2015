using System.Configuration;
using System.Linq;
using System.Web.Http;
using Dapper;
using Npgsql;

namespace Blackbird.Services.Controllers
{
    public class GeocodeController : ApiController
    {
        private readonly string connectionString;
        public GeocodeController()
        {
              connectionString =ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
        }
        // url http://localhost:60687/api/geocode?railnumber=1000&kmpoint=174
        public Position GetPosition(int railnumber, int kmPoint)
        {
            const string sql = "select ST_X(ST_GeomFromWKB(geom, 4326)) as Longitude,ST_Y(ST_GeomFromWKB(geom, 4326)) as Latitude from dbbahn.kilometerpoints where km=@km and streckennu=@rn";

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var positionsRes = conn.Query<Position>(sql, new { Km = kmPoint,rn=railnumber  });
                conn.Close();
                var pp = positionsRes.ToList()[0];
                return pp;
            }
        }
    }
}