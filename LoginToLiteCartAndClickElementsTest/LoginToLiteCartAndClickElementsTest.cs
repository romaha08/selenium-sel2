// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoginToLiteCartAndClickElementsTest.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the LoginToLiteCartTest type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CSharpSeleniumExample
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OpenQA.Selenium;
    using OpenQA.Selenium.IE;
    using OpenQA.Selenium.Support.UI;

    /// <summary>
    /// The login to lite cart test.
    /// </summary>
    [TestClass]
    public class LoginToLiteCartTestAndClickElements
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
        /// The link to menu items.
        /// </summary>
        private readonly string TitlePart = " | My Store";

        /// <summary>
        /// The login to lite cart and click to all menu items.
        /// </summary>
        [TestMethod]
        public void LoginToLiteCartAndClickToAllMenuItems()
        {
            this.LoginToLiteCart();

            this.GotoMenuPanel();
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
        /// The goto menu panel.
        /// </summary>
        public void GotoMenuPanel()
        {
            this.ClickToMenuItem("Appearence", "Template");

            this.ClickToMenuItem("Catalog");

            this.ClickToMenuItem("Countries");

            this.ClickToMenuItem("Currencies");

            this.ClickToMenuItem("Customers");

            this.ClickToMenuItem("Geo Zones");

            this.ClickToMenuItem("Languages");

            this.ClickToModulesMenuItem();

            this.ClickToMenuItem("Orders");

            this.ClickToMenuItem("Pages");

            this.ClickToMenuItem("Reports", "Monthly Sales");

            this.ClickToSettingsMenuItem();

            this.ClickToMenuItem("Slides");

            this.ClickToMenuItem("Tax", "Tax Classes");

            this.ClickToTranslationsMenuItem();

            this.ClickToMenuItem("Users");

            this.ClickToMenuItem("vQmods");
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
            element.Click();

            this.wait.Until(ExpectedConditions.TitleIs(nameOfMenu + this.TitlePart));

            var tempItem = this.driver.FindElements(By.TagName("ul"));

            var subElement = tempItem.SingleOrDefault(item => item.GetAttribute("class").Equals("docs"));

            if (subElement != null)
            {
                var allSubMenuItems = subElement.FindElements(By.TagName("li"));

                var tempListWithElements = allSubMenuItems.Select(item => item.GetAttribute("id")).ToList();

                foreach (var id in tempListWithElements)
                {
                    var subMenuItem = this.driver.FindElement(By.Id(id));

                    var titleText = subMenuItem.Text + this.TitlePart;

                    subMenuItem.Click();

                    this.wait.Until(ExpectedConditions.TitleIs(titleText));
                }
            }
        }

        /// <summary>
        /// The click to menu item.
        /// </summary>
        /// <param name="nameOfMenu">
        /// The name of menu.
        /// </param>
        /// <param name="specialTitleName">
        /// The special title name.
        /// </param>
        public void ClickToMenuItem(string nameOfMenu, string specialTitleName)
        {
            var leftMenu = this.driver.FindElement(By.Id("box-apps-menu"));
            var leftMenuItems = leftMenu.FindElements(By.Id("app-"));
            var element = leftMenuItems.SingleOrDefault(item => item.Text.ToLower().Equals(nameOfMenu.ToLower()));

            Assert.IsNotNull(element, "\r\nERROR: Element with name " + nameOfMenu + " was not found. Please check page you're looking at.");

            element.Click();

            this.wait.Until(
                String.IsNullOrEmpty(specialTitleName)
                    ? ExpectedConditions.TitleIs(nameOfMenu + this.TitlePart)
                    : ExpectedConditions.TitleIs(specialTitleName + this.TitlePart));

            var tempItem = this.driver.FindElements(By.TagName("ul"));
            var subElement = tempItem.SingleOrDefault(item => item.GetAttribute("class").Equals("docs"));

            if (subElement != null)
            {
                var allSubMenuItems = subElement.FindElements(By.TagName("li"));
                var tempListWithElements = allSubMenuItems.Select(item => item.GetAttribute("id")).ToList();

                foreach (var id in tempListWithElements)
                {
                    var subMenuItem = this.driver.FindElement(By.Id(id));

                    var titleText = subMenuItem.Text + this.TitlePart;

                    subMenuItem.Click();

                    this.wait.Until(ExpectedConditions.TitleIs(titleText));
                }
            }
        }

        /// <summary>
        /// The click to modules menu item.
        /// </summary>
        public void ClickToModulesMenuItem()
        {
            var nameOfMenu = "Modules";

            var leftMenu = this.driver.FindElement(By.Id("box-apps-menu"));
            var leftMenuItems = leftMenu.FindElements(By.Id("app-"));
            var element = leftMenuItems.SingleOrDefault(item => item.Text.ToLower().Equals(nameOfMenu.ToLower()));

            Assert.IsNotNull(element, "\r\nERROR: Element with name " + nameOfMenu + " was not found. Please check page you're looking at.");

            element.Click();

            this.wait.Until(ExpectedConditions.TitleIs("Job Modules" + this.TitlePart));

            var tempItem = this.driver.FindElements(By.TagName("ul"));
            var subElement = tempItem.SingleOrDefault(item => item.GetAttribute("class").Equals("docs"));

            if (subElement != null)
            {
                var allSubMenuItems = subElement.FindElements(By.TagName("li"));
                var tempListWithElements = allSubMenuItems.Select(item => item.GetAttribute("id")).ToList();

                foreach (var id in tempListWithElements)
                {
                    var subMenuItem = this.driver.FindElement(By.Id(id));

                    var titleText = subMenuItem.Text + " Modules" + this.TitlePart;
                    if (subMenuItem.Text.Equals("Background Jobs"))
                    {
                        titleText = "Job Modules" + this.TitlePart;
                    }
                    
                    subMenuItem.Click();

                    this.wait.Until(ExpectedConditions.TitleIs(titleText));
                }
            }
        }

        /// <summary>
        /// The click to settings menu item.
        /// </summary>
        public void ClickToSettingsMenuItem()
        {
            var nameOfMenu = "Settings";
            var titleText = nameOfMenu + this.TitlePart;

            var leftMenu = this.driver.FindElement(By.Id("box-apps-menu"));
            var leftMenuItems = leftMenu.FindElements(By.Id("app-"));
            var element = leftMenuItems.SingleOrDefault(item => item.Text.ToLower().Equals(nameOfMenu.ToLower()));

            Assert.IsNotNull(element, "\r\nERROR: Element with name " + nameOfMenu + " was not found. Please check page you're looking at.");

            element.Click();

            this.wait.Until(ExpectedConditions.TitleIs(titleText));

            var tempItem = this.driver.FindElements(By.TagName("ul"));
            var subElement = tempItem.SingleOrDefault(item => item.GetAttribute("class").Equals("docs"));

            if (subElement != null)
            {
                var allSubMenuItems = subElement.FindElements(By.TagName("li"));
                var tempListWithElements = allSubMenuItems.Select(item => item.GetAttribute("id")).ToList();

                foreach (var id in tempListWithElements)
                {
                    var subMenuItem = this.driver.FindElement(By.Id(id));

                    subMenuItem.Click();

                    this.wait.Until(ExpectedConditions.TitleIs(titleText));
                }
            }
        }

        /// <summary>
        /// The click to translations menu item.
        /// </summary>
        public void ClickToTranslationsMenuItem()
        {
            var nameOfMenu = "Translations";
            var titleText = "Search Translations" + this.TitlePart;

            var leftMenu = this.driver.FindElement(By.Id("box-apps-menu"));
            var leftMenuItems = leftMenu.FindElements(By.Id("app-"));
            var element = leftMenuItems.SingleOrDefault(item => item.Text.ToLower().Equals(nameOfMenu.ToLower()));

            Assert.IsNotNull(element, "\r\nERROR: Element with name " + nameOfMenu + " was not found. Please check page you're looking at.");

            element.Click();

            this.wait.Until(ExpectedConditions.TitleIs(titleText));

            var tempItem = this.driver.FindElements(By.TagName("ul"));
            var subElement = tempItem.SingleOrDefault(item => item.GetAttribute("class").Equals("docs"));

            if (subElement != null)
            {
                var allSubMenuItems = subElement.FindElements(By.TagName("li"));
                var tempListWithElements = allSubMenuItems.Select(item => item.GetAttribute("id")).ToList();

                foreach (var id in tempListWithElements)
                {
                    var subMenuItem = this.driver.FindElement(By.Id(id));

                    if (subMenuItem.Text.Equals("Scan Files"))
                    {
                        titleText = "Scan Files For Translations" + this.TitlePart;
                    }

                    subMenuItem.Click();

                    this.wait.Until(ExpectedConditions.TitleIs(titleText));
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
