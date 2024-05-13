using GFoods.DataAccess.Repository.IRepository;
using GFoods.Helpers;
using GFoods.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFoods.Models.Services
{
    public interface IVnPayRepository:IRepository<VnPayLibrary>
    {
        string CreatePaymentUrl(HttpContent content,VnPaymentRequestModel requestModel);
        VnPaymentResponseModel PaymenExcute(IQueryCollection collections);
    }
}
