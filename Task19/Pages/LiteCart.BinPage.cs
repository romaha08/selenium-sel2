namespace CSharpSeleniumExample.Task19.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.Extensions;
    using OpenQA.Selenium.Support.UI;

    internal class LiteCartBinPage : Page
    {
        public LiteCartBinPage(IWebDriver driver)
            : base(driver)
        {
        }

        /// <summary>
        /// The add item to bin.
        /// </summary>
        internal void AddItemToBin(int countOfItemsAddedToCart)
        {
            if (this.isElementPresent(By.CssSelector("td[class=options]")))
            {
                var sizeElement = this.driver.FindElement(By.CssSelector("td[class=options]"));
                var dropDown = sizeElement.FindElement(By.TagName("select"));

                this.SelectItemInDropDownList(dropDown, 0, 2);
            }

            var cart = this.driver.FindElement(By.Id("cart"));
            var quantityOld = cart.FindElement(By.CssSelector("a span[class=quantity]"));

            var button = this.driver.FindElement(By.CssSelector("button[name=add_cart_product]"));

            button.Click();

            var quantity = cart.FindElement(By.CssSelector("a span[class=quantity]"));

            if (this.isElementPresent(By.CssSelector("a span[class=quantity]")) && Int32.Parse(quantity.Text) > Int32.Parse(quantityOld.Text))
            {
                quantity = cart.FindElement(By.CssSelector("a span[class=quantity]"));

                Assert.AreEqual(countOfItemsAddedToCart.ToString(), quantity.Text);
            }
        }

        internal void RemoveItemFromBin(string text)
        {
            var allItemsToRemove = this.driver.FindElements(By.CssSelector("ul[class=shortcuts] li"));
            foreach (var webElement in allItemsToRemove)
            {
                var toChoose = webElement.FindElements(By.CssSelector("a[href=#]"));
                var firstOrDefault = toChoose?.FirstOrDefault();
                firstOrDefault?.Click();

                var textOfProduct = this.driver.FindElement(By.CssSelector("div p a"));
                if (textOfProduct.Text.Equals(text))
                {
                    var button = this.driver.FindElement(By.CssSelector("button[name=remove_cart_item]"));

                    button.Click();
                }
            }
        }

        internal void ClearBin()
        {
            var cart = this.driver.FindElement(By.Id("cart"));

            cart.Click();

            var list = new List<IWebElement>();
            var itemsTable = this.driver.FindElement(By.Id("order_confirmation-wrapper"));
            var allItemsInTable = itemsTable.FindElements(By.TagName("tr"));
            foreach (var webElement in allItemsInTable)
            {
                if (webElement.GetAttribute("class") != "header")
                {
                    var allAttributesOfItem = webElement.FindElements(By.CssSelector("td[class=item]"));
                    if (allAttributesOfItem.Count > 0)
                    {
                        list.Add(webElement);
                    }
                }
            }

            foreach (var element in list)
            {
                var alementOfProduct = element.FindElements(By.CssSelector("td"));
                var firstOrDefault = alementOfProduct.FirstOrDefault(item => item.GetAttribute("Class") != null);
                if (firstOrDefault != null && firstOrDefault.GetAttribute("Class").Equals("item"))
                {
                    var text = firstOrDefault.Text;

                    this.RemoveItemFromBin(text);

                    // Check
                    this.driver.Navigate().Refresh();

                    this.WaitForElementDisapear(element);
                }
            }
        }

        /// <summary>
        /// The select item in drop down list.
        /// </summary>
        /// <param name="element">
        /// The element.
        /// </param>
        /// <param name="argumentIndex">
        /// The argument index.
        /// </param>
        /// <param name="selectedItemIndex">
        /// The selected item index.
        /// </param>
        internal void SelectItemInDropDownList(IWebElement element, int argumentIndex, int selectedItemIndex)
        {
            var scriptSelect = String.Format("arguments[{0}].selectedIndex = {1}", argumentIndex, selectedItemIndex);
            var scriptShow = String.Format("arguments[{0}].style.opacity = {1}", argumentIndex, selectedItemIndex);

            this.driver.ExecuteJavaScript(scriptSelect, element);
            this.driver.ExecuteJavaScript(scriptShow, element);
        }

        public bool isElementPresent(By locator)
        {
            try
            {
                this.driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0));
                return this.driver.FindElements(locator).Count > 0;
            }
            finally
            {
                this.driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(10000));
            }
        }

        public void WaitForElementDisapear(IWebElement element)
        {
            this.wait.Until(ExpectedConditions.StalenessOf(element));
        }
    }
}
