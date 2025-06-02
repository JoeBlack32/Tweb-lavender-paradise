// BusinessLogic/CartServiceBL.cs
using System.Collections.Generic;
using System.Linq;
using Tweb_lavender_paradise.BusinessLogic.Interfaces;
using Tweb_lavender_paradise.Domain.AutoMapper;
using Tweb_lavender_paradise.Domain.Entities;
using Tweb_lavender_paradise.Domain.Models;
using Tweb_lavender_paradise.Helpers;

namespace Tweb_lavender_paradise.BusinessLogic
{
    public class CartServiceBL
    {
        private readonly ICart _cartApi;

        public CartServiceBL()
        {
            _cartApi = new CartApi();
        }

        public void AddProductToCart(int userId, int productId)
        {
            var user = _cartApi.GetUserById(userId);
            if (user == null) return;

            CartDBTable cart;

            if (user.CartId == null)
            {
                var goodsDict = new Dictionary<int, int> { [productId] = 1 };
                cart = new CartDBTable { Goods = CartHelper.Serialize(goodsDict) };
                _cartApi.AddCart(cart);

                user.CartId = cart.Id;
                _cartApi.UpdateUser(user);
            }
            else
            {
                cart = _cartApi.GetCartById(user.CartId.Value);
                if (cart == null) return;

                var goodsDict = CartHelper.Parse(cart.Goods);
                goodsDict.TryGetValue(productId, out int count);
                goodsDict[productId] = count + 1;

                cart.Goods = CartHelper.Serialize(goodsDict);
                _cartApi.UpdateCart(cart);
            }
        }

        public bool IncreaseQuantity(int cartId, int productId)
        {
            var cart = _cartApi.GetCartById(cartId);
            if (cart == null) return false;

            var goodsDict = CartHelper.Parse(cart.Goods);
            goodsDict.TryGetValue(productId, out int count);
            goodsDict[productId] = count + 1;

            cart.Goods = CartHelper.Serialize(goodsDict);
            _cartApi.UpdateCart(cart);
            return true;
        }

        public bool DecreaseQuantity(int cartId, int productId)
        {
            var cart = _cartApi.GetCartById(cartId);
            if (cart == null) return false;

            var goodsDict = CartHelper.Parse(cart.Goods);
            if (!goodsDict.ContainsKey(productId)) return false;

            if (goodsDict[productId] > 1)
                goodsDict[productId]--;
            else
                goodsDict.Remove(productId);

            cart.Goods = CartHelper.Serialize(goodsDict);
            _cartApi.UpdateCart(cart);
            return true;
        }

        public bool DeleteFromCart(int cartId, int productId)
        {
            var cart = _cartApi.GetCartById(cartId);
            if (cart == null) return false;

            var goodsDict = CartHelper.Parse(cart.Goods);
            if (!goodsDict.ContainsKey(productId)) return false;

            goodsDict.Remove(productId);
            cart.Goods = CartHelper.Serialize(goodsDict);
            _cartApi.UpdateCart(cart);
            return true;
        }

        public bool ConfirmOrder(UserModel user, out string errorMessage)
        {
            errorMessage = string.Empty;
            var dbUser = _cartApi.GetUserById(user.Id);
            if (dbUser == null || dbUser.CartId == null)
            {
                errorMessage = "Пользователь или корзина не найдены";
                return false;
            }

            var cart = _cartApi.GetCartById(dbUser.CartId.Value);
            if (cart == null || string.IsNullOrWhiteSpace(cart.Goods))
            {
                errorMessage = "Корзина пуста";
                return false;
            }

            var goodsDict = CartHelper.Parse(cart.Goods);
            if (goodsDict.Count == 0)
            {
                errorMessage = "Корзина содержит некорректные товары";
                return false;
            }

            var products = _cartApi.GetProductsByIds(goodsDict.Keys.ToList());
            decimal totalSum = 0;
            foreach (var p in products)
            {
                if (goodsDict.TryGetValue(p.GoodCode, out int qty))
                    totalSum += p.GoodPrice * qty;
            }

            if (dbUser.Balance < totalSum)
            {
                errorMessage = "Недостаточно средств на балансе";
                return false;
            }

            dbUser.Balance -= totalSum;
            cart.Goods = null;

            _cartApi.UpdateUser(dbUser);
            _cartApi.UpdateCart(cart);

            var history = new OrderHistory
            {
                UserId = dbUser.Id,
                Goods = CartHelper.Serialize(goodsDict),
                CheckOut = totalSum
            };
            var historyDb = AutoMapperConfig.MapperInstance.Map<OrderHistoryDBTable>(history);
            _cartApi.AddOrder(historyDb);

            return true;
        }

        public List<Product> GetCartProductsByUserId(int userId)
        {
            var user = _cartApi.GetUserById(userId);
            if (user?.CartId == null) return new List<Product>();

            var cart = _cartApi.GetCartById(user.CartId.Value);
            if (cart == null || string.IsNullOrWhiteSpace(cart.Goods)) return new List<Product>();

            var goodsDict = CartHelper.Parse(cart.Goods);
            if (goodsDict.Count == 0) return new List<Product>();

            var dbProducts = _cartApi.GetProductsByIds(goodsDict.Keys.ToList());
            var result = new List<Product>();

            foreach (var dbProduct in dbProducts)
            {
                var product = AutoMapperConfig.MapperInstance.Map<Product>(dbProduct);
                if (goodsDict.TryGetValue(product.GoodCode, out int count))
                {
                    for (int i = 0; i < count; i++)
                        result.Add(product);
                }
            }

            return result;
        }
    }

}
