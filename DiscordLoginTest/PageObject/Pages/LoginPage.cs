using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;

namespace DiscordTests.PageObject.Pages
{
    public class LoginPage
    {
        readonly string _discordComLoginUrl = "https://discord.com/login";
 
        private readonly IWebDriver _driver;
        private readonly TimeSpan _widgetTimeout = new(0, 0, 15);
        private readonly TimeSpan _pageTimeout = new(0, 0, 30);
        private readonly TimeSpan _manualTimeout = new(0, 20, 0);

        public LoginPage(IWebDriver driver)
        {
            this._driver = driver;
            PageFactory.InitElements(driver, this);
        }
 
        [FindsBy(How = How.Name, Using = "email")]
        [CacheLookup]
        private IWebElement _emailInput;
 
        [FindsBy(How = How.Name, Using = "password")]
        [CacheLookup]
        private IWebElement _passwordInput;
 
        [FindsBy(How = How.TagName, Using = "button")]
        [CacheLookup]
        private IWebElement _loginButton;

        private readonly By _captchawidget = By.XPath("//iframe[@data-hcaptcha-widget-id]");

        public void GoToPage()
        {
            _driver.Navigate().GoToUrl(_discordComLoginUrl);
        }

        public void TypeEmail(string email)
        {
            _emailInput.SendKeys(email);
        }

        public void TypePassword(string password)
        {
            _passwordInput.SendKeys(password);
        }

        public MainPage Submit()
        {
            _passwordInput.SendKeys(Keys.Enter);
            WaitCaptchaResolved();
            WaitPageCompletion();
            return new MainPage(_driver);
        }

        private void WaitPageCompletion()
        {
            var wait = new WebDriverWait(_driver, _pageTimeout);
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
        }

        private void WaitCaptchaResolved()
        {
            var waitForFrameToExist = new WebDriverWait(_driver, _widgetTimeout);
            waitForFrameToExist.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(
                    _captchawidget
                )
            );

            var waitForFrameNotToExist = new WebDriverWait(_driver, _manualTimeout);
            waitForFrameNotToExist.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(
                    _captchawidget
                )
            );
        }

    }
}