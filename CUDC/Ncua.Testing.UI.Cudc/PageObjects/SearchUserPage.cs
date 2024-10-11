using Ncua.Testing.Utilities;
using OpenQA.Selenium;
using System;
using System.Collections.ObjectModel;
using SeleniumExtras.WaitHelpers;

namespace Ncua.Testing.UI.Cudc.PageObjects
{
    public class SearchUserPage : BasePage
    {
        const string PAGEPATH = "admin/search-user";

        public SearchUserPage(WebDriverWrapper driver, string testName) : base(driver, PAGEPATH, testName)
        {
        }

        public bool[] VerifySearchUser(string[] topTableColumns, string[] bottomTableColumns, string user)
        {

            bool[] result = new bool[14]; // verification checks

            string topTableCaption = "Search Results";
            string bottomTableCaption = "Permission Details";
            IWebElement topTable = null;
            IWebElement bottomTable = null;
            ReadOnlyCollection<IWebElement> column = null;

            try
            {
                try
                {
                    topTable = testDriver.WaitUntilDisplayed(By.XPath($"//table[..//caption[contains(text(), '{topTableCaption}')]]"));

                    column = testDriver.WaitUntil(p =>
                    {
                        try
                        {
                            return topTable.FindElements(By.TagName("th"));
                        }
                        catch (NoSuchElementException)
                        {
                            return null;
                        }
                    });

                }
                catch (Exception e)
                {
                    Globals.CatchAndWrite(e, testDriver, TestName);
                    GoToPage();
                    topTable = testDriver.WaitUntilDisplayed(By.XPath($"//table[.//caption[contains(text(), '{topTableCaption}')]]"));
                    column = testDriver.WaitUntil(p =>
                    {
                        try
                        {
                            return topTable.FindElements(By.TagName("th"));
                        }
                        catch (NoSuchElementException)
                        {
                            return null;
                        }
                    });
                }
                
                result[0] = column[0].Text.Equals(topTableColumns[0]);
                result[1] = column[1].Text.Equals(topTableColumns[1]) || column[1].Text.Equals("Empoyee #");  // remove after typo fixed.
                result[2] = column[2].Text.Equals(topTableColumns[2]);
                result[3] = column[3].Text.Equals(topTableColumns[3]);
                result[4] = column[4].Text.Equals(topTableColumns[4]);
                result[5] = column[5].Text.Equals(topTableColumns[5]);

                IWebElement firstNameField = testDriver.WaitUntilDisplayed(By.Id("search-firstName"));
                firstNameField.SendKeys(user);

                IWebElement searchButton = testDriver.WaitUntilDisplayed(By.XPath("//input[@type='submit' and @value='Search']"));
                searchButton.Click();

                IWebElement viewPermissionsLink = testDriver.WaitUntil(ExpectedConditions.ElementToBeClickable(By.XPath(".//tr[1]//a")));

                viewPermissionsLink.Click();

                Console.WriteLine("[CanVerifySearchUser]...just clicked 'View Permissions' link 1st time.");

                bottomTable = testDriver.WaitUntilDisplayed(By.XPath($"//table[.//caption[contains(text(), '{bottomTableCaption}')]]"));

                var tableVisible = testDriver.WaitUntil(testDriver =>
                {
                    var row = bottomTable.FindElement(By.XPath(".//tr[2]"));
                    return row.Displayed;

                },21);

                IWebElement firstRow = null;
                try
                {
                    firstRow = testDriver.WaitUntil(testDriver =>
                    {
                        return bottomTable.FindElements(By.TagName("tr"))[1];

                    });

                }
                catch (Exception)
                {
                    Console.WriteLine("[CanVerifySearchUser]...was not able to find firstRow afrer clicking 'View Permissions' link the first time, clicking link again using js.");

                    IJavaScriptExecutor executor2 = (IJavaScriptExecutor)testDriver.WebDriver;
                    executor2.ExecuteScript("arguments[0].click();", viewPermissionsLink);

                    Console.WriteLine("[CanVerifySearchUser]...just clicked 'View Permissions' link a 2nd time");

                    bottomTable = testDriver.WaitUntilDisplayed(By.XPath($"//table[.//caption[contains(text(), '{bottomTableCaption}')]]"));

                    var tableVisible2 = testDriver.WaitUntil(testDriver =>
                    {
                        var row = bottomTable.FindElement(By.XPath(".//tr[2]"));
                        return row.Displayed;

                    },22);

                    firstRow = testDriver.WaitUntil(testDriver =>
                    {
                        return bottomTable.FindElements(By.TagName("tr"))[1];

                    });
                }

                column = testDriver.WaitUntil(p =>
                {
                    try
                    {
                        return bottomTable.FindElements(By.TagName("th"));
                    }
                    catch (NoSuchElementException)
                    {
                        return null;
                    }
                });
                result[6] = column[0].Text.Equals(bottomTableColumns[0]);
                result[7] = column[1].Text.Equals(bottomTableColumns[1]);
                result[8] = column[2].Text.Equals(bottomTableColumns[2]);
                result[9] = column[3].Text.Equals(bottomTableColumns[3]);
                result[10] = column[4].Text.Equals(bottomTableColumns[4]);
                result[11] = column[5].Text.Equals(bottomTableColumns[5]);
                result[12] = column[6].Text.Equals(bottomTableColumns[6]);

                Console.WriteLine("[CanVerifySearchUser]...just verified column headers for the 2nd table.");

                string firstRowText = firstRow.Text;
                Console.WriteLine("[CanVerifySearchUser]...firstRowText = " + firstRowText);
                result[13] = !string.IsNullOrEmpty(firstRowText.Trim());

            }
            catch (Exception e)
            {
                Globals.CatchAndWrite(e, testDriver, TestName);
            }

            return result;

        }

    }
}