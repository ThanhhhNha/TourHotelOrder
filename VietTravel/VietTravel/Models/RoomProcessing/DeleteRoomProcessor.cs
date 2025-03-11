using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VietTravel.Models.RoomProcessing
{
    public class DeleteRoomProcessor : RoomProcessor
    {
        protected override void Validate(Phong phong)
        {
            if (phong == null)
                throw new Exception("Phòng không tồn tại.");
        }

        protected override void SaveToDatabase(Phong phong)
        {
            db.Phongs.Remove(phong);
            db.SaveChanges();
        }
    }
}