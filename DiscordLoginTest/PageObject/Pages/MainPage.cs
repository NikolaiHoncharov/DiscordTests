using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;

namespace DiscordTests.PageObject.Pages
{
    public class MainPage
    {

        private readonly IWebDriver _driver;
        private readonly TimeSpan _widgetTimeout = new(0, 0, 15);

        public MainPage(IWebDriver driver)
        {
            this._driver = driver;
            PageFactory.InitElements(driver, this);
        }
 
        [FindsBy(How = How.XPath, Using ="//div[contains(@class,'usernameContainer-')]/div")]
        private IWebElement _userNameLabel;

        public string GetUserNameLabel()
        {
            var waitForElementToAppear = new WebDriverWait(_driver, _widgetTimeout);
            waitForElementToAppear.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(_userNameLabel));
            return _userNameLabel.Text;
        }
    }
}