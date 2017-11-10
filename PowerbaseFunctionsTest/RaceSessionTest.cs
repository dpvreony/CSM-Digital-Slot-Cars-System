using Microsoft.VisualStudio.TestTools.UnitTesting;
using RaceDirectorClientGUI.Models;
using RaceDirectorClientGUI.Models.Comms;
using RaceDirectorClientGUI.Models.Racing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerbaseFunctionsTest
{
    /// <summary>
    /// Summary description for RaceSessionTest
    /// </summary>
    [TestClass]

    class RaceSessionTest
    {

        List<Player> players;
        RaceSession raceSession;
        Powerbase powerbase = new Powerbase();

        public RaceSessionTest()
        {
            List<Player> players = new List<Player> { new Player() };
            RaceTypeBase raceType = new FreePlayRace(5, new TimeSpan(0, 2, 0), true);
            RaceSession raceSession = new RaceSession(1, raceType, players, false);

            powerbase.Run(raceSession);
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void CalculateLEDStatusLightsStaticTest()
        {
            int ledStatus = 0;
            int ok1 = 128 + 1;
            int ok2 = 128 + 2 + 1;
            int ok3 = 128 + 4 + 2 + 1;
            int ok4 = 128 + 8 + 4 + 2 + 1;
            int ok5 = 128 + 16 + 8 + 4 + 2 + 1;
            int ok6 = 128 + 32 + 16 + 8 + 4 + 2 + 1;

            ledStatus = powerbase.CalculateLEDStatusLightsStatic(1);
            Assert.AreEqual(ok1, ledStatus);
            ledStatus = powerbase.CalculateLEDStatusLightsStatic(2);
            Assert.AreEqual(ok2, ledStatus);
            ledStatus = powerbase.CalculateLEDStatusLightsStatic(3);
            Assert.AreEqual(ok3, ledStatus);
            ledStatus = powerbase.CalculateLEDStatusLightsStatic(4);
            Assert.AreEqual(ok4, ledStatus);
            ledStatus = powerbase.CalculateLEDStatusLightsStatic(5);
            Assert.AreEqual(ok5, ledStatus);
            ledStatus = powerbase.CalculateLEDStatusLightsStatic(6);
            Assert.AreEqual(ok6, ledStatus);
        }
    }
}
