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
        public string Tasks { get; set; }
    }

    public class PinPointController : ApiController
    {
        private readonly string connectionString;
        private string tel1 = "+31622378864";
        private string tel2 = "0031062237";

        public PinPointController()
        {
            connectionString =ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
        }

        // tunnelurl2: http://localhost:60687/api/pinpoint?latitude=50.841063222070794&longitude=9.59353920394821
        // tunnelurl = http://localhost:60687/api/pinpoint?longitude=10.0339239315153&latitude=53.6273756844966
        // url http://localhost:60687/api/pinpoint?longitude=10&latitude=50
        public Pinpoint GetPinPoint(double longitude, double latitude)
        {
            var wkt = String.Format("POINT({0} {1})", longitude, latitude);
            var pp = new Pinpoint {District = GetDistrict(wkt), IsInTunnel = IsInTunnel(wkt)};
            pp = addTasks(pp);
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

        private bool IsInTunnel(string wkt)
        {
            var sql = @"select gid
                from dbbahn.tunnel t 
                where ST_Intersects(t.geom, st_buffer(ST_GeomFromText(@Wkt,4326),0.01)) = true";
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var pinpointsRes = conn.Query<int>(sql, new { Wkt = wkt });
                conn.Close();
                var isintunnel = pinpointsRes.ToList().Count>0;

                return isintunnel;
            }
        }

        private Pinpoint addTasks(Pinpoint pinpoint)
        {
            var tasks = String.Empty;

            // rule 1 send sms to 

            tasks += "Send SMS message, police " + pinpoint.District.Split(',')[2] +"," + tel1 +"#";
            if (pinpoint.IsInTunnel)
            {
                tasks += "Send SMS message, Angela Merkel " + "," + tel2;
            }

            pinpoint.Tasks = tasks;
            return pinpoint;

        }
    }
}

