# SQL scripts for managing the data


## Add measures to linestrings based on start and end km from different table
```
UPDATE lines AS a SET geom = ST_AddMeasure(a.geom, b.m_start, b.m_end)
FROM km_data AS b WHERE a.bahnstreckenummer = b.bahnstreckenummer
```

## Get line segments based on km data
For instance: you have speed data like
line	speed	kmfrom	kmto
86		120		435.2	480.5
```
SELECT ST_LocateBetween(a.geom,b.kmfrom, b.kmto) geom, b.speed
FROM lines AS a LEFT JOIN speedinfo AS b ON (a.line_id = b.line_id)
```