namespace CSharpSeleniumExample
{
    using System;
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
    public class LoginToLiteCartAdminAndCheckBrowserLogs
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
        public void Login_To_LiteCart_Admin_And_Go_To_All_Products_And_Check_Browser_Log_Test()
        {
            this.LoginToLiteCart();

            this.ClickToMenuItem("Catalog");

            this.GoToAllProductsAndCheckBrowserLog();
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
        public void GoToAllProductsAndCheckBrowserLog()
        {
            var allElements = this.driver.FindElements(By.CssSelector(" tr[class=row] td a"));

            foreach (var element in allElements)
            {
                if (element.Text.Equals("Rubber Ducks"))
                {
                    element.Click();
                    break;
                }
            }


            var allProducts = this.driver.FindElements(By.CssSelector("tr[class=row]"));
            foreach (var product in allProducts)
            {
                var link = product.FindElements(By.TagName("a"));
                var duckText = link.SingleOrDefault(item => item.Text.ToLower().Contains("duck") && !item.Text.Equals("Rubber Ducks"));
                if (duckText != null)
                {
                    duckText.Click();

                    this.CheckBrowserLogForExceptions();

                    this.ClickCancelButton();

                    allProducts = this.driver.FindElements(By.CssSelector("tr[class=row]"));
                }
            }
        }

        public void ClickCancelButton()
        {
            var cancelButton = this.driver.FindElement(By.CssSelector("button[name=cancel]"));

            cancelButton?.Click();
        }

        public void CheckBrowserLogForExceptions()
        {
            var log = this.driver.Manage().Logs;
            var allLogs = log.GetLog("browser");

            if (allLogs != null)
            {
                foreach (LogEntry eventLog in allLogs)
                {
                    if (eventLog.Message.ToLower().Contains("exception") || eventLog.Message.ToLower().Contains("error"))
                    {
                        Console.WriteLine("\r\nERROR: " + eventLog);
                    }
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
