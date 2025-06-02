// Пример CartApi.cs
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Tweb_lavender_paradise.BusinessLogic.Context;
using Tweb_lavender_paradise.BusinessLogic.DBModel;
using Tweb_lavender_paradise.BusinessLogic.Interfaces;
using Tweb_lavender_paradise.Domain.Enitities.User;
using Tweb_lavender_paradise.Domain.Entities;
using Tweb_lavender_paradise.Domain.Models;

public class CartApi : ICart
{
    public CartDBTable GetCartById(int cartId)
    {
        using (var db = new CartContext())
        {
            return db.Carts.Find(cartId);
        }
    }

    public UserDBTable GetUserById(int userId)
    {
        using (var db = new UserContext())
        {
            return db.Users.Find(userId);
        }
    }

    public void UpdateCart(CartDBTable cart)
    {
        using (var db = new CartContext())
        {
            db.Entry(cart).State = EntityState.Modified;
            db.SaveChanges();
        }
    }

    public void AddCart(CartDBTable cart)
    {
        using (var db = new CartContext())
        {
            db.Carts.Add(cart);
            db.SaveChanges();
        }
    }

    public void UpdateUser(UserDBTable user)
    {
        using (var db = new UserContext())
        {
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
        }
    }

    public List<ProductDBTable> GetProductsByIds(List<int> ids)
    {
        using (var db = new ProductContext())
        {
            return db.Products.Where(p => ids.Contains(p.GoodCode)).ToList();
        }
    }

    public void AddOrder(OrderHistoryDBTable history)
    {
        using (var db = new UserContext())
        {
            db.OrderHistory.Add(history);
            db.SaveChanges();
        }
    }
}
