# Main Features

## Track Music You’ve Listened To
- Log the songs and albums you've listened to.
- Include details such as the date of listening, your rating, and any reviews or thoughts you have.
- This feature enables you to maintain a comprehensive music history and easily revisit your favorite tunes.

## Save Music You Want to Hear Later
- Add songs or albums to your "Listen Later" list.
- Whether you stumble for new music or receive recommendations, you can save it for future enjoyment.
- Never miss out on discovering great music again!

## Reviews
- Explore reviews from fellow users to uncover new music gems.
- Share your own thoughts and recommendations with the community.
- Discover hidden treasures and expand your music library with ease.

##  Discover users who are interested in specific tracks or have already discovered them.



# APIs for reviews


- **GET /api/ReviewData/getReviews/{trackId}**:
  Retrieves all reviews of a song based on the track ID.
  - **Example**: `GET https://localhost:44387/api/ReviewData/getReviews/{trackId}`
  - **Response**: Returns a list of reviews for the specified track.

- **POST /api/ReviewData/AddReview**:
  Adds a review for a song.
  - **Example**: `POST https://localhost:44387/api/reviewdata/addreview`
  - **Response**: "Review Added" for successful addition of the review.

- **POST /api/ReviewData/RemoveReview/{userId}/{reviewId}**:
  Deletes a review for a song.
  - **Example**: `POST https://localhost:44387/api/ReviewData/RemoveReview/6/1`
  - **Response**: "Review deleted" for successful deletion of the review.

- **POST /api/ReviewData/EditReview/{userId}/{reviewId}**:
  Edits a review for a song.
  - **Example**: `POST https://localhost:44387/api/reviewdata/editreview/10/20`
  - **Response**: "Review Updated" for successful update of the review.

- **GET /api/ReviewData/Find/{reviewId}**:
  Finds a review based on its ID.
  - **Example**: `GET https://localhost:44387/api/ReviewData/Find/{reviewId}`
  - **Response**: Returns the details of the review with the specified ID.

- **GET /api/ReviewData/GetUserReviews/{userId}**:
  Retrieves all reviews of a user based on the user ID.
  - **Example**: `GET https://localhost:44387/api/ReviewData/GetUserReviews/{userId}`
  - **Response**: Returns a list of reviews written by the specified user.



# APIs for track list


- **POST /api/TrackListDatax/AddToListenLater**:
  Adds multiple songs to the user's listen later list.
  - **Example**: `POST https://localhost:44387/api/TrackListDatax/AddToListenLater`
  - **Response**: 200 (OK) for successful addition.

- **GET /api/TrackListData/GetListenLaterList/{userId}**:
  Retrieves a list of songs in the user's listen later list.
  - **Example**: `GET https://localhost:44387/api/TrackListData/GetListenLaterList/{userId}`
  - **Response**: Returns a list of songs in the user's listen later list.

- **POST /api/TrackListData/AddToListenLaterList/{userId}/{trackId}**:
  Adds a song to the user's listen later list.
  - **Example**: `POST https://localhost:44387/api/TrackListData/AddToListenLaterList/{userId}/{trackId}`
  - **Response**: "Track is already in listen later list" if the track is already in the list; otherwise, an error message.

- **POST /api/TrackListData/removeFromListenLater/{userId}/{trackId}**:
  Removes a song from the user's listen later list.
  - **Example**: `POST https://localhost:44387/api/TrackListData/removeFromListenLater/{userId}/{trackId}`
  - **Response**: "User {userId} removed track with id {trackId} from his list" for successful removal.

- **GET /api/TrackListData/GetDiscoverdList/{userId}**:
  Retrieves a list of songs in the user's discovered list.
  - **Example**: `GET https://localhost:44387/api/TrackListData/GetDiscoverdList/{userId}`
  - **Response**: Returns a list of songs in the user's discovered list.

- **POST /api/TrackListData/AddToDiscoverdList/{userId}/{trackId}**:
  Adds a song to the user's discovered list.
  - **Example**: `POST https://localhost:44387/api/TrackListData/AddToDiscoverdList/{userId}/{trackId}`
  - **Response**: "Song added to discovered list" for successful addition.

- **POST /api/TrackListData/removeFromDiscoverd/{userId}/{trackId}**:
  Removes a song from the user's discovered list.
  - **Example**: `POST https://localhost:44387/api/TrackListData/removeFromDiscoverd/{userId}/{trackId}`
  - **Response**: "User {userId} removed track with id {trackId} from his list" for successful removal.




# APIs for users

- **POST /api/UserData/Add**:
  Adds multiple users to the database.
  - **Example**: `POST https://localhost:44387/api/UserData/Add`
  - **Response**: 200 (OK) for successful addition.

- **GET /api/UserData/GetProfileInfo/{id}**:
  Retrieves information about a user based on user ID.
  - **Example**: `GET https://localhost:44387/api/UserData/GetProfileInfo/{id}`
  - **Response**: Returns information about the user.

- **GET /api/UserData/findIntrestedUserForListenLater/{id}**:
  Retrieves a list of users who have a specific song in their listen later list.
  - **Example**: `GET https://localhost:44387/api/UserData/findIntrestedUserForListenLater/{id}`
  - **Response**: Returns a list of users interested in the specified song.

- **GET /api/UserData/findIntrestedUserForDiscoverd/{userId}/{trackId}**:
  Retrieves a list of users who have a specific song in their discovered list, excluding the specified user.
  - **Example**: `GET https://localhost:44387/api/UserData/findIntrestedUserForDiscoverd/{userId}/{trackId}`
  - **Response**: Returns a list of users interested in the specified song, excluding the specified user.

- **POST /api/UserData/Remove/{id}**:
  Removes a user from the database based on user ID.
  - **Example**: `POST https://localhost:44387/api/UserData/Remove/{id}`
  - **Response**: "User deleted from database" for successful removal.

- **GET /api/UserData/CheckUsername/{id}**:
  Checks if a username exists in the database.
  - **Example**: `GET https://localhost:44387/api/UserData/CheckUsername/{id}`
  - **Response**: Returns information about the user if found; otherwise, returns an error message.

- **GET /api/UserData/getUsernames**:
  Retrieves a list of usernames from the database.
  - **Example**: `GET https://localhost:44387/api/UserData/getUsernames`
  - **Response**: Returns a list of usernames.

# APIs for track

- **GET /api/TrackData/ListTracks**:
  Retrieves a list of all tracks in the database.
  - **Example**: `GET https://localhost:44387/api/TrackData/ListTracks`
  - **Response**: Returns a list of tracks.

- **POST /api/TrackData/AddTrack**:
  Adds multiple tracks to the database.
  - **Example**: `POST https://localhost:44387/api/TrackData/AddTrack`
  - **Response**: 200 (OK) for successful addition.

- **POST /api/TrackData/UpdateTrack/{id}**:
  Updates a track in the database.
  - **Example**: `POST https://localhost:44387/api/TrackData/UpdateTrack/{id}`
  - **Response**: 200 (OK) for successful update.

- **POST /api/TrackData/DeleteTrack/{id}**:
  Deletes a track from the database based on its ID.
  - **Example**: `POST https://localhost:44387/api/TrackData/DeleteTrack/{id}`
  - **Response**: 200 (OK) for successful deletion.

- **GET /api/TrackData/DisplaySongs/{id}**:
  Displays a list of songs associated with a specific user.
  - **Example**: `GET https://localhost:44387/api/TrackData/DisplaySongs/{id}`
  - **Response**: Returns a list of songs.






















