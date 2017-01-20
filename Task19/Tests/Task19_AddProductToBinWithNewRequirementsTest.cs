// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoginToLiteCartAndCheckCountiresTest.cs" company="">
//   
// </copyright>
// <summary>
//   The login to lite cart test.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CSharpSeleniumExample.Task19.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The login to lite cart test.
    /// </summary>
    [TestClass]
    public class AddProductToBinWithNewRequirements : TestBase
    {
        /// <summary>
        /// The login to lite cart and click to all menu items.
        /// </summary>
        [TestMethod]
        public void Task_19_Add_Products_To_Bin_Test()
        {
            this.app.LoginToLiteCart();

            this.app.AddAllItemsToBin();

            this.app.ClearBin();
        }
    }

}
