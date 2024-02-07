using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RhythmRecall.Models.ViewModels
{
    public class DetailsUser
    {
        // In profile we need data from 3 models.
        // 1. User
        // 2. TrackList
        // 3. Review
        // Using view models for this


        public UserDto SelectedUser { get; set; }

        public IEnumerable<TrackListDto> UserListenLaterList { get; set; }

        public IEnumerable<TrackListDto> UserDiscoverdList { get; set; }


        public IEnumerable<ReviewDto> UserReviews { get; set; }

    }
}