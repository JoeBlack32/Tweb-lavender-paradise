using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweb_lavender_paradise.Domain.Models;

namespace Tweb_lavender_paradise.BusinessLogic.Interfaces
{
    public interface IProductService
    {
        Product GetProductById(int id);
        List<Product> GetAllProducts();
        void AddProductToCart(int userId, int productId);
        List<Product> GetCartProductsByUserId(int userId);
        bool IncreaseProductQuantityInCart(int cartId, int productId);
        bool DecreaseProductQuantityInCart(int cartId, int productId);
        bool DeleteProductFromCart(int cartId, int productId);
        bool ConfirmOrder(UserModel user, out string errorMessage);
    }
}