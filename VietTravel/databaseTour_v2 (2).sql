 Optional: Uncomment this if running on an existing system
USE master;
GO

 Drop database if it exists
DROP DATABASE IF EXISTS TravelVN;
GO

 Create database TravelVN
CREATE DATABASE TravelVN;
GO

USE TravelVN;
GO

CREATE TABLE LoaiPhong (
    MaLoai NVARCHAR(10) PRIMARY KEY,
    TenLoai NVARCHAR(MAX) NULL
);

CREATE TABLE TrangThaiPhong (
    MaTrangThai NVARCHAR(10) PRIMARY KEY,
    TenTrangThai NVARCHAR(50) NOT NULL
);


CREATE TABLE TinhThanh (
    MaTinh NVARCHAR(10) PRIMARY KEY,
    TenTinh NVARCHAR(255) NOT NULL
);
CREATE TABLE Hotels (
    MaKhachSan NVARCHAR(10) PRIMARY KEY,
    TenKhachSan NVARCHAR(MAX) NOT NULL,
    DiaChi NVARCHAR(MAX),
    MoTa NVARCHAR(MAX),
    HinhAnh NVARCHAR(255),
	MaTinh NVARCHAR(10) NULL,
	FOREIGN KEY (MaTinh) REFERENCES TinhThanh(MaTinh)
);


CREATE TABLE Users (
    MaUser NVARCHAR(10) PRIMARY KEY,
    TenUser NVARCHAR(50) NOT NULL,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL,
    Email NVARCHAR(100),
    DienThoai NVARCHAR(15)
);


CREATE TABLE Phong (
    MaPhong NVARCHAR(10) PRIMARY KEY,
    MaLoai NVARCHAR(10) NOT NULL,
    MaTrangThai NVARCHAR(10) NOT NULL,
    MaKhachSan NVARCHAR(10) NOT NULL,
    MaUser NVARCHAR(10) NULL,
    Gia DECIMAL(18, 2) NOT NULL,
    Hinh NVARCHAR(255),
    FOREIGN KEY (MaLoai) REFERENCES LoaiPhong(MaLoai),
    FOREIGN KEY (MaTrangThai) REFERENCES TrangThaiPhong(MaTrangThai),
    FOREIGN KEY (MaKhachSan) REFERENCES Hotels(MaKhachSan),
    FOREIGN KEY (MaUser) REFERENCES Users(MaUser)
);


CREATE TABLE LoaiTour (
    MaLoaiTour NVARCHAR(10) PRIMARY KEY,
    TenLoaiTour NVARCHAR(MAX) NOT NULL
);

CREATE TABLE Tour (
    MaTour NVARCHAR(10) PRIMARY KEY,
    TenTour NVARCHAR(MAX) NOT NULL,
    MoTa NVARCHAR(MAX),
    NgayKhoiHanh DATE NOT NULL,
    ThoiGian INT NOT NULL,  -- Duration of the tour (days)
    Gia DECIMAL(18, 2) NOT NULL,
    MaLoaiTour NVARCHAR(10) NOT NULL,
    HinhAnh NVARCHAR(255),
    PhuongTien NVARCHAR(255),
	MaTinh NVARCHAR(10) NULL,
	FOREIGN KEY (MaTinh) REFERENCES TinhThanh(MaTinh),
    FOREIGN KEY (MaLoaiTour) REFERENCES LoaiTour(MaLoaiTour)
);


CREATE TABLE Flight (
    MaFlight NVARCHAR(10) PRIMARY KEY,
    TenFlight NVARCHAR(100) NOT NULL,
    HangBay NVARCHAR(50),
    LoaiMayBay NVARCHAR(50)
);


CREATE TABLE OrderBooking (
    Id INT PRIMARY KEY IDENTITY(1,1),  
    Name NVARCHAR(255) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    Phone NVARCHAR(50),
    Title NVARCHAR(50),
    FullName NVARCHAR(255),
    Dob DATE,
    Requests NVARCHAR(MAX),
    RoomType NVARCHAR(100),
    Price DECIMAL(18, 2),
    CheckinDate DATE NOT NULL,
    CheckoutDate DATE NOT NULL,
    RoomQuantity INT NOT NULL,
    BookingDate DATETIME NOT NULL DEFAULT GETDATE(),
    MaKhachSan NVARCHAR(10) NOT NULL,  
    FOREIGN KEY (MaKhachSan) REFERENCES Hotels(MaKhachSan)
);

CREATE TABLE Combo (
    MaCombo INT PRIMARY KEY IDENTITY(1,1),
    TenCombo NVARCHAR(255) NOT NULL,
    GiaCombo DECIMAL(18, 2) NOT NULL
);



