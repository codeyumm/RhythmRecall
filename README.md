### TODO


- api method to get all songs of user - done
- api method to get discoverd songs of user

```
     List<TrackList> tracks = db.TrackLists
  
    .Where(track => track.UserId == id)
  
    .Where(track => track.Name == "MyTrackList")
  
    .ToList();  
```

- api method to get listen late song sof user

- api method to add discoverd songs of user
- api method to add listen late song sof user

- login/signup
