using Ncua.Testing.Utilities;
using OpenQA.Selenium;
using System;

namespace Ncua.Testing.UI.Cudc.PageObjects
{
    public class RadarViewPage : BasePage
    {
        const string PAGEPATH = "admin/radar-view";

        public RadarViewPage(WebDriverWrapper driver, string testName) : base(driver, PAGEPATH, testName)
        {
        }

        public bool[] VerifyRadarView(string[] fieldNames)
        {

            bool[] result = new bool[3]; // verification checks

            try
            {
                try
                {
                    result[0] = testDriver.WaitUntilDisplayed(By.XPath($"//form/div/label[text()='{fieldNames[0]}']")).Displayed;
                }
                catch (Exception e)
                {
                    Globals.CatchAndWrite(e, testDriver, TestName);
                    GoToPage();
                    result[0] = testDriver.WaitUntilDisplayed(By.XPath($"//form/div/label[text()='{fieldNames[0]}']")).Displayed;
                }

                result[1] = testDriver.WaitUntilDisplayed(By.XPath($"//form/div/label[text()='{fieldNames[1]}']")).Displayed;
                result[2] = testDriver.WaitUntilDisplayed(By.XPath($"//form/div/label[text()='{fieldNames[2]}']")).Displayed;

            }
            catch (Exception e)
            {
                Globals.CatchAndWrite(e, testDriver, TestName);
            }

            return result;

        }

    }
}