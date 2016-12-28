// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateNewAccountInLiteCartTest.cs" company="">
//   
// </copyright>
// <summary>
//   The unit test 1.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CSharpSeleniumExample
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OpenQA.Selenium;
    using OpenQA.Selenium.IE;
    using OpenQA.Selenium.Support.Extensions;
    using OpenQA.Selenium.Support.UI;

    /// <summary>
    /// The unit test 1.
    /// </summary>
    [TestClass]
    public class CreateNewAccountInLiteCartTest
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
        /// The link to lite cart.
        /// </summary>
        private readonly string LinkToLiteCart = "http://localhost:81/litecart";

        private static readonly string UserName = "qa" + Extensions.CreateRandomString(5) + Extensions.CreateRandomInt(3);
        private readonly string  UniqeEmail = UserName + "@qa.test";
        private readonly string Password = "qqq111!";

        /// <summary>
        /// The start.
        /// </summary>
        [TestInitialize]
        public void Start()
        {
            this.driver = new InternetExplorerDriver();
            this.wait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(10));

            this.OpenLiteCart();
        }

        /// <summary>
        /// The create_ new_ account_ for_ lite cart_ test.
        /// </summary>
        [TestMethod]
        public void Create_New_Account_For_LiteCart_Test()
        {
            var allLinks = this.driver.FindElements(By.TagName("a"));

            var createAccountLink =
                allLinks.SingleOrDefault(item => item.Text.Equals("New customers click here"));

            if (createAccountLink == null)
            {
                throw new Exception("\r\nERROR: Link 'Create new Customer' not found.");
            }

            createAccountLink.Click();

            // Check Page was loaded
            this.wait.Until(ExpectedConditions.TitleIs("Create Account | My Store"));

            var formWithInfo = this.driver.FindElement(By.CssSelector("form[name=customer_form]"));

            // Input All Account Info
            this.EnterNewAccountInfo(formWithInfo);

            var buttonCreate = formWithInfo.FindElements(By.CssSelector("button[name=create_account]"))[0];
            buttonCreate.Click();

            if (this.IsThereAnyExceptionInEmailAddress())
            {
                formWithInfo = this.driver.FindElement(By.CssSelector("form[name=customer_form]"));

                // Input All Account Info
                this.EnterNewAccountInfo(formWithInfo);

                buttonCreate = formWithInfo.FindElements(By.CssSelector("button[name=create_account]"))[0];
                buttonCreate.Click();
            }

            this.wait.Until(ExpectedConditions.TitleIs("Online Store | My Store"));

            var accountBlock = this.driver.FindElement(By.Id("box-account"));

            var allList = accountBlock.FindElements(By.TagName("li"));

            var logoutLinkElement = allList.SingleOrDefault(item => item.Text.ToLower().Equals("logout"));
            if (logoutLinkElement != null)
            {
                var logoutLink = logoutLinkElement.FindElement(By.TagName("a"));

                logoutLink?.Click();

                this.wait.Until(ExpectedConditions.TitleIs("Online Store | My Store"));

                var allTextElements = this.driver.FindElements(By.CssSelector("input[type=text]"));
                var emailField = allTextElements.SingleOrDefault(item => item.GetAttribute("name").Equals("email"));
                emailField?.SendKeys(this.UniqeEmail);

                allTextElements = this.driver.FindElements(By.CssSelector("input[name=password]"));
                var passwordField = allTextElements.SingleOrDefault(item => item.GetAttribute("name").Equals("password"));
                passwordField?.SendKeys(this.Password);

                var button =
                    this.driver
                        .FindElements(By.CssSelector("button[type=submit]"))
                        .SingleOrDefault(item => item.GetAttribute("name").Equals("login"));

                button?.Click();

                accountBlock = this.driver.FindElement(By.Id("box-account"));

                allList = accountBlock.FindElements(By.TagName("li"));

                logoutLinkElement = allList.SingleOrDefault(item => item.Text.ToLower().Equals("logout"));

                if (logoutLinkElement != null)
                {
                    logoutLink = logoutLinkElement.FindElement(By.TagName("a"));
                }

                logoutLink?.Click();
            }
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
        /// The enter new account info.
        /// </summary>
        /// <param name="form">
        /// The form.
        /// </param>
        public void EnterNewAccountInfo(IWebElement form)
        {
            var allFields = form.FindElements(By.TagName("td"));

            foreach (var field in allFields)
            {
                var reqieremnt = field.FindElements(By.CssSelector("span[class=required]"));
                var input = field.FindElements(By.CssSelector("span input"));
                if (reqieremnt.Count > 0 || input.Count > 0)
                {
                   this.FillFieldByType(field);
                }
            }
        }

        /// <summary>
        /// The is there any exception.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsThereAnyExceptionInEmailAddress()
        {
            var exception = this.driver.FindElements(By.Id("notices"));

            if (exception.Count > 0)
            {
                var exceptionText = exception[0].FindElement(By.CssSelector("div i"));
                if (exceptionText.Text.Equals("The email address already exists in our customer database. Please login or select a different email address."))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// The fill field by type.
        /// </summary>
        /// <param name="fieldElement">
        /// The field Element.
        /// </param>
        public void FillFieldByType(IWebElement fieldElement)
        {
            var tempField = fieldElement.FindElements(By.TagName("input"));

            if (tempField.Count > 0)
            {
                var field = tempField.First();
                var typeOfField = field.GetAttribute("name");

                switch (typeOfField)
                {
                    case "firstname":
                        field.SendKeys("FirstName");
                        break;

                    case "lastname":
                        field.SendKeys("LastName");
                        break;

                    case "address1":
                        field.SendKeys("Address 1");
                        break;

                    case "postcode":
                        field.SendKeys("122321");
                        break;

                    case "city":
                        field.SendKeys("St.Petersburg");
                        break;

                    case "email":
                        this.EnterUniquEmail(field);
                        break;

                    case "phone":
                        field.SendKeys("12345678");
                        break;

                    case "password":
                        field.SendKeys(this.Password);
                        break;

                    case "confirmed_password":
                        field.SendKeys(this.Password);
                        break;
                }
            }

            var tempFieldSelect = fieldElement.FindElements(By.TagName("select"));

            if (tempFieldSelect.Count > 0)
            {
                var typeOfField = tempFieldSelect.First().GetAttribute("name");
                switch (typeOfField)
                {
                    case "country_code":
                        this.ChooseAnyCountry(fieldElement, false);
                        break;
                }
            }
        }

        /// <summary>
        /// The enter uniqu email.
        /// </summary>
        /// <param name="emailField">
        /// The email Field.
        /// </param>
        public void EnterUniquEmail(IWebElement emailField)
        {
            Assert.IsNotNull(emailField);

            emailField.Clear();

            emailField.SendKeys(this.UniqeEmail);
        }

        /// <summary>
        /// The choose any country.
        /// </summary>
        /// <param name="elementForm">
        /// The element form.
        /// </param>
        /// <param name="doWeNeedToChooseRandomCountry">
        /// The do We Need To Choose Random Country.
        /// </param>
        public void ChooseAnyCountry(IWebElement elementForm, bool doWeNeedToChooseRandomCountry)
        {
            if (doWeNeedToChooseRandomCountry)
            {
                var elementToExecute = elementForm.FindElement(By.CssSelector("span"));

                this.driver.ExecuteJavaScript("arguments[0].selectedIndex = 3", elementToExecute);
                this.driver.ExecuteJavaScript("arguments[0].style.opacity = 1", elementToExecute);
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

    public static class Extensions
    {
        private static readonly Random random = new Random((int)DateTime.Now.Ticks);

        public static string CreateRandomString(int stringLength)
        {

            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < stringLength; i++)
            {
                ch = Convert.ToChar(random.Next(0x41, 0x5A));
                builder.Append(ch);
            }
            return builder.ToString().ToLower();
        }
        public static string CreateRandomInt(int intLength)
        {

            StringBuilder builder = new StringBuilder();
            int ch;
            for (int i = 0; i < intLength; i++)
            {
                ch = Convert.ToInt32(random.Next(0, 9));
                builder.Append(ch);
            }
            return builder.ToString().ToLower();
        }
    }
}
