using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Tweb_lavender_paradise.BusinessLogic.Core.Product;
using Tweb_lavender_paradise.BusinessLogic.Interfaces;
using Tweb_lavender_paradise.Domain.AutoMapper;
using Tweb_lavender_paradise.Domain.Models;

namespace Tweb_lavender_paradise.BusinessLogic.BLogic
{
    public class ProductServiceBL : IProductService
    {

        private readonly ProductApi _productApi;
        public ProductServiceBL()
        {
            _productApi = new ProductApi();
        }

        public List<Product> GetAllProducts()
        {
            var productsFromDB = _productApi.GetAllProductsApi();

            var middleProducts = new List<Product>();
            foreach (var p in productsFromDB)
            {
                middleProducts.Add(new Product
                {
                    GoodCode = p.GoodCode,
                    GoodName = p.GoodName,
                    GoodDescription = p.GoodDescription,
                    GoodPrice = p.GoodPrice,
                    ImgSrc = p.ImgSrc,
                    Category = p.Category
                });
            }

            return middleProducts;
        }

        public List<Category> GetAllCategories()
        {
            var dbCategories = ProductApi.GetAllCategories();
            return dbCategories.Select(c => AutoMapperConfig.MapperInstance.Map<Category>(c)).ToList();
        }

        //public List<Product> GetCartProductsByUserId(int userId)
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        connection.Open();

        //        // Получаем CartId пользователя
        //        var getUserCommand = new SqlCommand("SELECT CartId FROM Users WHERE Id = @UserId", connection);
        //        getUserCommand.Parameters.AddWithValue("@UserId", userId);
        //        var cartIdObj = getUserCommand.ExecuteScalar();

        //        if (cartIdObj == null || cartIdObj == DBNull.Value)
        //            return new List<Product>();

        //        int cartId = Convert.ToInt32(cartIdObj);

        //        // Получаем содержимое корзины
        //        var getCartCommand = new SqlCommand("SELECT Goods FROM Cart WHERE Id = @CartId", connection);
        //        getCartCommand.Parameters.AddWithValue("@CartId", cartId);
        //        var goodsStr = getCartCommand.ExecuteScalar()?.ToString();

        //        if (string.IsNullOrWhiteSpace(goodsStr))
        //            return new List<Product>();

        //        // Разбор строки формата "{1:2},{4:1}"
        //        var goodsDict = new Dictionary<int, int>();
        //        var entries = goodsStr.Split(',', (char)StringSplitOptions.RemoveEmptyEntries);
        //        foreach (var entry in entries)
        //        {
        //            var trimmed = entry.Trim('{', '}');
        //            var parts = trimmed.Split(':');
        //            if (parts.Length == 2 &&
        //                int.TryParse(parts[0], out int productId) &&
        //                int.TryParse(parts[1], out int quantity))
        //            {
        //                goodsDict[productId] = quantity;
        //            }
        //        }

        //        if (goodsDict.Count == 0)
        //            return new List<Product>();

        //        // Получаем все продукты по их ID
        //        var idList = string.Join(",", goodsDict.Keys);
        //        var getProductsCommand = new SqlCommand($"SELECT * FROM Product WHERE GoodCode IN ({idList})", connection);
        //        var products = new List<Product>();

        //        using (var reader = getProductsCommand.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                var product = new Product
        //                {
        //                    GoodCode = Convert.ToInt32(reader["GoodCode"]),
        //                    GoodName = reader["GoodName"].ToString(),
        //                    GoodDescription = reader["GoodDescription"].ToString(),
        //                    GoodPrice = Convert.ToDecimal(reader["GoodPrice"]),
        //                    ImgSrc = reader["ImgSrc"].ToString()
        //                };

        //                if (goodsDict.TryGetValue(product.GoodCode, out int count))
        //                {
        //                    for (int i = 0; i < count; i++)
        //                        products.Add(product);
        //                }
        //            }
        //        }

        //        return products;
        //    }
        //}


        public Product GetProductById(int id)
        {
            var productDb = _productApi.GetProductByIdApi(id); // API возвращает DB-модель

            if (productDb == null)
                return null;

            // Преобразуем в промежуточную модель
            return new Product
            {
                GoodCode = productDb.GoodCode,
                GoodName = productDb.GoodName,
                GoodDescription = productDb.GoodDescription,
                GoodPrice = productDb.GoodPrice,
                ImgSrc = productDb.ImgSrc,
                Category = productDb.Category
            };
        }

        //        public void AddProductToCart(int userId, int productId)
        //        {
        //            using (var connection = new SqlConnection(_connectionString))
        //            {
        //                connection.Open();

