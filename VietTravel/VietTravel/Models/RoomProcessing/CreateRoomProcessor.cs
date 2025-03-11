using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VietTravel.Models.RoomProcessing
{
    public class CreateRoomProcessor : RoomProcessor
    {
        protected override void Validate(Phong phong)
        {
            if (string.IsNullOrEmpty(phong.MaPhong))
                throw new Exception("Mã phòng không được để trống.");
        }

        protected override void SaveToDatabase(Phong phong)
        {
            db.Phongs.Add(phong);
            db.SaveChanges();
        }

        protected override void AfterProcessing(Phong phong)
        {
            Console.WriteLine("Phòng mới đã được thêm: " + phong.MaPhong);
        }
    }
}