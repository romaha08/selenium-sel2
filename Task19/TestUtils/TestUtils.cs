
namespace CSharpSeleniumExample.Task19.TestUtils
{
    using System;
    using System.Linq;
    using CSharpSeleniumExample.Task19.Pages;
    using OpenQA.Selenium;
    using OpenQA.Selenium.IE;
    using OpenQA.Selenium.Support.UI;

    /// <summary>
    /// The test utils.
    /// </summary>
    public class TestUtils
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
        /// The bin page.
        /// </summary>
        private readonly LiteCartBinPage binPage;

        /// <summary>
        /// The main page.
        /// </summary>
        private readonly LiteCartMainPage mainPage;

        public TestUtils()
        {
            this.InitializeDriver();

            this.binPage = new LiteCartBinPage(this.driver);
            this.mainPage = new LiteCartMainPage(this.driver);
        }

        public void InitializeDriver()
        {
            this.driver = new InternetExplorerDriver();
            this.driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(15000));
            this.wait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(15));
        }

        public void StopDriver()
        {
            this.driver.Quit();
            this.driver = null;
        }

        public void LoginToLiteCart()
        {
            this.mainPage.Open();

            // Check Login was done
            this.wait.Until(ExpectedConditions.TitleIs("Online Store | My Store"));
        }

        public void AddAllItemsToBin()
        {
            for (int i = 0; i < 3; i++)
            {
                this.mainPage.OpenAnyProductDetails();

                this.binPage.AddItemToBin(i);

                var allbreadCrumbs = this.driver.FindElements(By.CssSelector("nav[id=breadcrumbs] ul li a"));

                var home = allbreadCrumbs.FirstOrDefault(item => item.Text.Equals("Home"));

                home?.Click();
            }
        }

        public void ClearBin()
        {
            this.binPage.ClearBin();
        }

        public void WaitForElementDisapear(IWebElement element)
        {
            this.wait.Until(ExpectedConditions.StalenessOf(element));
        }
    }
}
