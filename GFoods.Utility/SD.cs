using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFoods.Utility
{
    public class SD
    {
        public const string Role_Customer = "Customer";
        public const string Role_Company = "Company";
        public const string Role_Admin = "Admin";
        public const string Role_Employee = "Employee";


        public const string StatusPending = "Pending";//Đang chờ
		public const string StatusApproved = "Approved";//Đã duyệt
		public const string StatusInProcess = "InProcess";  //Đang xử lý
		public const string StatusShipped = "Shipped";//Đã gửi hàng
		public const string StatusCancelled = "Cancelled";//Đã hủy  
		public const string StatusRefunded = "Refunded";//Đã hoàn tiền

		public const string PaymentStatusPending = "Pending";//Chờ thanh toán
		public const string PaymentStatusApproved = "Approved";//Đã thanh toán
		public const string PaymentStatusDelayedPayment = "ApprovedForDelayedPayment";//Đã duyệt cho thanh toán sau
		public const string PaymentStatusRejected = "Rejected";//Bị từ chối thanh toán


		public const string SessionCart = "SessionShoppingCart";
	}
}
