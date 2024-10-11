using System;
using System.Text.RegularExpressions;
using Ncua.Testing.Utilities;
using OpenQA.Selenium;

namespace Ncua.Testing.UI.Cudc.PageObjects
{
    public class ManageSurveyPage : BasePage
    {
        const string PAGEPATH = "admin/manage";

        private By btnSearchLocator = By.XPath("//input[@value='Search']");
        private By txtCuNameLocator = By.Id("search-name");//NAVY FEDERAL CREDIT UNION

        public ManageSurveyPage(WebDriverWrapper driver, string testName) : base(driver, PAGEPATH, testName)
        {
        }

        public void ClickbtnSearch()
        {
            testDriver.WaitUntilDisplayed(btnSearchLocator, 60).Click();
        }

        public void TypetxtCuName(string cuName)
        {
            testDriver.WaitUntilDisplayed(txtCuNameLocator, 60).SendKeys(cuName);
        }

        public void ClicklnkCuName(string cuName)
        {
            testDriver.WaitUntilDisplayed(By.LinkText(cuName)).Click();
        }

        public bool[] VerifyManageSurvey(string[] fieldNames, string[] columnNames, string charter, string[] questionFieldNames)
        {

            bool[] result = new bool[15]; // verification checks

            try
            {
                try
                {
                    testDriver.WaitUntilDisplayed(By.CssSelector("div[class='form-horizontal'"));
                }
                catch (Exception e)
                {
                    Globals.CatchAndWrite(e, testDriver, TestName);
                    GoToPage();
                    testDriver.WaitUntilDisplayed(By.CssSelector("div[class='form-horizontal'"));
                }

                result[0] = testDriver.WaitUntilDisplayed(By.XPath(string.Format("//div/div/label[contains(text(), '" + fieldNames[0] + "')]"))).Displayed;
                result[1] = testDriver.WaitUntilDisplayed(By.XPath(string.Format("//div/div/label[contains(text(), '" + fieldNames[1] + "')]"))).Displayed;
                result[2] = testDriver.WaitUntilDisplayed(By.XPath(string.Format("//div/div/label[contains(text(), '" + fieldNames[2] + "')]"))).Displayed;
                result[3] = testDriver.WaitUntilDisplayed(By.XPath(string.Format("//div/div/label[contains(text(), '" + fieldNames[3] + "')]"))).Displayed;
                result[4] = testDriver.WaitUntilDisplayed(By.XPath(string.Format("//div/div/label[contains(text(), '" + fieldNames[4] + "')]"))).Displayed;
                result[5] = testDriver.WaitUntilDisplayed(By.XPath(string.Format("//div/div/label[contains(text(), '" + fieldNames[5] + "')]"))).Displayed;
                result[6] = testDriver.WaitUntilDisplayed(By.XPath(string.Format("//div/div/label[contains(text(), '" + fieldNames[6] + "')]"))).Displayed;

                TypetxtCuName(charter);
                ClickbtnSearch();

                testDriver.WaitUntilDisplayed(By.XPath(string.Format("//td[contains(text(), '" + charter + "')]")), 30);

                var table = testDriver.WaitUntilDisplayed(By.XPath("//table[@class='table table-bordered']/tbody"));
                IWebElement firstRow = testDriver.WaitUntil(testDriver =>
                {
                    try
                    {
                        return table.FindElement(By.TagName("tr"));
                    }
                    catch (NoSuchElementException)
                    {
                        return null;
                    }
                });
                string firstRowText = firstRow.Text;
                result[10] = !string.IsNullOrEmpty(firstRowText.Trim());

                result[7] = testDriver.WaitUntilDisplayed(By.XPath($"//table/thead/tr/th[text()='{columnNames[0]}']")).Displayed;
                result[8] = testDriver.WaitUntilDisplayed(By.XPath($"//table/thead/tr/th[text()='{columnNames[1]}']")).Displayed;
                result[9] = testDriver.WaitUntilDisplayed(By.XPath($"//table/thead/tr/th[text()='{columnNames[2]}']")).Displayed;

                var tableElement = testDriver.WaitUntilDisplayed(By.XPath("//table[@class='table table-bordered']/tbody"));

                IWebElement row = testDriver.WaitUntil(p =>
                {
                    try
                    {
                        return table.FindElements(By.TagName("tr"))[0];
                    }
                    catch (NoSuchElementException)
                    {
                        return null;
                    }
                });
                IWebElement cell = testDriver.WaitUntil(p =>
                {
                    try
                    {
                        return row.FindElements(By.TagName("td"))[2];
                    }
                    catch (NoSuchElementException)
                    {
                        return null;
                    }
                });
                IWebElement dropdown = testDriver.WaitUntil(p =>
                {
                    try
                    {
                        return cell.FindElement(By.TagName("button"));
                    }
                    catch (NoSuchElementException)
                    {
                        return null;
                    }
                });
                dropdown.Click();

                // Wait for dropdown to show
                testDriver.WaitUntilDisplayed(By.CssSelector("div[class='dropdown-menu show']"));

                var linksWithCatSurvey = testDriver.WaitUntil(p =>
                {
                    try
                    {
                        return p.FindElements(By.XPath("//a[contains(text(), 'CAT Survey')]"));
                    }
                    catch (NoSuchElementException)
                    {
                        return null;
                    }
                });

                linksWithCatSurvey[0].Click();

                IWebElement question0 = testDriver.WaitUntilDisplayed(By.XPath(string.Format("//div[contains(text(), '" + questionFieldNames[0] + "')]")));
                string completeText = question0.Text;
                string pattern = $"{questionFieldNames[0]}[A-Za-z0-9]+";
                result[11] = Regex.IsMatch(completeText, pattern);

                IWebElement question1 = testDriver.WaitUntilDisplayed(By.XPath(string.Format("//div[contains(text(), '" + questionFieldNames[1] + "')]")));
                completeText = question1.Text;
                pattern = $"{questionFieldNames[1]}[A-Za-z0-9]+";
                result[12] = Regex.IsMatch(completeText, pattern);

                IWebElement question2 = testDriver.WaitUntilDisplayed(By.XPath(string.Format("//div[contains(text(), '" + questionFieldNames[2] + "')]")));
                completeText = question2.Text;
                pattern = $"{questionFieldNames[2]}[A-Za-z0-9]+";
                result[13] = Regex.IsMatch(completeText, pattern);

                IWebElement question3 = testDriver.WaitUntilDisplayed(By.XPath(string.Format("//div[contains(text(), '" + questionFieldNames[3] + "')]")));
                completeText = question3.Text;
                pattern = $"{questionFieldNames[3]}[A-Za-z0-9]+";
                result[14] = Regex.IsMatch(completeText, pattern);

            }
            catch (Exception e)
            {
                Globals.CatchAndWrite(e, testDriver, TestName);
            }

            return result;

        }

    }
}