CREATE TABLE ComboTour (
    MaCombo INT NOT NULL,
    MaTour NVARCHAR(10) NOT NULL,
    PRIMARY KEY (MaCombo, MaTour),
    FOREIGN KEY (MaCombo) REFERENCES Combo(MaCombo),
    FOREIGN KEY (MaTour) REFERENCES Tour(MaTour)
);


CREATE TABLE ComboKhachSan (
    MaCombo INT NOT NULL,
    MaKhachSan NVARCHAR(10) NOT NULL,
    PRIMARY KEY (MaCombo, MaKhachSan),
    FOREIGN KEY (MaCombo) REFERENCES Combo(MaCombo),
    FOREIGN KEY (MaKhachSan) REFERENCES Hotels(MaKhachSan)
);

CREATE TABLE ComboChuyenBay (
    MaCombo INT NOT NULL,
    MaFlight NVARCHAR(10) NOT NULL,
    PRIMARY KEY (MaCombo, MaFlight),
    FOREIGN KEY (MaCombo) REFERENCES Combo(MaCombo),
    FOREIGN KEY (MaFlight) REFERENCES Flight(MaFlight)
);


CREATE TABLE RevenueSummary (
    Id INT PRIMARY KEY IDENTITY(1,1),
    TotalRoomsBooked INT NOT NULL,
    TotalRevenue DECIMAL(18, 2) NOT NULL,
    Date DATETIME NOT NULL
);

CREATE TABLE TourBooking (
    BookingId INT IDENTITY(1,1) PRIMARY KEY, 
    Name NVARCHAR(100),                      
    Email NVARCHAR(100),                     
    Phone NVARCHAR(15),                     
    Title NVARCHAR(50),                     
    FullName NVARCHAR(100),                  
    Dob DATE,                               
    Requests NVARCHAR(500),                  
    PaymentMethod NVARCHAR(50),           
    TourId  NVARCHAR(10),  
	BookingDate DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (TourId) REFERENCES Tour(MaTour)
);



INSERT INTO LoaiPhong (MaLoai, TenLoai)
VALUES (N'C1', N'Phòng đơn'), (N'C2', N'Phòng đôi'), (N'C3', N'Phòng gia đình');


INSERT INTO TrangThaiPhong (MaTrangThai, TenTrangThai)
VALUES (N'S1', N'Phòng trống'), (N'S2', N'Phòng đã đặt'), (N'S3', N'Phòng đang thuê');

INSERT INTO Users (MaUser, TenUser, Username, Password, Email, DienThoai)
VALUES (N'U002', N'Người dùng A', N'userA', N'password_hash_2', N'userA@travel.com', '0987654321');

INSERT INTO LoaiTour (MaLoaiTour, TenLoaiTour)
VALUES (N'T1', N'Tour trong nước'), (N'T2', N'Tour quốc tế');

INSERT INTO Flight (MaFlight, TenFlight, HangBay, LoaiMayBay)
VALUES (N'FL01', N'Chuyến bay VN123', N'Vietnam Airlines', N'Boeing 787');


INSERT INTO TinhThanh (MaTinh, TenTinh)
VALUES
(N'T1', N'Hà Nội'),
(N'T2', N'TP Hồ Chí Minh'),
(N'T3', N'Thái Nguyên'),
(N'T4', N'Đà Nẵng'),
(N'T5', N'Hải Phòng'),
(N'T6', N'Đồng Nai'),
(N'T7', N'Bình Dương'),
(N'T8', N'Bắc Ninh'),
(N'T9', N'Quảng Ninh'),
(N'T10', N'Thừa Thiên - Huế'),
(N'T11', N'Hà Giang'),
(N'T12', N'Lào Cai'),
(N'T13', N'Sơn La'),
(N'T14', N'Mộc Châu'),
(N'T15', N'Điện Biên'),
(N'T16', N'Yên Bái'),
(N'T17', N'Tuyên Quang'),
(N'T18', N'Lạng Sơn'),
(N'T19', N'Nghệ An'),
(N'T20', N'Hà Tĩnh'),
(N'T21', N'Nam Định'),
(N'T22', N'Ninh Bình'),
(N'T23', N'Hưng Yên'),
(N'T24', N'Hà Nam'),
(N'T25', N'Thái Bình'),
(N'T26', N'Hải Dương'),
(N'T27', N'Gia Lai'),
(N'T28', N'Kon Tum'),
(N'T29', N'Phú Yên'),
(N'T30', N'Khánh Hòa'),
(N'T31', N'Tây Ninh'),
(N'T32', N'Bình Phước'),
(N'T33', N'Lâm Đồng'),
(N'T34', N'Đắk Lắk'),
(N'T35', N'Đắk Nông'),
(N'T37', N'Bến Tre'),
(N'T38', N'Tiền Giang'),
(N'T39', N'Sóc Trăng'),
(N'T40', N'Hậu Giang'),
(N'T41', N'Vĩnh Long'),
(N'T42', N'Đồng Tháp'),
(N'T43', N'An Giang'),
(N'T44', N'Kiên Giang'),
(N'T45', N'Cà Mau'),
(N'T46', N'Bạc Liêu'),
(N'T47', N'Cần Thơ'),
(N'T48', N'Trà Vinh'),
(N'T49', N'Ninh Thuận'),
(N'T50', N'Bình Thuận'),
(N'T51', N'Quảng Bình'),
(N'T52', N'Quảng Trị'),
(N'T54', N'Phú Thọ'),
(N'T55', N'Hòa Bình'),
(N'T57', N'Bắc Giang');

