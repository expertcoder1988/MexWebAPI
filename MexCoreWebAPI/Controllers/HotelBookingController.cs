using MexCoreWebAPI.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MexCoreWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("ReactPolicy")]

    public class HotelBookingController : ControllerBase
    {
        private readonly HotelDBContext objSQL;

        public HotelBookingController(HotelDBContext SQL)
        {
            objSQL = SQL;
        }

        [HttpGet("list")]
        public string BookingList(int propertyId, DateTime CheckIn, DateTime CheckOut, string BookingStatus, int Roomcate)
        {
            try
            {
                var res = (from obj in objSQL.Bookings
                           join b in objSQL.RoomCategories
                          on obj.RoomCatCode equals b.Id into rooms
                           from r in rooms.DefaultIfEmpty()
                           where (obj.HotelCode == propertyId && obj.ReservationDate >= CheckIn && obj.ReservationDate <= CheckOut && obj.BookingStatus != "Blocked")
                           orderby obj.BookingID descending
                           select new
                           {
                               BookingID = obj.BookingID,
                               PaxFName = obj.PaxTitle + " " + obj.PaxFName + " " + obj.PaxLName,
                               PaxPhoneNumber = obj.PaxPhoneNumber,
                               RoomCategory = r.Category,
                               RoomNum = obj.RoomNum,
                               Adults = obj.Adults,
                               NoOfRooms = obj.NoOfRooms,
                               Children = obj.Children,
                               CheckIn = obj.CheckIn,
                               CheckOut = obj.CheckOut,
                               Rate = obj.RoomRate,
                               status = obj.BookingStatus,
                               roomCateCode = obj.RoomCatCode,
                               ReservationDate = obj.DateModified
                           }).ToList();
                            

                if (BookingStatus != "-1")
                {
                    res = res.Where(r => r.status == BookingStatus).ToList();
                }
                if (Roomcate != -1)
                {
                    res = res.Where(r => r.roomCateCode == Roomcate).ToList();
                }
             

                var json = System.Text.Json.JsonSerializer.Serialize(res);
                return json.ToString();
            }
            catch (Exception ex)
            {
                var json = System.Text.Json.JsonSerializer.Serialize(ex.Message.ToString());
                return json.ToString();
            }
        }

        [HttpGet("roomnumbers")]
        public string LoadRoomNums(int propertyId, DateTime CheckIn, DateTime CheckOut, int Roomcate)
        {
            try
            {
                var list1 = (from a in objSQL.RoomNumbers
                             join b in objSQL.Rooms
                             on a.RoomCode equals b.RoomCode
                             where (b.RoomCategory == Roomcate)
                             select new
                             {
                                 RoomNums = a.RoomNumber1
                             }).ToList();

                var list2 = (from a in objSQL.Bookings
                             where (a.HotelCode == propertyId && a.CheckIn >= CheckIn && a.CheckOut <= CheckOut && a.RoomCatCode == Roomcate)
                             select new
                             {
                                 RoomNums = a.RoomNum
                             }).ToList();

                var res = list1.Except(list2).ToList();

                var json = System.Text.Json.JsonSerializer.Serialize(res);
                return json.ToString();
            }
            catch (Exception ex)
            {
                var json = System.Text.Json.JsonSerializer.Serialize(ex.Message.ToString());
                return json.ToString();
            }
        }
    }
}
