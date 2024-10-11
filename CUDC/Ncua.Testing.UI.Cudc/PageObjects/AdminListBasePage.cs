using Ncua.Testing.Utilities;
using OpenQA.Selenium;
using System;

namespace Ncua.Testing.UI.Cudc.PageObjects
{
    public abstract class AdminListBasePage : BasePage
    {
        public AdminListBasePage(WebDriverWrapper driver, string pagePath, string testName) : base(driver, pagePath, testName)
        {
        }

        public int GetNonLoadingRowCount()
        {
            var nonLoadingSelector = By.XPath("//table/tbody/tr[td[not(@colspan)]]");
            var loadingSelector = By.XPath("//table/tbody/tr/td[@colspan!='']");
            try
            {
                testDriver.WaitUntil(p =>
                {
                    var rows = p.FindElements(loadingSelector);
                    return rows.Count == 0;
                }, 20);
            }
            catch (Exception e)
            {
                Globals.CatchAndWrite(e, testDriver, TestName);
                return -1;
            }
            testDriver.WaitUntil(p =>
            {
                var rows = p.FindElements(nonLoadingSelector);
                return rows.Count > 0;
            });
            var nonLoadingRows = testDriver.WebDriver.FindElements(nonLoadingSelector);
            return nonLoadingRows.Count;
        }
    }
}
