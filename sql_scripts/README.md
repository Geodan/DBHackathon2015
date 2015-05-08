# SQL scripts for managing the data


## Add measures to linestrings based on start and end km from different table
```
UPDATE lines AS a SET geom = ST_AddMeasure(a.geom, b.m_start, b.m_end)
FROM km_data AS b WHERE a.id = b.id
```

