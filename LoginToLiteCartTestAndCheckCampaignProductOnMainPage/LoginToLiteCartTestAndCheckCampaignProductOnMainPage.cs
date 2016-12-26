// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoginToLiteCartTestAndCheckProductsOnMainPage.cs" company="">
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
    using OpenQA.Selenium.Support.UI;

    /// <summary>
    /// The login to lite cart test.
    /// </summary>
    [TestClass]
    public class LoginToLiteCartTestAndCheckCampaignProductOnMainPage
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
        public void Login_To_LiteCart_And_Check_Products_On_Main_Page_Campaigns_Category_Test()
        {
            this.OpenLiteCart();

            this.CheckMainPageCampaignsItems();
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
        /// The check main page campaigns items.
        /// </summary>
        public void CheckMainPageCampaignsItems()
        {
            var allItemsOnMainPage = this.driver.FindElements(By.CssSelector("div#box-campaigns ul li"));

            if (allItemsOnMainPage.Count.Equals(0))
            {
                throw new Exception("\r\nERROR: There is no any product on Main page in Campaigns category");
            }

            Assert.IsTrue(this.driver.FindElement(By.Id("breadcrumbs")).Displayed);

            var stiker = allItemsOnMainPage.First().FindElement(By.TagName("a"));

            var nameofProduct = stiker.GetAttribute("title");
            var priceElement = stiker
                                .FindElements(By.TagName("div"))
                                .Single(item => item.GetAttribute("class").Equals("price-wrapper"));

            // Get Color and BackGround for Price(regular and campaign)
            var priceColorDict = new Dictionary<string, string>();
            var priceBackGroundDict = new Dictionary<string, string>();

            var regularPrice = priceElement.FindElement(By.TagName("s"));
            var regularPriceStyleColor = regularPrice.GetCssValue("color");
            var regularPriceStyleBackGround = regularPrice.GetCssValue("background");

            priceColorDict.Add(regularPrice.Text, regularPriceStyleColor);
            priceBackGroundDict.Add(regularPrice.Text, regularPriceStyleBackGround);

            var campaignPrice = priceElement.FindElement(By.TagName("strong"));
            var campaignPriceStyleColor = campaignPrice.GetCssValue("color");
            var campaignPriceStyleBackGround = campaignPrice.GetCssValue("background");

            priceColorDict.Add(campaignPrice.Text, campaignPriceStyleColor);
            priceBackGroundDict.Add(campaignPrice.Text, campaignPriceStyleBackGround);

            // Goto Product page
            stiker.Click();

            var titleOfProduct = this.driver.FindElement(By.CssSelector("h1[class=title]"));

            Assert.AreEqual(titleOfProduct.Text, nameofProduct, " \r\nERROR: Title of Product + " + nameofProduct + " was not loaded correctly. ");

            var informationPrice = this.driver.FindElement(By.CssSelector("div div[class=price-wrapper]"));

            // Get Color and BackGround for Price(regular and campaign)
            var priceColorDictInProduct = new Dictionary<string, string>();
            var priceBackGroundDictInProduct = new Dictionary<string, string>();

            var regularPriceInProduct = informationPrice.FindElement(By.TagName("s"));
            var regularPriceInProductStyleColor = regularPriceInProduct.GetCssValue("color");
            var regularPriceInProductStyleBackGround = regularPriceInProduct.GetCssValue("background");

            priceColorDictInProduct.Add(regularPriceInProduct.Text, regularPriceInProductStyleColor);
            priceBackGroundDictInProduct.Add(regularPriceInProduct.Text, regularPriceInProductStyleBackGround);

            var campaignPriceInProduct = informationPrice.FindElement(By.TagName("strong"));
            var campaignPriceInProductStyleColor = campaignPriceInProduct.GetCssValue("color");
            var campaignPriceInProductStyleBackGround = campaignPriceInProduct.GetCssValue("background");

            priceColorDictInProduct.Add(campaignPriceInProduct.Text, campaignPriceInProductStyleColor);
            priceBackGroundDictInProduct.Add(campaignPriceInProduct.Text, campaignPriceInProductStyleBackGround);

            Assert.AreEqual(priceColorDict, priceColorDictInProduct, "\r\nERROR: Price and Color are not equal with price and color in Main page");
            Assert.AreEqual(priceBackGroundDict, priceBackGroundDictInProduct, "\r\nERROR: Price and BackGround are not equal with price and background in Main page");
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
