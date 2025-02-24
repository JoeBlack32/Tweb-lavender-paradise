﻿using LavenderParadise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LavenderParadise.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Catalog()
        {
            List<GoodsCard> goods = new List<GoodsCard>{
                new GoodsCard { 
                    GoodName = "Эфирное масло лаванды", 
                    GoodCode = 1, 
                    GoodDescription = "Натуральный продукт с лавандой", 
                    GoodPrice = 1200, 
                    ImgSrc="../src/img/maslo-lovaadi.jpg",
                    Category = "Aromatherapy"
                
                },
                new GoodsCard { 
                    GoodName = "Лавандовый чай", 
                    GoodCode = 2, 
                    GoodDescription = "Ароматный травяной чай", 
                    GoodPrice = 800,
                    ImgSrc="../src/img/chai.jpg",
                    Category = "Aromatherapy"

                },
                new GoodsCard { 
                    GoodName = "Мыло с лавандой", 
                    GoodCode = 3, 
                    GoodDescription = "Натуральное мыло ручной работы", 
                    GoodPrice = 500,
                    ImgSrc="../src/img/milo.jpg",
                    Category = "Aromatherapy"
                },
                new GoodsCard { GoodName = "Мыло с лавандой", 
                    GoodCode = 4, 
                    GoodDescription = "Натуральное мыло ручной работы", 
                    GoodPrice = 500, 
                    ImgSrc = "../src/img/milo.jpg", 
                    Category = "Body Care" 
                },
                new GoodsCard { 
                    GoodName = "Соль для ванны", 
                    GoodCode = 5, 
                    GoodDescription = "Ароматная соль с лавандой", 
                    GoodPrice = 850, 
                    ImgSrc = "../src/img/soli.jpg", 
                    Category = "Body Care"
                },
                new GoodsCard { GoodName = "Лавандовый крем", 
                    GoodCode = 6, 
                    GoodDescription = "Натуральный увлажняющий крем", 
                    GoodPrice = 1250, 
                    ImgSrc = "../src/img/krem.jpg", 
                    Category = "Body Care"
                },
                new GoodsCard { 
                    GoodName = "Свеча с лавандой", 
                    GoodCode = 7, 
                    GoodDescription = "Ароматическая свеча", 
                    GoodPrice = 750, 
                    ImgSrc = "../src/img/svechka.jpg", 
                    Category = "For Home and Rest"
                },
                new GoodsCard { 
                    GoodName = "Саше с лавандой", 
                    GoodCode = 8, 
                    GoodDescription = "Ароматическое саше", 
                    GoodPrice = 300, 
                    ImgSrc = "../src/img/sahe.jpg", 
                    Category = "For Home and Rest"
                },
                new GoodsCard { 
                    GoodName = "Ароматический спрей", 
                    GoodCode = 9, 
                    GoodDescription = "Спрей для дома", 
                    GoodPrice = 900, 
                    ImgSrc = "../src/img/dva-sprei.jpg", 
                    Category = "For Home and Rest"
                },
                new GoodsCard { 
                    GoodName = "Чай с лавандой", 
                    GoodCode = 10, 
                    GoodDescription = "Травяной чай", 
                    GoodPrice = 600, 
                    ImgSrc = "../src/img/chai.jpg", 
                    Category = "Products for Consuming" 
                },
                new GoodsCard { 
                    GoodName = "Мёд с лавандой", 
                    GoodCode = 11, 
                    GoodDescription = "Натуральный мёд", 
                    GoodPrice = 1400, 
                    ImgSrc = "../src/img/med.jpg", 
                    Category = "Products for Consuming"
                }
            };
            return View(goods);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Registration()
        {
            return View();
        }

        public ActionResult Avtorization()
        {
            return View();
        }
    }
}