using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VietTravel.Models;

namespace VietTravel.Controllers
{
    public class BookingController : Controller
    {
        private TravelVNEntities db = new TravelVNEntities();

        [HttpPost]
        public ActionResult Payment(string roomType, decimal price, string checkinDate, string checkoutDate, int roomQuantity, string hotelId)
        {
            ViewBag.RoomType = roomType;
            ViewBag.Price = price;
            ViewBag.CheckinDate = checkinDate;
            ViewBag.CheckoutDate = checkoutDate;
            ViewBag.RoomQuantity = roomQuantity;
            ViewBag.HotelId = hotelId;

            // Lưu thông tin này vào TempData hoặc Session để truyền vào PaymentWithPaypal
            TempData["RoomType"] = roomType;
            TempData["Price"] = price;
            TempData["RoomQuantity"] = roomQuantity;

            return View();
        }


        public ActionResult PaymentTour(string id)
        {
            var tour = db.Tours.FirstOrDefault(t => t.MaTour == id);
            if (tour == null)
            {
                return HttpNotFound();
            }
            TempData["TourId"] = tour.MaTour;
            TempData["TourPrice"] = tour.Gia.ToString();
            TempData["TourName"] = tour.TenTour;
            return View(tour);
        }


        [HttpPost]

        public ActionResult PaymentConfirmation(
        string name, string email, string phone, string title,
        string fullname, DateTime dob, string requests,
        string roomType, decimal? price, string checkinDate,
        string checkoutDate, int? roomQuantity, string hotelId,
        string paymentMethod)
        {
            OrderBooking orderBooking = new OrderBooking
            {
                Name = name,
                Email = email,
                Phone = phone,
                Title = title,
                FullName = fullname,
                Dob = dob,
                Requests = requests,
                RoomType = roomType,
                Price = price ?? 0,
                CheckinDate = DateTime.Parse(checkinDate),
                CheckoutDate = DateTime.Parse(checkoutDate),
                RoomQuantity = roomQuantity ?? 1,
                BookingDate = DateTime.Now,
                MaKhachSan = hotelId,
            };

            try
            {
                db.OrderBookings.Add(orderBooking);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Đặt phòng thành công!";

                if (paymentMethod == "PayPal")
                {
                    return RedirectToAction("PaymentWithPaypal");
                }
                else if (paymentMethod == "MoMo")
                {
                }

                return RedirectToAction("PaymentSuccess");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi khi lưu thông tin đặt phòng. Vui lòng kiểm tra lại.");
                return View();
            }
        }

        [HttpPost]
        public ActionResult ConfirmBooking(TourBooking booking)
        {
            booking.BookingDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                using (var context = new TravelVNEntities())
                {
                    context.TourBookings.Add(booking);
                    context.SaveChanges();
                }
                return RedirectToAction("PaymentWithPaypal");
            }
            return View(booking);
        }

        public ActionResult PaymentSuccess()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View();
        }

        [HttpGet]
        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            // Get API context for PayPal
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Booking/PaymentWithPaypal?";
                    var guid = Convert.ToString((new Random()).Next(100000));
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);

                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectUrl = lnk.href;
                        }
                    }

                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                    TempData["SuccessMessage"] = "Payment completed successfully!";
                }
            }
            catch (Exception ex)
            {
                return View("FailureView");
            }
            return RedirectToAction("PaymentSuccess");
        }

        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };

            // Lấy thông tin phòng từ TempData
            string roomType = TempData["RoomType"]?.ToString();
            decimal roomPriceVND = Convert.ToDecimal(TempData["Price"]);
            string roomQuantity = TempData["RoomQuantity"]?.ToString();

            // Lấy thông tin tour từ TempData
            string tourId = TempData["TourId"]?.ToString();
            decimal tourPriceVND = !string.IsNullOrEmpty(TempData["TourPrice"]?.ToString())
                ? Convert.ToDecimal(TempData["TourPrice"])
                : 0;
            string tourName = TempData["TourName"]?.ToString();

            // Exchange rate (static example, ideally retrieved from a live API)
            const decimal exchangeRate = 0.000041m; // Example rate (1 VND = 0.000041 USD)

            // Convert room price to USD
            decimal roomPriceUSD = Math.Round(roomPriceVND * exchangeRate, 2);
            decimal tourPriceUSD = Math.Round(tourPriceVND * exchangeRate, 2);

            // Add room information to item list if applicable
            if (!string.IsNullOrEmpty(roomType) && roomPriceUSD > 0)
            {
                itemList.items.Add(new Item()
                {
                    name = roomType,
                    currency = "USD",
                    price = roomPriceUSD.ToString("F2"),
                    quantity = roomQuantity ?? "1",
                    sku = "room"
                });
            }

            // Add tour information to item list if applicable
            if (!string.IsNullOrEmpty(tourId) && tourPriceUSD > 0)
            {
                itemList.items.Add(new Item()
                {
                    name = tourName,
                    currency = "USD",
                    price = tourPriceUSD.ToString("F2"),
                    quantity = "1",
                    sku = tourId
                });
            }

            var payer = new Payer()
            {
                payment_method = "paypal"
            };

            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };

            // Calculate the total amount for all items
            decimal totalAmountUSD = roomPriceUSD * (string.IsNullOrEmpty(roomQuantity) ? 1 : int.Parse(roomQuantity)) + tourPriceUSD;

            var details = new Details()
            {
                subtotal = totalAmountUSD.ToString("F2")
            };

            var amount = new Amount()
            {
                currency = "USD",
                total = details.subtotal,
                details = details
            };

            var transactionList = new List<Transaction>();
            var paypalOrderId = DateTime.Now.Ticks;
            transactionList.Add(new Transaction()
            {
                description = $"Invoice #{paypalOrderId}",
                invoice_number = paypalOrderId.ToString(),
                amount = amount,
                item_list = itemList
            });

            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            return this.payment.Create(apiContext);
        }
    }
}