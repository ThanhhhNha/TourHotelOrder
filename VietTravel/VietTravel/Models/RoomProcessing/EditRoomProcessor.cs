using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace VietTravel.Models.RoomProcessing
{
    public class EditRoomProcessor : RoomProcessor
    {
        protected override void Validate(Phong phong)
        {
            if (string.IsNullOrEmpty(phong.MaPhong))
                throw new Exception("Mã phòng không hợp lệ.");
        }

        protected override void SaveToDatabase(Phong phong)
        {
            db.Entry(phong).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}