        //                // Получаем текущий CartId пользователя
        //                var getUserCommand = new SqlCommand("SELECT CartId FROM Users WHERE Id = @UserId", connection);
        //                getUserCommand.Parameters.AddWithValue("@UserId", userId);
        //                var cartIdObj = getUserCommand.ExecuteScalar();

        //                int cartId;

        //                if (cartIdObj == null || cartIdObj == DBNull.Value)
        //                {
        //                    // Создаем новую корзину
        //                    var createCartCommand = new SqlCommand("INSERT INTO Cart (Goods) OUTPUT INSERTED.ID VALUES (@Goods)", connection);
        //                    createCartCommand.Parameters.AddWithValue("@Goods", $"{{{productId}:1}}");
        //                    cartId = (int)createCartCommand.ExecuteScalar();

        //                    // Привязываем корзину к пользователю
        //                    var updateUserCommand = new SqlCommand("UPDATE Users SET CartId = @CartId WHERE Id = @UserId", connection);
        //                    updateUserCommand.Parameters.AddWithValue("@CartId", cartId);
        //                    updateUserCommand.Parameters.AddWithValue("@UserId", userId);
        //                    updateUserCommand.ExecuteNonQuery();
        //                }
        //                else
        //                {
        //                    cartId = Convert.ToInt32(cartIdObj);

        //                    // Получаем текущие данные корзины
        //                    var getCartCommand = new SqlCommand("SELECT Goods FROM Cart WHERE Id = @CartId", connection);
        //                    getCartCommand.Parameters.AddWithValue("@CartId", cartId);
        //                    var goodsStr = getCartCommand.ExecuteScalar()?.ToString() ?? "";

        //                    // Парсим строку в словарь
        //                    var goodsDict = new Dictionary<int, int>();
        //                    if (!string.IsNullOrWhiteSpace(goodsStr))
        //                    {
        //                        var entries = goodsStr.Split(',', (char)StringSplitOptions.RemoveEmptyEntries);
        //                        foreach (var entry in entries)
        //                        {
        //                            var trimmed = entry.Trim('{', '}');
        //                            var parts = trimmed.Split(':');
        //                            if (parts.Length == 2 && int.TryParse(parts[0], out int id) && int.TryParse(parts[1], out int qty))
        //                                goodsDict[id] = qty;
        //                        }
        //                    }

        //                    // Обновляем количество товара
        //                    if (goodsDict.ContainsKey(productId))
        //                        goodsDict[productId]++;
        //                    else
        //                        goodsDict[productId] = 1;

        //                    // Формируем строку обратно в формате {id:qty},{id2:qty2},...
        //                    var updatedGoods = string.Join(",", goodsDict.Select(kvp => $"{{{kvp.Key}:{kvp.Value}}}"));

        //                    var updateCartCommand = new SqlCommand("UPDATE Cart SET Goods = @Goods WHERE Id = @CartId", connection);
        //                    updateCartCommand.Parameters.AddWithValue("@Goods", updatedGoods);
        //                    updateCartCommand.Parameters.AddWithValue("@CartId", cartId);
        //                    updateCartCommand.ExecuteNonQuery();
        //                }
        //            }
        //        }

        //        public bool IncreaseProductQuantityInCart(int cartId, int productId)
        //        {
        //            using (var connection = new SqlConnection(_connectionString))
        //            {
        //                connection.Open();

        //                var selectCommand = new SqlCommand("SELECT Goods FROM Cart WHERE Id = @Id", connection);
        //                selectCommand.Parameters.AddWithValue("@Id", cartId);
        //                var goods = (string)selectCommand.ExecuteScalar();

        //                if (string.IsNullOrWhiteSpace(goods)) return false;

        //                var entries = goods.Split(',', (char)StringSplitOptions.RemoveEmptyEntries).ToList();
        //                for (int i = 0; i < entries.Count; i++)
        //                {
        //                    var entry = entries[i].Trim('{', '}').Split(':');
        //                    if (int.Parse(entry[0]) == productId)
        //                    {
        //                        int newQty = int.Parse(entry[1]) + 1;
        //                        entries[i] = $"{{{productId}:{newQty}}}";
        //                        break;
        //                    }
        //                }

        //                var newGoods = string.Join(",", entries);
        //                var updateCmd = new SqlCommand("UPDATE Cart SET Goods = @Goods WHERE Id = @Id", connection);
        //                updateCmd.Parameters.AddWithValue("@Goods", newGoods);
        //                updateCmd.Parameters.AddWithValue("@Id", cartId);
        //                updateCmd.ExecuteNonQuery();

        //                return true;
        //            }
        //        }

        //        public bool DecreaseProductQuantityInCart(int cartId, int productId)
        //        {
        //            using (var connection = new SqlConnection(_connectionString))
        //            {
        //                connection.Open();

