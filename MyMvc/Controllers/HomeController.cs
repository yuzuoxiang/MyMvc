using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyMvc.Models;
using Ninject;

namespace MyMvc.Controllers
{
    public class HomeController : Controller
    {
        private IValueCalculator calc;
        private Product[] products ={
            new Product{ Name="aaa",Category="wkwekee",Price=25M},
            new Product{ Name="bbb",Category="sdfsdfd",Price=13M},
            new Product{ Name="ccc",Category="ewewewr",Price=52M},
            new Product{ Name="ddd",Category="ggdsdfs",Price=44M},
            new Product{ Name="eee",Category="aasdddd",Price=66M},
        };

        public HomeController(IValueCalculator calcParam)
        {
            calc = calcParam;
        }

        // GET: Home
        public ActionResult Index()
        {
            //创建Ninject内核
            IKernel ninjectKernel = new StandardKernel();
            //配置Ninject内核
            ninjectKernel.Bind<IValueCalculator>().To<LinqValueCalculator>();
            //使用Ninject来创建一个对象
            IValueCalculator calc = ninjectKernel.Get<IValueCalculator>();

            ShopingCart cart = new ShopingCart(calc) { Products = products };
            decimal totalValue = cart.CalculateProductTotal();
            return View(totalValue);
        }
    }
}