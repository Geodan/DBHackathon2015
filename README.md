## Blackbird

Geodan Micro Incident Management System build for dbhackathon2015 Berlin, may 2015

## Features:

- Intake incident
- Visualize incident
- Analyse incident (geocoding incident + regio detection + tunnel detection)
- Task analysis using rule based engine
- Send SMS notifications

## Techniques

- UI: WPF with Mapsui mapping library
- Services: ASP.NET Web API
- Database: PostgreSQL + PostGIS extension
- Server-side Mapping: Geoserver

## Libraries
- Mapsui https://github.com/pauldendulk/Mapsui
- Dapper https://github.com/StackExchange/dapper-dot-net
- Npgsql https://github.com/npgsql/npgsql
- Web API https://github.com/aspnet

## Data used

- kilometerpoints shape
- bahnstrecken shape
- tunnels shape

Basemap: Toner Light from Stamen
http://maps.stamen.com/toner-lite/#12/37.7706/-122.3782

## Screenshot


## Presentation powerpoint

https://github.com/Geodan/DBHackathon2015/blob/master/doc/Presentation%20team%20Geodan.pptx

## DB Bahn Web Api

- HTTP GET api/geocode?railnumber=:railnumber&kmpoint=:kmpoint

returns latitude, longitude of incident

- HTTP GET /api/pinpoint?longitude=:longitude&latitude=:latitude

returns distric information, is in tunnel and a list of tasks (send sms messages for now)

