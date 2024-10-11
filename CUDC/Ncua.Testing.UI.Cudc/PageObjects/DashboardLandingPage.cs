using Ncua.Testing.Utilities;
using OpenQA.Selenium;
using System;
using System.Text.RegularExpressions;

namespace Ncua.Testing.UI.Cudc.PageObjects
{
    public class DashboardLandingPage : BasePage
    {
        const string PAGEPATH = "";

        public DashboardLandingPage(WebDriverWrapper driver, string testName) : base(driver, PAGEPATH, testName)
        {
        }

        public void DoGoToPage()
        {
            try
            {
                GoToPage();

                IWebElement navigationMenu = testDriver.WaitUntilDisplayed(By.Id("admin-navbar"), 3);

            }
            catch (Exception)
            {
                try
                {
                    GoToPage();

                    IWebElement mainDiv = testDriver.WaitUntilDisplayed(By.Id("admin-navbar"));

                }
                catch (Exception e)
                {
                    Globals.CatchAndWrite(e, testDriver, TestName);

                }
            }
        }        

        public bool[] VerifyDashboard(string[] menuNames, string[] sectionNames, string ntId)
        {
            bool[] result = new bool[13]; // verification checks

            try
            {
                result[0] = testDriver.WaitUntilDisplayed(By.XPath($"//a[@class='nav-link' and text()='{menuNames[0]}']")).Displayed;
                result[1] = testDriver.WaitUntilDisplayed(By.XPath($"//a[@class='nav-link' and text()='{menuNames[1]}']")).Displayed;
                result[2] = testDriver.WaitUntilDisplayed(By.XPath($"//a[@class='nav-link' and text()='{menuNames[2]}']")).Displayed;
                result[3] = testDriver.WaitUntilDisplayed(By.XPath($"//a[@class='nav-link' and text()='{menuNames[3]}']")).Displayed;
                result[4] = testDriver.WaitUntilDisplayed(By.XPath($"//a[@class='nav-link' and text()='{menuNames[4]}']")).Displayed;
                result[5] = testDriver.WaitUntilDisplayed(By.XPath($"//a[@class='nav-link' and text()='{menuNames[5]}']")).Displayed;

                result[6] = testDriver.WaitUntilDisplayed(By.XPath($"//div[@class='card-header']//strong[text()='{sectionNames[0]}']")).Displayed;
                result[7] = testDriver.WaitUntilDisplayed(By.XPath($"//div[@class='card-header']//strong[text()='{sectionNames[1]}']")).Displayed;
                result[8] = testDriver.WaitUntilDisplayed(By.XPath($"//div[@class='card-header']//strong[text()='{sectionNames[2]}']")).Displayed;
                result[9] = testDriver.WaitUntilDisplayed(By.XPath($"//div[@class='card-header']//strong[text()='{sectionNames[3]}']")).Displayed;
                result[10] = testDriver.WaitUntilDisplayed(By.XPath($"//div[@class='card-header']//strong[text()='{sectionNames[4]}']")).Displayed;

                string desiredText = ntId.ToLower();
                IWebElement element = testDriver.WaitUntilDisplayed(By.XPath($"//*[translate(text(), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz') = '{desiredText}']"));

                result[11] = element.Displayed;

                string version = GetVersionNumber();
                
                // Define the regular expression pattern for the version format
                string pattern = @"^\d+(\.\d+)*$";
                
                result[12] = Regex.IsMatch(version, pattern);

            }
            catch (Exception e)
            {
                Globals.CatchAndWrite(e, testDriver, TestName);
            }

            return result;
        }

        public void SelectFromMenu(string routerLink)
        {
            string routerLinkComplete = "/admin/" + routerLink;
            try
            {
                IWebElement linkElement = testDriver.WaitUntilDisplayed(By.CssSelector($"a[routerlink='{routerLinkComplete}']"));
                string hrefAttributeValue = linkElement.GetAttribute("href");
                testDriver.WebDriver.Navigate().GoToUrl(hrefAttributeValue);
            }
            catch (Exception e)
            {
                Globals.CatchAndWrite(e, testDriver, TestName);
                GoToPage();
                IWebElement linkElement = testDriver.WaitUntilDisplayed(By.CssSelector($"a[routerlink='{routerLinkComplete}']"));
                string hrefAttributeValue = linkElement.GetAttribute("href");
                testDriver.WebDriver.Navigate().GoToUrl(hrefAttributeValue);
            }

        }

        private string GetVersionNumber()
        {
            var versionSelector = By.XPath("//footer/div/div/em");

            testDriver.WaitUntil(p =>
            {
                var ems = p.FindElements(versionSelector);
                return ems.Count > 0 && !string.IsNullOrWhiteSpace(ems[0].Text);
            });
            var ver = testDriver.WaitUntilElement(versionSelector);
            return ver.Text;
        }

    }
}