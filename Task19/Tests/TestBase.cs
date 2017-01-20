
namespace CSharpSeleniumExample.Task19.Tests
{
    using CSharpSeleniumExample.Task19.TestUtils;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The test base.
    /// </summary>
    public class TestBase
    {
        /// <summary>
        /// The app.
        /// </summary>
        public TestUtils app;

        /// <summary>
        /// The start.
        /// </summary>
        [TestInitialize] 
         public void Start()
         { 
             this.app = new TestUtils(); 
         }

        /// <summary>
        /// The stop.
        /// </summary>
        [TestCleanup] 
         public void Stop()
         { 
             this.app.StopDriver(); 
             this.app = null; 
         } 
     } 
 } 
