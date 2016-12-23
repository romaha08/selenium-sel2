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
    using OpenQA.Selenium.Support.UI;

    /// <summary>
    /// The login to lite cart test.
    /// </summary>
    [TestClass]
    public class LoginToLiteCartAndCheckCountries
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
        public void Login_To_LiteCart_And_Check_Countries_Test()
        {
            this.LoginToLiteCart();

            this.ClickToMenuItem("Countries");

            this.CheckCountriesByAlphabet();
        }

        /// <summary>
        /// The login to lite cart.
        /// </summary>
        public void LoginToLiteCart()
        {
            this.driver.Url = this.LinkToLiteCart;

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
        public void CheckCountriesByAlphabet()
        {
            var tableForm = this.driver.FindElements(By.CssSelector("form[name=countries_form]"));

            Assert.IsTrue(tableForm.Count.Equals(1), "\r\nERROR: Count of Elements by search criteria 'form' is more than 1. Please check page.");

            var allCountries = tableForm.First().FindElements(By.CssSelector("tr[class=row]"));

            var tempDictOfCountries = new Dictionary<string, int>();
            var listOfCountries = new List<string>();

            foreach (var country in allCountries)
            {
                var lineElements = country.FindElements(By.TagName("td"));

                var countryName = lineElements[4].Text;
                var zone = Int32.Parse(lineElements[5].Text);

                listOfCountries.Add(countryName);

                if (zone != 0)
                {
                    tempDictOfCountries.Add(countryName, zone);
                }
            }

            // Check Alphabet sorting
            var tempListOfCountries = new List<string>();
            tempListOfCountries = listOfCountries;
            tempListOfCountries.Sort();

            Assert.AreEqual(tempListOfCountries, listOfCountries, "\r\nERROR: Countries are not in alphabetic order");

            // Check zoneCode > 0
            foreach (var countryWizZone in tempDictOfCountries)
            {
                tableForm = this.driver.FindElements(By.CssSelector("form[name=countries_form]"));

                allCountries = tableForm.First().FindElements(By.CssSelector("tr[class=row]"));

                var element =
                    allCountries
                        .Single(item => item.FindElements(By.TagName("td"))[4].Text.Equals(countryWizZone.Key));
                
                var linkToClick = element.FindElement(By.TagName("a"));

                linkToClick.Click();
                
                this.CheckZoneCodesByCountry();

                this.ClickToMenuItem("Countries");
            }
        }

        /// <summary>
        /// The check zone codes by country.
        /// </summary>
        public void CheckZoneCodesByCountry()
        {
            var table = this.driver.FindElement(By.Id("table-zones"));

            var zones = table.FindElements(By.CssSelector("tr"));

            var listOfZones = new List<string>();
            foreach (var zone in zones)
            {
                if (String.IsNullOrEmpty(zone.GetAttribute("class")))
                {
                    var lineElements = zone.FindElements(By.TagName("td"));
                    var zoneName = lineElements[2].Text;

                    listOfZones.Add(zoneName);
                }
            }

            // Check Alphabet sorting
            var tempListOfCountries = new List<string>();
            tempListOfCountries = listOfZones;
            tempListOfCountries.Sort();

            Assert.AreEqual(tempListOfCountries, listOfZones, "\r\nERROR: Zones are not in alphabetic order");
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
