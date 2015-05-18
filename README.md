## Blackbird

Geodan Micro Incident Management System build for dbhackathon2015, may 2015

## Features:

- Intake incident
- Visualize incident
- Analyse incident (tunnel detection)
- Task analysis using rule based engine
- Send notifications

## Techniques

- UI: WPF with Mapsui mapping library
- Services: ASP.NET Web API
- Database: Postgress + PostGIS extension
- Mapping: Geoserver

## Data

Data used: kilometerpoints, bahnstrecken and tunnels

## Screenshot



## DB Bahn Web Api

- HTTP GET api/geocode?railnumber=:railnumber&kmpoint=:kmpoint

returns latitude, longitude of incident

- HTTP GET /api/pinpoint?longitude=:longitude&latitude=:latitude

returns distric information, is in tunnel and a list of tasks (send sms messages for now)

