//using System;
//using System.Text;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.Data;

//namespace MvcApplication1.Tests.DataContext
//{
//    /// <summary>
//    /// Summary description for DataContextTest
//    /// </summary>
//    [TestClass]
//    public class DataContextTest
//    {
//        private static Extratistico.Classes.DataContext.DataContext dataContext;

//        public DataContextTest()
//        {
//            //
//            // TODO: Add constructor logic here
//            //
//        }

//        [ClassInitialize()]
//        public static void  DataContextTestInitializer(TestContext testContext){
//            dataContext = new Extratistico.Classes.DataContext.DataContext();
//        }

//        [ClassCleanup()]
//        public static void DataContextTestCleanup()
//        {
//            dataContext.Dispose();
//        }

//        private TestContext testContextInstance;

//        /// <summary>
//        ///Gets or sets the test context which provides
//        ///information about and functionality for the current test run.
//        ///</summary>
//        public TestContext TestContext
//        {
//            get
//            {
//                return testContextInstance;
//            }
//            set
//            {
//                testContextInstance = value;
//            }
//        }

//        #region Additional test attributes
//        //
//        // You can use the following additional attributes as you write your tests:
//        //
//        // Use ClassInitialize to run code before running the first test in the class
//        // [ClassInitialize()]
//        // public static void MyClassInitialize(TestContext testContext) { }
//        //
//        // Use ClassCleanup to run code after all tests in a class have run
//        // [ClassCleanup()]
//        // public static void MyClassCleanup() { }
//        //
//        // Use TestInitialize to run code before running each test 
//        // [TestInitialize()]
//        // public void MyTestInitialize() { }
//        //
//        // Use TestCleanup to run code after each test has run
//        // [TestCleanup()]
//        // public void MyTestCleanup() { }
//        //
//        #endregion

//        [TestInitialize()]
//        public void TransactionRollBackTestInitialize()
//        {
//            dataContext.ExecuteNonQuery("create table teste(t int)");
//            dataContext.Commit();
//        }

//        [TestMethod]
//        public void TransactionRollBackTest()
//        {
//            dataContext.ExecuteNonQuery("insert into teste(t) values(1)");
//            dataContext.Commit();
//            DataRow dr = dataContext.SelectSingle("select *from teste");
//            Assert.AreEqual(dr, null, "O campo foi inserido e mesmo com rollback ele permaneceu na tabela.");
//        }

//        [TestCleanup()]
//        public void TransactionRollBackTestCleanup()
//        {
//            dataContext.ExecuteNonQuery("drop table teste");
//            dataContext.Commit();
//        }
//    }
//}
