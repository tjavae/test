using Ncua.Testing.Utilities;
using OpenQA.Selenium;
using System;

namespace Ncua.Testing.UI.Cudc.PageObjects
{
    public class CudcconfirmationPage : BasePage
    {
        const string PAGEPATH = "";
        public CudcconfirmationPage(WebDriverWrapper driver, string testName) : base(driver, PAGEPATH, testName)
        {
        }

        public bool VerifySubmitted(string textToFind)
        {
            try
            {
                var locator = By.XPath($"//*[contains(text(),'{textToFind}')]");
                testDriver.WaitUntilDisplayed(locator);
                return true;
            }
            catch (Exception e)
            {
                Globals.CatchAndWrite(e, testDriver, TestName);
                testDriver.LogError($"Unable to find comment text: ${e}");
                return false;
            }
        }
    }
}
