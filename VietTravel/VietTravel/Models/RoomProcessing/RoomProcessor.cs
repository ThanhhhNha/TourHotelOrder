using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VietTravel.Models.RoomProcessing
{
    public abstract class RoomProcessor
    {
        protected TravelVNEntities db = new TravelVNEntities();

        // Template Method: định nghĩa các bước chung
        public void Process(Phong phong)
        {
            Validate(phong);        // Kiểm tra dữ liệu
            SaveToDatabase(phong);  // Lưu vào database
            AfterProcessing(phong); // Hậu xử lý (nếu có)
        }

        protected abstract void Validate(Phong phong);
        protected abstract void SaveToDatabase(Phong phong);
        protected virtual void AfterProcessing(Phong phong) { } // Tuỳ chọn
    }

}