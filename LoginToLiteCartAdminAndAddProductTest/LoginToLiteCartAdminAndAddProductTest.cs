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
    using System.Text;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OpenQA.Selenium;
    using OpenQA.Selenium.IE;
    using OpenQA.Selenium.Support.Extensions;
    using OpenQA.Selenium.Support.UI;

    /// <summary>
    /// The login to lite cart test.
    /// </summary>
    [TestClass]
    public class LoginToLiteCartAdminAndAddProduct
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
        public void Login_To_LiteCart_Admin_And_Add_Product_Test()
        {
            this.LoginToLiteCart();

            this.ClickToMenuItem("Catalog");

            this.CreateNewProduct();
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
        public void CreateNewProduct()
        {
            var allButtons = this.driver.FindElements(By.CssSelector("a[class=button]"));

            var button = allButtons.SingleOrDefault(item => item.Text.Equals("Add New Product"));

            button?.Click();

            this.wait.Until(ExpectedConditions.TitleIs("Add New Product | My Store"));

            this.FillGeneralTabInfo();

            this.FillInformationTabInfo();

            this.FillPriceTabInfo();

            // Save
            var button2 = this.driver.FindElements(By.CssSelector("input[type=submit]"));
            var buttonSave = button2.SingleOrDefault(item => item.Text.Equals("Save"));
            buttonSave?.Click();
        }

        public void FillGeneralTabInfo()
        {
            var NameOfProduct = "TestProduct" + Extensions.CreateRandomInt(5);

            var radiEnEl = this.driver.FindElements(By.CssSelector("label"));
            foreach (var element in radiEnEl)
            {
                var enableElement = element.FindElement(By.CssSelector("input[type=radio]"));
                if (element.Text.Equals("Enabled"))
                {
                    enableElement?.Click();
                }
            }
           
            // Name
            var textsInput = this.driver.FindElements(By.CssSelector("input[type=text]"));
            var nameField = textsInput.SingleOrDefault(item => item.GetAttribute("name").Equals("name[en]"));

            nameField?.SendKeys(NameOfProduct);

            // Code
            var codeField = textsInput.SingleOrDefault(item => item.GetAttribute("name").Equals("code"));
            codeField?.SendKeys("123456");

            // Product Category
            var checkBoxInput = this.driver.FindElements(By.CssSelector("input[type=checkbox]"));
            foreach (var element in checkBoxInput)
            {
                var rubberField = element.GetAttribute("name");
                var rubberField2 = element.GetAttribute("data-name"); 
                if (rubberField.Equals("categories[]") && rubberField2.Equals("Rubber Ducks"))
                {
                    element.Click();
                }
            }

            // Product Group
            foreach (var element in checkBoxInput)
            {
                var rubberField = element.GetAttribute("name");
                var rubberField2 = element.GetAttribute("value");
                if (rubberField.Equals("product_groups[]") && rubberField2.Equals("1-1"))
                {
                    element.Click();
                }
            }

            // Quantity
            var numberInput = this.driver.FindElements(By.CssSelector("input[type=number]"));
            var quantityField = numberInput.SingleOrDefault(item => item.GetAttribute("name").Equals("quantity"));

            quantityField?.Clear();
            quantityField?.SendKeys("1");

            this.SetDatepicker("input[name=date_valid_from]", "02/20/2016");
            this.SetDatepicker("input[name=date_valid_to]", "03/20/2016");
        }
        public void FillInformationTabInfo()
        {
            var tabForm = this.driver.FindElement(By.CssSelector("div[class=tabs]"));
            var allTabs = tabForm.FindElements(By.CssSelector("li"));

            var infoTab = allTabs.SingleOrDefault(item => item.Text.Equals("Information"));
            infoTab?.Click();

            var select1 = this.driver.FindElement(By.CssSelector("select[name=manufacturer_id]"));
            this.driver.ExecuteJavaScript("arguments[0].selectedIndex = 1", select1);
            this.driver.ExecuteJavaScript("arguments[0].style.opacity = 1", select1);

            var select2 = this.driver.FindElement(By.CssSelector("select[name=supplier_id]"));
            this.driver.ExecuteJavaScript("arguments[0].selectedIndex = 1", select2);
            this.driver.ExecuteJavaScript("arguments[0].style.opacity = 1", select2);

            var textTypes = this.driver.FindElements(By.CssSelector("input[type=text]"));

            var keyW = textTypes.SingleOrDefault(item => item.GetAttribute("name").Equals("keywords"));
            keyW?.SendKeys("Product1");

            var keySd = textTypes.SingleOrDefault(item => item.GetAttribute("name").Equals("short_description[en]"));
            keySd?.SendKeys("P1 Description");

            var textInputDesc = this.driver.FindElement(By.CssSelector("div[class=trumbowyg-editor]"));
            textInputDesc?.SendKeys("Test Product");

            var keyT = textTypes.SingleOrDefault(item => item.GetAttribute("name").Equals("head_title[en]"));
            keyT?.SendKeys("P1Title");

            var keyM = textTypes.SingleOrDefault(item => item.GetAttribute("name").Equals("meta_description[en]"));
            keyM?.SendKeys("P1Title");
        }
        public void FillPriceTabInfo()
        {
            var tabForm = this.driver.FindElement(By.CssSelector("div[class=tabs]"));
            var allTabs = tabForm.FindElements(By.CssSelector("li"));

            var infoTab = allTabs.SingleOrDefault(item => item.Text.Equals("Prices"));
            infoTab?.Click();

            var numberInput = this.driver.FindElements(By.CssSelector("input[type=number]"));
            var price = numberInput.SingleOrDefault(item => item.GetAttribute("name").Equals("purchase_price"));
            price?.Clear();
            price?.SendKeys("5");

            var selectDollar = this.driver.FindElement(By.CssSelector("select[name=purchase_price_currency_code]"));
            this.driver.ExecuteJavaScript("arguments[0].selectedIndex = 1", selectDollar);
            this.driver.ExecuteJavaScript("arguments[0].style.opacity = 1", selectDollar);

            var price2 = numberInput.SingleOrDefault(item => item.GetAttribute("name").Equals("gross_prices[USD]"));
            price2?.Clear();
            price2?.SendKeys("1");
        }
        
        public void SetDatepicker(string cssSelector, string date)
        {
            this.driver.FindElement(By.CssSelector(cssSelector)).SendKeys(date);
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
