# Main Features

## Track Music You’ve Listened To
- Log the songs and albums you've listened to.
- Include details such as the date of listening, your rating, and any reviews or thoughts you have.
- This feature enables you to maintain a comprehensive music history and easily revisit your favorite tunes.

## Save Music You Want to Hear Later
- Add songs or albums to your "Listen Later" list.
- Whether you stumble upon new music or receive recommendations, you can save it for future enjoyment.
- Never miss out on discovering great music again!

## Reviews
- Explore reviews from fellow users to uncover new music gems.
- Share your own thoughts and recommendations with the community.
- Discover hidden treasures and expand your music library with ease.

# Additional Features

## User Connections
- Connect with other users who share similar music interests.
- Discover users who are interested in specific tracks or have already discovered them.
- Expand your musical network and engage with like-minded individuals.



# APIs for reviews


- **GET /api/ReviewData/getReviews/{trackId}**:
  Retrieves all reviews of a song based on the track ID.
  - **Example**: `GET https://localhost:44387/api/ReviewData/getReviews/{trackId}`
  - **Response**: Returns a list of reviews for the specified track.

- **POST /api/ReviewData/AddReview**:
  Adds a review for a song.
  - **Example**: `POST https://localhost:44387/api/reviewdata/addreview`
  - **Response**: "Review Added" upon successful addition of the review.

- **POST /api/ReviewData/RemoveReview/{userId}/{reviewId}**:
  Deletes a review for a song.
  - **Example**: `POST https://localhost:44387/api/ReviewData/RemoveReview/6/1`
  - **Response**: "Review deleted" upon successful deletion of the review.

- **POST /api/ReviewData/EditReview/{userId}/{reviewId}**:
  Edits a review for a song.
  - **Example**: `POST https://localhost:44387/api/reviewdata/editreview/10/20`
  - **Response**: "Review Updated" upon successful update of the review.

- **GET /api/ReviewData/Find/{reviewId}**:
  Finds a review based on its ID.
  - **Example**: `GET https://localhost:44387/api/ReviewData/Find/{reviewId}`
  - **Response**: Returns the details of the review with the specified ID.

- **GET /api/ReviewData/GetUserReviews/{userId}**:
  Retrieves all reviews of a user based on the user ID.
  - **Example**: `GET https://localhost:44387/api/ReviewData/GetUserReviews/{userId}`
  - **Response**: Returns a list of reviews written by the specified user.































### Ignore, for personal user only

### TODO

## API
      - api method to get all songs of user - done ✅
      - api method to get discoverd songs of user - done ✅
      - api method to get listen later songs of user - done ✅
      
      - api method to add listen later song of user - done ✅
      - api method to add discoverd songs of user - done ✅
      
      - api method to delete song from listen later list of user - done ✅
      - api method to delete  songs from discoverd list of user - done ✅
      
      - make model of Review entity - done ✅
      
      - api method to view a review of a song -  done ✅
      - api method to write a review of a song - done ✅
      - api method to update a review of a song - done ✅
      - api method to delete a review of a song - done ✅
      - api method to view all reviews of user - done ✅

  ## Controller and Views
  
     ## TrackList Controller - done ✅
    
      - Display ListenLater - done ✅
      - Display Discoverd - done ✅
      - Remove from listen later - done ✅
      - Reomve from discoverd - done ✅
      - Add to listen later - done ✅
      - Add to discoverd - done ✅

    ## Review COntroller with views -done ✅

    ## Home - populate some randome users review- DO 💭


    ## Profile - make users profile page to navigate to listen later, discoverd list and reviews- DO 💭


    ## Other users discoverd and listen later list- DO 💭


    ## CSS to all views - DO 💭

              

- login/signup - DO 🗨
