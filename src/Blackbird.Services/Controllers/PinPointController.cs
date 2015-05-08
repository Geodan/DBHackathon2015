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
        public string District { get; set; }
        public bool IsInTunnel { get; set; }
    }


    public class PinPointController : ApiController
    {
        private readonly string connectionString;

        public PinPointController()
        {
            connectionString =ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
        }

        // url http://localhost:60687/api/pinpoint?longitude=10&latitude=50
        public Pinpoint GetPinPoint(double longitude, double latitude)
        {
            var wkt = String.Format("POINT({0} {1})", longitude, latitude);
            var pp = new Pinpoint {District = GetDistrict(wkt), IsInTunnel = true};
            return pp;
        }

        private string GetDistrict(string wkt)
        {
            const string sql = @"select name_1 || ', ' || name_2 || ', ' || name_3 as name
            from administrative.district d 
                where ST_Intersects(d.geom, ST_GeomFromText(@wkt,4326)) = true";

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var district = conn.Query<String>(sql, new { Wkt = wkt});
                conn.Close();
                return district.ToList()[0];
            }
        }

        private Pinpoint GetTunnel(string wkt)
        {
            // sample coordinate 10.0339239315153 53.6273756844966
            var sql = @"select geom
                from dbbahn.tunnel t 
                where ST_Intersects(t.geom, ST_GeomFromText('POINT(10.0339239315153 53.6273756844966)',4326)) = true";

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var pinpointsRes = conn.Query<Pinpoint>(sql, new { Wkt = wkt });
                conn.Close();
                var pp = pinpointsRes.ToList()[0];
                //pp.Type = "district";
                //return pp;
                return null;
            }
        }

    }
}

