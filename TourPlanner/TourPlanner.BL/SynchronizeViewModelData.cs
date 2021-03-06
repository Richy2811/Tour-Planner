using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Models;

namespace TourPlanner.BL
{
    public class SynchronizeViewModelData
    {
        public static void SynchronizeTitleDescriptionTourList(TourData tourInfo, TourData tourSelection)
        {
            //if a tour in the list is being selected and a property of tourInfo changed through user input
            if ((tourSelection != null) && (tourSelection.IsUpdating == false))
            {
                //set all properties of current selection in tour list to reflect the changes in the input fields
                tourSelection.ID = tourInfo.ID;
                tourSelection.TourName = tourInfo.TourName;
                tourSelection.TourDescription = tourInfo.TourDescription;
                tourSelection.Start = tourInfo.Start;
                tourSelection.Destination = tourInfo.Destination;
                tourSelection.TransportType = tourInfo.TransportType;
                tourSelection.TourDistance = tourInfo.TourDistance;
                tourSelection.Time = tourInfo.Time;
                tourSelection.ImageName = tourInfo.ImageName;
                tourSelection.Popularity = tourInfo.Popularity;
                tourSelection.ChildFriendliness = tourInfo.ChildFriendliness;
                tourSelection.Favourite = tourInfo.Favourite;
            }
        }

        public static void SynchronizeTourListTitleDescription(TourData tourSelection, TourData tourInfo)
        {
            if (tourSelection != null)
            {
                //prevent SynchronizeTitleDescriptionTourList from changing properties of current selection as long as the selection is updating input fields
                tourSelection.IsUpdating = true;

                tourInfo.ID = tourSelection.ID;
                tourInfo.TourName = tourSelection.TourName;
                tourInfo.TourDescription = tourSelection.TourDescription;
                tourInfo.Start = tourSelection.Start;
                tourInfo.Destination = tourSelection.Destination;
                tourInfo.TransportType = tourSelection.TransportType;
                tourInfo.TourDistance = tourSelection.TourDistance;
                tourInfo.Time = tourSelection.Time;
                tourInfo.ImageName = tourSelection.ImageName;
                tourInfo.Popularity = tourSelection.Popularity;
                tourInfo.ChildFriendliness = tourSelection.ChildFriendliness;
                tourInfo.Favourite = tourSelection.Favourite;

                tourSelection.IsUpdating = false;
            }
        }

    }
}