INSERT INTO Hotels (MaKhachSan, TenKhachSan, DiaChi, MoTa, HinhAnh, MaTinh)
VALUES 
(N'HS02', N'Khách sạn Mặt Trời', N'456 Đường Nguyên Văn Cừ, Hà Nội', N'Khách sạn hiện đại, gần trung tâm thành phố', 'hotelB.jpg', N'T1'),
(N'HS03', N'Khách sạn Ánh Dương', N'789 Đường Lê Lợi, TP Hồ Chí Minh', N'Khách sạn sang trọng với tầm nhìn ra sông', 'hotelC.jpg', N'T2'),
(N'HS04', N'Khách sạn Bình Minh', N'101 Đường Võ Nguyên Giáp, Đà Nẵng', N'Khách sạn bên bờ biển với dịch vụ tốt', 'hotelD.jpg', N'T4'),
(N'HS05', N'Khách sạn Thiên Đường', N'202 Đường Trần Phú, Khánh Hoà', N'Khách sạn gần biển với không gian thoáng đãng', 'hotelE.jpg', N'T30'),
(N'HS06', N'Khách sạn Ngọc Lan', N'303 Đường Lạch Tray, Hải Phòng', N'Khách sạn 5 sao với nhiều tiện ích', 'hotelF.jpg', N'T5'),
(N'HS07', N'Khách sạn Hoàng Hôn', N'404 Đường Hùng Vương, Thừa Thiên - Huế', N'Khách sạn cổ điển, gần di tích lịch sử', 'hotelG.jpg', N'T10'),
(N'HS08', N'Khách sạn Hoa Vàng', N'505 Đường Trần Hưng Đạo, Kon Tum', N'Khách sạn yên tĩnh, lý tưởng cho nghỉ dưỡng', 'hotelH.jpg', N'T28'),
(N'HS09', N'Khách sạn Vịnh Biển', N'606 Đường Hạ Long, Quảng Ninh', N'Khách sạn có dịch vụ spa và hồ bơi', 'hotelI.jpg', N'T9'),
(N'HS10', N'Khách sạn Cổ Điển', N'707 Đường Nguyễn Ái Quốc, Đồng Nai', N'Khách sạn thân thiện, gần khu công nghiệp', 'hotelJ.jpg', N'T6'),
(N'HS11', N'Khách sạn Bến Cảng', N'808 Đường Nguyễn Thái Học, Kiên Giang', N'Khách sạn nghỉ dưỡng bên bờ biển', 'hotelK.jpg', N'T44');


INSERT INTO Phong (MaPhong, MaLoai, MaTrangThai, MaKhachSan, Gia, Hinh)
VALUES (N'P001', N'C1', N'S1',N'HS02',  1000000, 'room1.jpg'),
    (N'P002', N'C2', N'S1',  N'HS02', 1500000, 'room2.jpg'),
    (N'P003', N'C3', N'S1', N'HS03',  2000000, 'room3.jpg'),
    (N'P004',  N'C1', N'S1', N'HS04', 1200000, 'room4.jpg'),
    (N'P005',  N'C2', N'S1', N'HS05', 1700000, 'room5.jpg'),
    (N'P006', N'C3', N'S1', N'HS06',  2500000, 'room6.jpg'),
    (N'P007', N'C1', N'S1', N'HS07',  900000, 'room7.jpg'),
    (N'P008',N'C2', N'S1',  N'HS08',  1600000, 'room8.jpg'),
    (N'P009', N'C3', N'S1', N'HS09',  2100000, 'room9.jpg'),
    (N'P010', N'C1', N'S1', N'HS10',  1100000, 'room10.jpg'),
    (N'P011',  N'C2', N'S1', N'HS11', 1300000, 'room11.jpg');

