using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweb_lavender_paradise.Domain.Enitities.User;
using Tweb_lavender_paradise.Domain.Entities;
using Tweb_lavender_paradise.Domain.Models;

namespace Tweb_lavender_paradise.BusinessLogic.Interfaces
{
    public interface ICart
    {
        CartDBTable GetCartById(int cartId);
        UserDBTable GetUserById(int userId);
        void UpdateCart(CartDBTable cart);
        void AddCart(CartDBTable cart);
        void UpdateUser(UserDBTable user);
        List<ProductDBTable> GetProductsByIds(List<int> ids);
        void AddOrder(OrderHistoryDBTable history);

    }
}