        //                var selectCommand = new SqlCommand("SELECT Goods FROM Cart WHERE Id = @Id", connection);
        //                selectCommand.Parameters.AddWithValue("@Id", cartId);
        //                var goods = (string)selectCommand.ExecuteScalar();

        //                if (string.IsNullOrWhiteSpace(goods)) return false;

        //                var entries = goods.Split(',', (char)StringSplitOptions.RemoveEmptyEntries).ToList();
        //                for (int i = 0; i < entries.Count; i++)
        //                {
        //                    var entry = entries[i].Trim('{', '}').Split(':');
        //                    if (int.Parse(entry[0]) == productId)
        //                    {
        //                        int qty = int.Parse(entry[1]) - 1;
        //                        if (qty <= 0)
        //                            entries.RemoveAt(i);
        //                        else
        //                            entries[i] = $"{{{productId}:{qty}}}";
        //                        break;
        //                    }
        //                }

        //                var newGoods = string.Join(",", entries);
        //                var updateCmd = new SqlCommand("UPDATE Cart SET Goods = @Goods WHERE Id = @Id", connection);
        //                updateCmd.Parameters.AddWithValue("@Goods", newGoods);
        //                updateCmd.Parameters.AddWithValue("@Id", cartId);
        //                updateCmd.ExecuteNonQuery();

        //                return true;
        //            }
        //        }

        //        public bool DeleteProductFromCart(int cartId, int productId)
        //        {
        //            using (var connection = new SqlConnection(_connectionString))
        //            {
        //                connection.Open();

        //                var selectCommand = new SqlCommand("SELECT Goods FROM Cart WHERE Id = @Id", connection);
        //                selectCommand.Parameters.AddWithValue("@Id", cartId);
        //                var goods = (string)selectCommand.ExecuteScalar();

        //                if (string.IsNullOrWhiteSpace(goods)) return false;

        //                var entries = goods.Split(',', (char)StringSplitOptions.RemoveEmptyEntries).ToList();
        //                entries = entries
        //                    .Where(e => int.Parse(e.Trim('{', '}').Split(':')[0]) != productId)
        //                    .ToList();

        //                var newGoods = string.Join(",", entries);
        //                var updateCmd = new SqlCommand("UPDATE Cart SET Goods = @Goods WHERE Id = @Id", connection);
        //                updateCmd.Parameters.AddWithValue("@Goods", newGoods);
        //                updateCmd.Parameters.AddWithValue("@Id", cartId);
        //                updateCmd.ExecuteNonQuery();

        //                return true;
        //            }
        //        }

        //        public bool ConfirmOrder(UserModel user, out string errorMessage)
        //        {
        //            errorMessage = string.Empty;

        //            using (var connection = new SqlConnection(_connectionString))
        //            {
        //                connection.Open();

        //                if (!int.TryParse(user.CartId, out int cartId))
        //                {
        //                    errorMessage = "Некорректный идентификатор корзины.";
        //                    return false;
        //                }

        //                decimal balance = user.Balance;

        //                // Получаем содержимое корзины
        //                var getCartCmd = new SqlCommand("SELECT Goods FROM Cart WHERE Id = @CartId", connection);
        //                getCartCmd.Parameters.AddWithValue("@CartId", cartId);
        //                var goodsStr = getCartCmd.ExecuteScalar()?.ToString();

        //                if (string.IsNullOrWhiteSpace(goodsStr))
        //                {
        //                    errorMessage = "Корзина пуста.";
        //                    return false;
        //                }

        //                // Разбор строкового содержимого корзины
        //                var goodsDict = new Dictionary<int, int>();
        //                var entries = goodsStr.Split(',', (char)StringSplitOptions.RemoveEmptyEntries);

        //                foreach (var entry in entries)
        //                {
        //                    var parts = entry.Trim('{', '}').Split(':');
        //                    if (parts.Length == 2 &&
        //                        int.TryParse(parts[0], out int productId) &&
        //                        int.TryParse(parts[1], out int quantity))
        //                    {
        //                        goodsDict[productId] = quantity;
        //                    }
        //                }

        //                if (goodsDict.Count == 0)
        //                {
        //                    errorMessage = "Корзина не содержит допустимых товаров.";
        //                    return false;
        //                }

        //                // Получаем общую сумму заказа
        //                var productIds = string.Join(",", goodsDict.Keys);
        //                var getPricesCmd = new SqlCommand($"SELECT GoodCode, GoodPrice FROM Product WHERE GoodCode IN ({productIds})", connection);

        //                decimal totalSum = 0;
        //                using (var reader = getPricesCmd.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        int id = Convert.ToInt32(reader["GoodCode"]);
        //                        decimal price = Convert.ToDecimal(reader["GoodPrice"]);

