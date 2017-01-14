// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoginToLiteCartAndCheckCountiresTest.cs" company="">
//   
// </copyright>
// <summary>
//   The login to lite cart test.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CSharpSeleniumExample
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OpenQA.Selenium;
    using OpenQA.Selenium.IE;
    using OpenQA.Selenium.Support.Extensions;
    using OpenQA.Selenium.Support.UI;

    /// <summary>
    /// The login to lite cart test.
    /// </summary>
    [TestClass]
    public class AddProductToBin
    {
        /// <summary>
        /// The driver.
        /// </summary>
        private IWebDriver driver;

        /// <summary>
        /// The wait.
        /// </summary>
        private WebDriverWait wait;

        /// <summary>
        /// The start.
        /// </summary>
        [TestInitialize]
        public void Start()
        {
            this.driver = new InternetExplorerDriver();
            this.driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(15000));
            this.wait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(15));
        }

        /// <summary>
        /// The link to lite cart.
        /// </summary>
        private readonly string LinkToLiteCart = "http://localhost:81/litecart/";

        /// <summary>
        /// The login to lite cart and click to all menu items.
        /// </summary>
        [TestMethod]
        public void Add_Products_To_Bin_Test()
        {
            this.LoginToLiteCart();

            this.AddAllItemsToBin();

            this.ClearBin();
        }

        /// <summary>
        /// The login to lite cart.
        /// </summary>
        public void LoginToLiteCart()
        {
            this.driver.Url = this.LinkToLiteCart;

            // Check Login was done
            this.wait.Until(ExpectedConditions.TitleIs("Online Store | My Store"));
        }

        /// <summary>
        /// The open any product details.
        /// </summary>
        public void OpenAnyProductDetails()
        {
            var allItemsOnMainPage = this.driver.FindElements(By.CssSelector("div#box-most-popular ul li"));

            Console.WriteLine("\r\n Most Popular Items: ");

            var item = allItemsOnMainPage.RandomElement();

            item.Click();
        }

        /// <summary>
        /// The add item to bin.
        /// </summary>
        public void AddItemToBin(int countOfItemsAddedToCart)
        {
            if (this.isElementPresent(this.driver, By.CssSelector("td[class=options]")))
            {
                var sizeElement = this.driver.FindElement(By.CssSelector("td[class=options]"));
                var dropDown = sizeElement.FindElement(By.TagName("select"));

                this.SelectItemInDropDownList(dropDown, 0, 2);
            }

            var cart = this.driver.FindElement(By.Id("cart"));
            var quantityOld = cart.FindElement(By.CssSelector("a span[class=quantity]"));

            var button = this.driver.FindElement(By.CssSelector("button[name=add_cart_product]"));

            button.Click();

            var quantity = cart.FindElement(By.CssSelector("a span[class=quantity]"));

            if (this.isElementPresent(this.driver, By.CssSelector("a span[class=quantity]")) && Int32.Parse(quantity.Text) > Int32.Parse(quantityOld.Text))
            {
                quantity = cart.FindElement(By.CssSelector("a span[class=quantity]"));

                Assert.AreEqual(countOfItemsAddedToCart.ToString(), quantity.Text);
            }
        }

        /// <summary>
        /// The select item in drop down list.
        /// </summary>
        /// <param name="element">
        /// The element.
        /// </param>
        /// <param name="argumentIndex">
        /// The argument index.
        /// </param>
        /// <param name="selectedItemIndex">
        /// The selected item index.
        /// </param>
        public void SelectItemInDropDownList(IWebElement element, int argumentIndex, int selectedItemIndex)
        {
            var scriptSelect = String.Format("arguments[{0}].selectedIndex = {1}", argumentIndex, selectedItemIndex);
            var scriptShow = String.Format("arguments[{0}].style.opacity = {1}", argumentIndex, selectedItemIndex);

            this.driver.ExecuteJavaScript(scriptSelect, element);
            this.driver.ExecuteJavaScript(scriptShow, element);
        }

        /// <summary>
        /// The is element present.
        /// </summary>
        /// <param name="webDriver">
        /// The web Driver.
        /// </param>
        /// <param name="locator">
        /// The locator.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool isElementPresent(IWebDriver webDriver, By locator)
        {
            try
            {
                webDriver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0));
                return webDriver.FindElements(locator).Count > 0;
            }
            finally
            {
                webDriver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(10000));
            }
        }

        public void AddAllItemsToBin()
        {
            for (int i = 0; i < 3; i++)
            {
                this.OpenAnyProductDetails();

                this.AddItemToBin(i);

                var allbreadCrumbs = this.driver.FindElements(By.CssSelector("nav[id=breadcrumbs] ul li a"));

                var home = allbreadCrumbs.FirstOrDefault(item => item.Text.Equals("Home"));

                home?.Click();
            }
        }

        public void ClearBin()
        {
            var cart = this.driver.FindElement(By.Id("cart"));

            cart.Click();

            var list = new List<IWebElement>();
            var itemsTable = this.driver.FindElement(By.Id("order_confirmation-wrapper"));
            var allItemsInTable = itemsTable.FindElements(By.TagName("tr"));
            foreach (var webElement in allItemsInTable)
            {
                if (webElement.GetAttribute("class") != "header")
                {
                    var allAttributesOfItem = webElement.FindElements(By.CssSelector("td[class=item]"));
                    if (allAttributesOfItem.Count> 0)
                    {
                        list.Add(webElement);
                    }
                }
            }

            foreach (var element in list)
            {
                var alementOfProduct = element.FindElements(By.TagName("td"));
                var firstOrDefault = alementOfProduct.FirstOrDefault(item => item.GetAttribute("Class").Equals("item"));
                if (firstOrDefault != null)
                {
                    var text = firstOrDefault.Text;

                    this.RemoveItemFromBin(text);
                }

                // Check
                this.driver.Navigate().Refresh();
                this.wait.Until(ExpectedConditions.StalenessOf(element));
            }
       }

        public void RemoveItemFromBin(string text)
        {
            var allItemsToRemove = this.driver.FindElements(By.CssSelector("ul[class=shortcuts] li"));
            foreach (var webElement in allItemsToRemove)
            {
                var toChoose = webElement.FindElements(By.CssSelector("a[href=#]"));
                var firstOrDefault = toChoose.FirstOrDefault();
                firstOrDefault?.Click();

                var textOfProduct = this.driver.FindElement(By.CssSelector("div p a"));
                if (textOfProduct.Text.Equals(text))
                {
                    var button = this.driver.FindElement(By.CssSelector("button[name=remove_cart_item]"));

                    button.Click();
                }
            }
        }

        /// <summary>
        /// The stop.
        /// </summary>
        [TestCleanup]
        public void Stop()
        {
            this.driver.Quit();
            this.driver = null;
        }
    }

}
