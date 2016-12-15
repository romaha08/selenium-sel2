// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoginToLiteCartTest.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the LoginToLiteCartTest type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CSharpSeleniumExample
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OpenQA.Selenium;
    using OpenQA.Selenium.IE;
    using OpenQA.Selenium.Support.UI;

    /// <summary>
    /// The login to lite cart test.
    /// </summary>
    [TestClass]
    public class LoginToLiteCartTest
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
        /// The login to lite cart.
        /// </summary>
        [TestMethod]
        public void LoginToLiteCart()
        {
            this.driver.Url = this.LinkToLiteCart;

            // Check page was loaded correctly(Only form checking)
            this.CheckLoginFormLoaded();
            
            // Login
            this.CheckUserNameFieldExists().SendKeys(this.Login);

            // Password
            this.CheckPasswordFieldExists().SendKeys(this.Password);

            // Click Button Login
            this.CheckButtonLoginExists().Click();

            // Check Login was done
            this.wait.Until(ExpectedConditions.TitleIs("My Store"));
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
