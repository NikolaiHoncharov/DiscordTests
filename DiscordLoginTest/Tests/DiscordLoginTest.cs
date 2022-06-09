using System.Collections;
using System.IO;
using System.Linq;
using DiscordTests.PageObject.Pages;
using NUnit.Framework;
using OpenQA.Selenium;

namespace DiscordTests.Tests
{
    public class DiscordLoginTest
    {
        IWebDriver _driver;

        private static IEnumerable GetTestDataFromTxt()
        {
            var testdata = File
                .ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, "DataDriven", "users.txt"))
                .Select(line => line.Split(';').ToArray());
            foreach (var row in testdata)
            {
                yield return new TestCaseData(row);
            }
        }

        [SetUp]
        public void Setup()
        {
            _driver = new OpenQA.Selenium.Chrome.ChromeDriver();
            _driver.Manage().Window.Maximize();
        }

        [Test, TestCaseSource(nameof(GetTestDataFromTxt))]
        public void LoginPositiveTest(string testuserEmail, string testuserPassword, string expectedUserNameLabel)
        {
            var loginPage = new LoginPage(_driver);
            loginPage.GoToPage();
            loginPage.TypeEmail(testuserEmail);
            loginPage.TypePassword(testuserPassword);
            var mainPage = loginPage.Submit();
            Assert.AreEqual(expectedUserNameLabel, mainPage.GetUserNameLabel(), "User name does not correspond to the expected one");
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Close();
        }
    }
}