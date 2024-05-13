using GFoods.DataAccess.Data;
using GFoods.Models.Services;
using GFoods.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security.Policy;
using GFoods.Helpers;

namespace GFoods.DataAccess.Repository
{
    public class VnPayReponsitory :  Repository<VnPayLibrary>,IVnPayRepository
    {
        private readonly ApplicationDbContext _db;
        public VnPayReponsitory(ApplicationDbContext db) {
            _db = db;
        }
        public string CreatePaymentUrl(HttpContent content, VnPaymentRequestModel requestModel)
        {
            var tick = DateTime.Now.Ticks.ToString();
            var vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", );
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (order.Amount * 100).ToString()); //Số tiền thanh toán. Số tiền không 
            mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND
            (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần(khử phần thập phân), sau đó gửi sang VNPAY
            là: 10000000
                if (cboBankCode.SelectedItem != null && !string.IsNullOrEmpty(cboBankCode.SelectedItem.Value))
            {
                vnpay.AddRequestData("vnp_BankCode", cboBankCode.SelectedItem.Value);
            }
            vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());

        }

        public VnPaymentResponseModel PaymenExcute(IQueryCollection collections)
        {
            throw new NotImplementedException();
        }
    }
}
