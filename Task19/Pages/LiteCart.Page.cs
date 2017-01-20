// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LiteCart.Page.cs" company="">
//   
// </copyright>
// <summary>
//   The page.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CSharpSeleniumExample.Task19.Pages
{
    using System;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    /// <summary>
    /// The page.
    /// </summary>
    internal class Page
    {
        /// <summary>
        /// The driver.
        /// </summary>
        protected IWebDriver driver;

        /// <summary>
        /// The wait.
        /// </summary>
        protected WebDriverWait wait;

        /// <summary>
        /// Initializes a new instance of the <see cref="Page"/> class.
        /// </summary>
        /// <param name="driver">
        /// The driver.
        /// </param>
        public Page(IWebDriver driver)
         { 
             this.driver = driver; 
             this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10)); 
         } 
     } 
 } 
