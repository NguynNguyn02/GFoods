using GFoods.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFoods.DataAccess.Repository.IRepository
{
    public interface IVnPayRepository
    {
        string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel requestModel);
        VnPaymentResponseModel PaymentExcute(IQueryCollection collections);
    }
}
