// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoginToLiteCartTestAndCheckMainPage.cs" company="">
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
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OpenQA.Selenium;
    using OpenQA.Selenium.IE;
    using OpenQA.Selenium.Support.UI;

    /// <summary>
    /// The login to lite cart test.
    /// </summary>
    [TestClass]
    public class LoginToLiteCartTestAndCheckMainPage
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
            this.wait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// The link to lite cart.
        /// </summary>
        private readonly string LinkToLiteCart = "http://localhost:81/litecart";

        /// <summary>
        /// The login to lite cart and check items on page.
        /// </summary>
        [TestMethod]
        public void LoginToLiteCartAndCheckItemsOnPage()
        {
            this.OpenLiteCart();

            this.CheckMainPagePopularItems();

            this.CheckMainPageCampaignsItems();

            this.CheckMainPageLatestProductsItems();
        }

        /// <summary>
        /// The login to lite cart.
        /// </summary>
        public void OpenLiteCart()
        {
            this.driver.Url = this.LinkToLiteCart;

            // Check Page was loaded
            this.wait.Until(ExpectedConditions.TitleIs("Online Store | My Store"));
        }

        /// <summary>
        /// The check main page popular items.
        /// </summary>
        public void CheckMainPagePopularItems()
        {
            var allItemsOnMainPage = this.driver.FindElements(By.CssSelector("div#box-most-popular ul li"));
            var tempList = new List<string>();

            Console.WriteLine("\r\n Most Popular Items: ");

            foreach (var item in allItemsOnMainPage)
            {
                var stiker = item.FindElements(By.TagName("div"));
                foreach (var webElement in stiker)
                {
                    if (webElement.GetAttribute("class").Contains("sticker"))
                    {
                        tempList.Add(webElement.Text);
                    }
                }
                
                Console.WriteLine("\r\n Item checked: " + item.Text);
                Assert.AreEqual(tempList.Count, 1, "\r\nERROR: There is NOT one Sticker of Element: " + item.Text);

                tempList = new List<string>();
            }
        }

        /// <summary>
        /// The check main page campaigns items.
        /// </summary>
        public void CheckMainPageCampaignsItems()
        {
            var allItemsOnMainPage = this.driver.FindElements(By.CssSelector("div#box-campaigns ul li"));
            var tempList = new List<string>();

            Console.WriteLine("\r\n Campaigns Items: ");

            foreach (var item in allItemsOnMainPage)
            {
                var stiker = item.FindElements(By.TagName("div"));
                foreach (var webElement in stiker)
                {
                    if (webElement.GetAttribute("class").Contains("sticker"))
                    {
                        tempList.Add(webElement.Text);
                    }
                }

                Console.WriteLine("\r\n Item checked: " + item.Text);
                Assert.AreEqual(tempList.Count, 1, "\r\nERROR: There is NOT one Sticker of Element: " + item.Text);

                tempList = new List<string>();
            }
        }

        /// <summary>
        /// The check main page latest products items.
        /// </summary>
        public void CheckMainPageLatestProductsItems()
        {
            var allItemsOnMainPage = this.driver.FindElements(By.CssSelector("div#box-latest-products ul li"));
            var tempList = new List<string>();

            Console.WriteLine("\r\n LAtest Products Items: ");

            foreach (var item in allItemsOnMainPage)
            {
                var stiker = item.FindElements(By.TagName("div"));
                foreach (var webElement in stiker)
                {
                    if (webElement.GetAttribute("class").Contains("sticker"))
                    {
                        tempList.Add(webElement.Text);
                    }
                }

                Console.WriteLine("\r\n Item checked: " + item.Text);
                Assert.AreEqual(tempList.Count, 1, "\r\nERROR: There is NOT one Sticker of Element: " + item.Text);

                tempList = new List<string>();
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

        /// <summary>
        /// The check user name field exists.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public IWebElement CheckUserNameFieldExists()
        {
            try
            {
                return this.driver.FindElement(By.Name("username"));
            }
            catch (Exception e)
            {
                throw new Exception("\r\n UserName not found on page. Exception is: " + e.Message);
            }
        }

        /// <summary>
        /// The check password field exists.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public IWebElement CheckPasswordFieldExists()
        {
            try
            {
                return this.driver.FindElement(By.Name("password"));
            }
            catch (Exception e)
            {
                throw new Exception("\r\n Password field not found on page. Exception is: " + e.Message);
            }
        }

        /// <summary>
        /// The check button login exists.
        /// </summary>
        /// <returns>
        /// The <see cref="IWebElement"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public IWebElement CheckButtonLoginExists()
        {
            try
            {
                return this.driver.FindElement(By.Name("login"));
            }
            catch (Exception e)
            {
                throw new Exception("\r\n Button Login not found on page. Exception is: " + e.Message);
            }
        }

        /// <summary>
        /// The check login form loaded.
        /// </summary>
        /// <exception cref="Exception">
        /// </exception>
        public void CheckLoginFormLoaded()
        {
            try
            {
                this.driver.FindElement(By.Name("login_form"));
            }
            catch (Exception e)
            {
                throw new Exception("\r\n Page was not loaded correctly.\r\n" + e.Message);
            }
        }
    }
}