        //                        if (goodsDict.TryGetValue(id, out int count))
        //                            totalSum += price * count;
        //                    }
        //                }

        //                // Проверка баланса
        //                if (totalSum > balance)
        //                {
        //                    errorMessage = "У вас недостаточно средств для этого заказа!";
        //                    return false;
        //                }

        //                // Копируем корзину в OrderHistory с привязкой к UserId
        //                var insertOrderCmd = new SqlCommand(
        //                    "INSERT INTO OrderHistory (UserId, Goods, CheckOut) VALUES (@UserId, @Goods, @CheckOut)", connection);
        //                insertOrderCmd.Parameters.AddWithValue("@UserId", user.Id);
        //                insertOrderCmd.Parameters.AddWithValue("@Goods", goodsStr);
        //                insertOrderCmd.Parameters.AddWithValue("@CheckOut", totalSum);
        //                insertOrderCmd.ExecuteNonQuery();

        //                // Обновляем баланс пользователя
        //                var updateUserCmd = new SqlCommand(
        //                    "UPDATE Users SET Balance = @Balance WHERE Id = @UserId", connection);
        //                updateUserCmd.Parameters.AddWithValue("@Balance", balance - totalSum);
        //                updateUserCmd.Parameters.AddWithValue("@UserId", user.Id);
        //                updateUserCmd.ExecuteNonQuery();

        //                // Очищаем корзину
        //                var clearCartCmd = new SqlCommand("UPDATE Cart SET Goods = NULL WHERE Id = @CartId", connection);
        //                clearCartCmd.Parameters.AddWithValue("@CartId", cartId);
        //                clearCartCmd.ExecuteNonQuery();

        //                return true;
        //            }
        //        }


        //        public List<(List<Product> Products, decimal Checkout)> GetOrdersByUserId(int userId)
        //{
        //    var result = new List<(List<Product>, decimal)>();

        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        connection.Open();

        //        // Получаем все записи из OrderHistory для пользователя
        //        var command = new SqlCommand("SELECT Goods, Checkout FROM OrderHistory WHERE UserId = @UserId", connection);
        //        command.Parameters.AddWithValue("@UserId", userId);

        //        using (var reader = command.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                var goodsStr = reader["Goods"].ToString();
        //                decimal checkout = Convert.ToDecimal(reader["Checkout"]);

        //                var goodsDict = new Dictionary<int, int>();

        //                var entries = goodsStr.Split(',', (char)StringSplitOptions.RemoveEmptyEntries);
        //                foreach (var entry in entries)
        //                {
        //                    var trimmed = entry.Trim('{', '}');
        //                    var parts = trimmed.Split(':');
        //                    if (parts.Length == 2 &&
        //                        int.TryParse(parts[0], out int productId) &&
        //                        int.TryParse(parts[1], out int quantity))
        //                    {
        //                        goodsDict[productId] = quantity;
        //                    }
        //                }

        //                if (goodsDict.Count == 0)
        //                    continue;

        //                var idList = string.Join(",", goodsDict.Keys);
        //                var products = new List<Product>();

        //                using (var innerConnection = new SqlConnection(_connectionString))
        //                {
        //                    innerConnection.Open();

        //                    var getProductsCommand = new SqlCommand($"SELECT * FROM Product WHERE GoodCode IN ({idList})", innerConnection);
        //                    using (var productReader = getProductsCommand.ExecuteReader())
        //                    {
        //                        while (productReader.Read())
        //                        {
        //                            var product = new Product
        //                            {
        //                                GoodCode = Convert.ToInt32(productReader["GoodCode"]),
        //                                GoodName = productReader["GoodName"].ToString(),
        //                                GoodDescription = productReader["GoodDescription"].ToString(),
        //                                GoodPrice = Convert.ToDecimal(productReader["GoodPrice"]),
        //                                ImgSrc = productReader["ImgSrc"].ToString()
        //                            };

        //                            if (goodsDict.TryGetValue(product.GoodCode, out int count))
        //                            {
        //                                for (int i = 0; i < count; i++)
        //                                    products.Add(product);
        //                            }
        //                        }
        //                    }
        //                }

        //                result.Add((products, checkout));
        //            }
        //        }
        //    }

        //    return result;
        //}

        //public void UpdateUserBalance(int userId, decimal newBalance)
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        connection.Open();
        //        var cmd = new SqlCommand("UPDATE Users SET Balance = @Balance WHERE Id = @UserId", connection);
        //        cmd.Parameters.AddWithValue("@Balance", newBalance);
        //        cmd.Parameters.AddWithValue("@UserId", userId);
        //        cmd.ExecuteNonQuery();
        //    }
        //}
    }
}
