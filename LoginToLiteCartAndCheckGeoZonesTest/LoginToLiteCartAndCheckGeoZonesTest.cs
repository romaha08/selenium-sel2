// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoginToLiteCartAndCheckGeoZonesTest.cs" company="">
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
    public class LoginToLiteCartAndCheckGeoZones
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
            this.wait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(15));
        }

        /// <summary>
        /// The link to lite cart.
        /// </summary>
        private readonly string LinkToLiteCart = "http://localhost:81/litecart/admin/";

        /// <summary>
        /// The login.
        /// </summary>
        private readonly string Login = "admin";

        /// <summary>
        /// The password.
        /// </summary>
        private readonly string Password = "admin";


        /// <summary>
        /// The login to lite cart and click to all menu items.
        /// </summary>
        [TestMethod]
        public void Login_To_LiteCart_And_Check_Geo_Zones()
        {
            this.LoginToLiteCart();

            this.ClickToMenuItem("Geo Zones");

            this.CheckZonesByAlphabet();
        }

        /// <summary>
        /// The login to lite cart.
        /// </summary>
        public void LoginToLiteCart()
        {
            this.driver.Url = this.LinkToLiteCart;
            this.driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(1000));

            // Check page was loaded correctly(Only form checking)
            this.CheckLoginFormLoaded();

            this.driver.FindElement(By.Name("username")).SendKeys(this.Login);
            this.driver.FindElement(By.Name("password")).SendKeys(this.Password);
            this.driver.FindElement(By.Name("login")).Click();

            // Check Login was done
            this.wait.Until(ExpectedConditions.TitleIs("My Store"));
        }

        /// <summary>
        /// The click to menu item.
        /// </summary>
        /// <param name="nameOfMenu">
        /// The name of menu.
        /// </param>
        public void ClickToMenuItem(string nameOfMenu)
        {
            var leftMenu = this.driver.FindElement(By.Id("box-apps-menu"));
            var leftMenuItems = leftMenu.FindElements(By.Id("app-"));

            var element = leftMenuItems.SingleOrDefault(item => item.Text.ToLower().Equals(nameOfMenu.ToLower()));

            Assert.IsNotNull(element, "\r\nERROR: Element with name " + nameOfMenu + " was not found. Please check page you're looking at.");

            element.Click();

            this.wait.Until(ExpectedConditions.TitleIs(nameOfMenu + " | My Store"));
        }

        /// <summary>
        /// The check countries by alphabet.
        /// </summary>
        public void CheckZonesByAlphabet()
        {
            var tableForm = this.driver.FindElements(By.CssSelector("form[name=geo_zones_form]"));
            Assert.IsTrue(
                tableForm.Count.Equals(1),
                "\r\nERROR: Count of Geo Zones by search criteria 'form' is more than 1. Please check page.");

            var allZones = tableForm.First().FindElements(By.CssSelector("tr[class=row]"));

            var listOfZones = new List<string>();
            var tempList = new List<string>();

            foreach (var zone in allZones)
            {
                var lineElements = zone.FindElements(By.TagName("td"));

                tempList.Add(lineElements[2].FindElement(By.TagName("a")).Text);
            }

            foreach (var zone in tempList)
            {
                tableForm = this.driver.FindElements(By.CssSelector("form[name=geo_zones_form]"));
                allZones = tableForm.First().FindElements(By.CssSelector("tr[class=row]"));

                var element = allZones.Single(item => item.FindElements(By.TagName("td"))[2].Text.Equals(zone));

                var linkToClick = element.FindElement(By.TagName("a"));

                linkToClick.Click();

                var table = this.driver.FindElement(By.Id("table-zones"));

                var elementOfZones = table.FindElements(By.TagName("tr"));

                foreach (var elementOfZone in elementOfZones)
                {
                    if (String.IsNullOrEmpty(elementOfZone.GetAttribute("class")))
                    {
                        var tenpElements = elementOfZone.FindElements(By.TagName("td"));

                        if (tenpElements.Count > 1)
                        {
                            var zoneChooses = tenpElements[2].FindElement(By.TagName("select"));

                            var selectedOption =
                                zoneChooses.FindElements(By.TagName("option"))
                                    .Single(item => item.Selected.Equals(true));

                            listOfZones.Add(selectedOption.Text.ToString());
                        }
                    }
                }

                // Check Alphabet sorting
                var tempListOfZoneCodes = new List<string>();
                tempListOfZoneCodes = listOfZones;
                tempListOfZoneCodes.Sort();

                Assert.AreEqual(tempListOfZoneCodes, listOfZones, "\r\nERROR: Zones are not in alphabetic order");

                this.ClickToMenuItem("Geo Zones");
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
