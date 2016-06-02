using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "VenueRegistrationLoginService" in code, svc and config file together.
public class VenueRegistrationLoginService : IVenueRegistrationLoginService
{
    ShowTrackerEntities db = new ShowTrackerEntities();

    public bool AddArtist(Artist a)
    {
        //check if the artist is already in the database. If it is, return false
        Artist existingArtist = db.Artists.FirstOrDefault(i => i.ArtistEmail == a.ArtistEmail);

        if(existingArtist != null)
        {
            return false;
        }

        Artist artist = new Artist();
        artist.ArtistDateEntered = DateTime.Now;
        artist.ArtistEmail = a.ArtistEmail;
        artist.ArtistName = a.ArtistName;
        artist.ArtistWebPage = a.ArtistWebPage;
        bool result = true;
        try
        {
            db.Artists.Add(artist);
            db.SaveChanges();
        }catch
        {
           
                result = false;
        }
        return result;

        
    }

    public bool AddShow(Show s)
    {
        //check if the show is already in the database. If it is, return false
        Show existingShow = db.Shows.FirstOrDefault(i => i.ShowName == s.ShowName);

        if (existingShow != null)
        {
            return false;
        }


        Show show = new Show();
        show.ShowDate = s.ShowDate;
        show.ShowDateEntered = DateTime.Now;
        show.ShowName = s.ShowName;
        show.ShowTicketInfo = s.ShowTicketInfo;
        show.ShowTime = s.ShowTime;
        show.VenueKey = s.VenueKey;
        bool result = true;
        try
        {
            db.Shows.Add(show);
            db.SaveChanges();

        }
        catch
        {

            result = false;
        }
        return result;
    }

    

    public bool AddShowDetail(ShowDetail sd)
    {
        bool result = true;

        ShowDetail showDetail = new ShowDetail();

        showDetail.ShowKey = sd.ShowKey;
        showDetail.ArtistKey = sd.ArtistKey;
        showDetail.ShowDetailArtistStartTime = sd.ShowDetailArtistStartTime;
        showDetail.ShowDetailAdditional = sd.ShowDetailAdditional;
        

        return result;
    }

    public bool RegisterVenue(Venue v, string VenueUserName, string VenuePassword)
    {
        bool result = true;
        int pass = db.usp_RegisterVenue(v.VenueName, v.VenueAddress, v.VenueCity, v.VenueState, v.VenueZipCode, v.VenuePhone, v.VenueEmail, v.VenueWebPage, v.VenueAgeRestriction, VenueUserName, VenuePassword);
        if (pass == -1)
        {
            result = false;
        }
        return result;

    }

    public int VenueLogin(string userName, string password)
    {
        int venueKey = 0;
        int result = db.usp_venueLogin(userName, password);
        if (result != -1)
        {
            var key = (from v in db.VenueLogins
                       where v.VenueLoginUserName.Equals(userName)
                       select new { v.VenueLoginKey }).FirstOrDefault();

            venueKey = key.VenueLoginKey;
        }
        return venueKey;
    }
}