INSERT INTO Tour (MaTour, TenTour, MoTa, NgayKhoiHanh, ThoiGian, Gia, MaLoaiTour, HinhAnh, PhuongTien, MaTinh)
VALUES 
(N'T002', N'Tour Sapa 2 ngày', N'Tour khám phá Sapa, phong cảnh thiên nhiên tuyệt đẹp', '2024-11-10', 2, 3000000, N'T1', N'tour01.jpg', N'Xe', N'T12'),
(N'T003', N'Tour Phú Quốc 3 ngày', N'Tour tham quan đảo Phú Quốc, tắm biển và thưởng thức hải sản', '2024-12-05', 3, 5000000, N'T1', N'tour02.jpg', N'Máy bay', N'T44'),
(N'T004', N'Tour Nha Trang 4 ngày', N'Tour nghỉ dưỡng tại Nha Trang, tham gia các hoạt động thể thao dưới nước', '2024-11-15', 4, 7000000, N'T1', N'tour03.jpg', N'Máy bay', N'T30'),
(N'T005', N'Tour Hội An 2 ngày', N'Tour khám phá phố cổ Hội An và các làng nghề truyền thống', '2024-11-20', 2, 2500000, N'T1', N'tour04.jpg', N'Xe', N'T4'),
(N'T006', N'Tour Đà Lạt 3 ngày', N'Tour khám phá Đà Lạt, thành phố ngàn hoa', '2024-12-12', 3, 3500000, N'T1', N'tour05.jpg', N'Xe', N'T33'),
(N'T007', N'Tour Quy Nhơn 2 ngày', N'Tour tham quan các điểm du lịch nổi tiếng tại Quy Nhơn', '2024-11-25', 2, 2800000, N'T1', N'tour06.jpg', N'Xe', N'T27'),
(N'T008', N'Tour Hạ Long 1 ngày', N'Tour tham quan vịnh Hạ Long, thưởng thức hải sản', '2024-11-30', 1, 1500000, N'T1', N'tour07.jpg', N'Xe', N'T9'),
(N'T009', N'Tour Mộc Châu 2 ngày', N'Tour khám phá vùng cao nguyên Mộc Châu', '2024-12-01', 2, 2200000, N'T1', N'tour08.jpg', N'Xe', N'T14'),
(N'T010', N'Tour Bình Ba 3 ngày', N'Tour tham quan đảo Bình Ba, tắm biển và thưởng thức hải sản', '2024-12-10', 3, 4500000, N'T1', N'tour09.jpg', N'Máy bay', N'T29'),
(N'T011', N'Tour Đồng Hới 2 ngày', N'Tour khám phá các điểm du lịch tại Đồng Hới', '2024-12-15', 2, 2000000, N'T1', N'tour10.jpg', N'Xe', N'T51'),
(N'T012', N'Tour Kiên Giang 2 ngày', N'Tour khám phá các điểm du lịch nổi tiếng tại Kiên Giang', '2024-12-01', 2, 2500000, N'T1', N'tour11.jpg', N'Xe', N'T44'),
(N'T013', N'Tour Hà Tiên 3 ngày', N'Tour tham quan Hà Tiên, tìm hiểu văn hóa và lịch sử', '2024-12-10', 3, 3000000, N'T1', N'tour12.jpg', N'Máy bay', N'T44'),
(N'T014', N'Tour Phú Quốc 4 ngày', N'Tour nghỉ dưỡng tại Phú Quốc, thưởng thức hải sản và khám phá thiên nhiên', '2024-12-15', 4, 5000000, N'T1', N'tour13.jpg', N'Máy bay', N'T44'),
(N'T015', N'Tour Rạch Giá 2 ngày', N'Tour khám phá Rạch Giá, tham quan các địa điểm nổi tiếng', '2024-12-20', 2, 2200000, N'T1', N'tour14.jpg', N'Xe', N'T44'),
(N'T016', N'Tour Kiên Giang 3 ngày', N'Tour tham quan các đảo và bãi biển đẹp tại Kiên Giang', '2024-12-25', 3, 3500000, N'T1', N'tour15.jpg', N'Xe', N'T44')

CREATE TABLE Admin (
    MaAdmin NVARCHAR(10) PRIMARY KEY,
    TenAdmin NVARCHAR(MAX) NULL,
    VaiTro NVARCHAR(MAX) NULL,
    Username VARCHAR(50),
    Passwords VARCHAR(50)
);
INSERT INTO Admin (MaAdmin, TenAdmin, VaiTro, Username, Passwords)
VALUES (N'AD001', N'Capypara', N'Admin', 'capy', 'capy123');


