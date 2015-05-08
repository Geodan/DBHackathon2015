using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using Npgsql;
using Dapper;

namespace Blackbird.Services.Controllers
{
    public class Pinpoint
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
    }


    public class PinPointController : ApiController
    {
        private readonly string connectionString;
        private readonly List<Pinpoint> pinpoints;

        public PinPointController()
        {
            connectionString =ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
            pinpoints = new List<Pinpoint>();
        }

        // url http://localhost:60687/api/pinpoint?longitude=10&latitude=50
        public List<Pinpoint> GetPinPoint(double longitude, double latitude)
        {
            var wkt = String.Format("POINT({0} {1})", longitude, latitude);
            pinpoints.Add(GetDistrict(wkt));
            return pinpoints;
        }

        private Pinpoint GetDistrict(string wkt)
        {
            const string sql = @"select gid as id, name_3 as name
            from administrative.district d 
                where ST_Intersects(d.geom, ST_GeomFromText('POINT(10 50)',4326)) = true";

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var pinpointsRes = conn.Query<Pinpoint>(sql, new { Wkt = wkt});
                conn.Close();
                var pp = pinpointsRes.ToList()[0];
                pp.Type = "district";
                return pp;
            }
        }
    }
}

