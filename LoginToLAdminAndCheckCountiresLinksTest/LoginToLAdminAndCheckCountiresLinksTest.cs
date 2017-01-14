
namespace CSharpSeleniumExample
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OpenQA.Selenium;
    using OpenQA.Selenium.IE;
    using OpenQA.Selenium.Support.UI;

    /// <summary>
    /// The login to lite cart test.
    /// </summary>
    [TestClass]
    public class LoginToLAdminAndCheckCountiresLinks
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
        public void Login_To_Admin_And_Check_Countries_Links_Test()
        {
            this.LoginToLiteCart();

            this.ClickToMenuItem("Countries");

            this.OpenLinkAndBack();
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
        public void OpenLinkAndBack()
        {
            var tableForm = this.driver.FindElements(By.CssSelector("form[name=countries_form]"));

            Assert.IsTrue(tableForm.Count.Equals(1), "\r\nERROR: Count of Elements by search criteria 'form' is more than 1. Please check page.");

            var allCountries = tableForm.First().FindElements(By.CssSelector("tr[class=row]"));
            var lineElements = allCountries[2].FindElements(By.TagName("td"))[4];

            var linkToClick = lineElements.FindElement(By.TagName("a"));

            linkToClick.Click();

            var links =
                this.driver.FindElements(By.CssSelector("table tbody tr td"));

            var link = links[0];
            foreach (var webElement in links)
            {
                if (webElement.Text.Equals("Code (ISO 3166-1 alpha-2)"))
                {
                    var i1 = webElement.FindElement(By.TagName("a"));
                    link = i1;
                    break;
                }
            }
            
            var mainWindow = this.driver.CurrentWindowHandle;
            var oldWindows = this.driver.WindowHandles;

            link.Click();

            var windowNeeded = this.AnyWindowOtherThan(oldWindows[0]);
            this.driver.SwitchTo().Window(windowNeeded);
            this.driver.Close();

            this.driver.SwitchTo().Window(mainWindow);
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
        /// The any window other than.
        /// </summary>
        /// <param name="oldWindows">
        /// The old Windows.
        /// </param>
        /// <returns>
        /// The <see cref="ExpectedConditions"/>.
        /// </returns>
        public string AnyWindowOtherThan(string oldWindows)
        {
            var result = string.Empty;
            for (int i = 0; i < 1000; i++)
            {
                ReadOnlyCollection<string> windowHandles = this.driver.WindowHandles;

                if (windowHandles.Count > 1)
                {
                    foreach (var window in windowHandles)
                    {
                        if (!window.Equals(oldWindows))
                        {
                            return window;
                        }
                    }
                }
            }

            return result;
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
