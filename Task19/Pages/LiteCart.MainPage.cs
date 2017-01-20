// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LiteCart.MainPage.cs" company="">
//   
// </copyright>
// <summary>
//   The lite cart main page.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CSharpSeleniumExample.Task19.Pages
{
    using System;
    using OpenQA.Selenium;

    /// <summary>
    /// The lite cart main page.
    /// </summary>
    internal class LiteCartMainPage : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LiteCartMainPage"/> class.
        /// </summary>
        /// <param name="driver">
        /// The driver.
        /// </param>
        public LiteCartMainPage(IWebDriver driver)
            : base(driver)
        {
        }

        /// <summary>
        /// The open.
        /// </summary>
        /// <returns>
        /// The <see cref="LiteCartMainPage"/>.
        /// </returns>
        internal LiteCartMainPage Open()
        {
            this.driver.Url = "http://localhost:81/litecart/";
            return this;
        }

        /// <summary>
        /// The is on this page.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        internal bool IsOnThisPage()
        {
            return this.driver.FindElements(By.Id("box-login")).Count > 0;
        }

        /// <summary>
        /// The open any product details.
        /// </summary>
        internal void OpenAnyProductDetails()
        {
            var allItemsOnMainPage = this.driver.FindElements(By.CssSelector("div#box-most-popular ul li"));

            Console.WriteLine("\r\n Most Popular Items: ");

            var item = allItemsOnMainPage.RandomElement();

            item.Click();
        }
    }
}
