using Ncua.Testing.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Ncua.Testing.UI.Cudc.PageObjects
{
    public class AdminSurveyListPage : AdminListBasePage
    {
        const string PAGEPATH = "admin/surveys";

        public AdminSurveyListPage(WebDriverWrapper driver, string testName) : base(driver, PAGEPATH, testName)
        {
        }

        public bool[] CopySurveyAndMakeActive(string surveyBeingTaken)
        {
            bool[] result = new bool[2]; // holds result of verification checks
            try
            {
                GoToPage();               
               
                var surveyBeingTakenExists = testDriver.WaitUntilDisplayed(By.XPath(string.Format("//td[contains(text(), '" + surveyBeingTaken + "')]")), 60);

                result[0] = surveyBeingTakenExists.Displayed;

                result[1] = CopySurvey(surveyBeingTaken);

                ChangeSurveyStatus(surveyBeingTaken + " (COPY", "true");

                testDriver.WaitUntilDisplayed(By.XPath(string.Format("//td[contains(text(), '" + surveyBeingTaken + "')]")),60);

            }
            catch (Exception e)
            {
                Globals.CatchAndWrite(e, testDriver, TestName);
            }
            
            return result;
        }

        public bool DeleteSurveyCopies(string surveyTaken)
        {
            bool result = false;
            try
            {
                byte count = 0;
                while (DeleteSurveyCopy(surveyTaken))
                {
                    count++;
                }
                result = count > 0;

            }
            catch (Exception e)
            {
                Globals.CatchAndWrite(e, testDriver, TestName);
            }

            return result;
        }

        private bool DeleteSurveyCopy(string surveyTaken)
        {
            
            bool canDeleteSurveysResult = false;
            try
            {
               try
                {
                    GoToPage();
                    testDriver.WaitUntilDisplayed(By.XPath(string.Format("//td[contains(text(), '" + surveyTaken + "')]")));
                }
                catch (Exception e)
                {
                    Globals.CatchAndWrite(e, testDriver, TestName);
                    GoToPage();
                    testDriver.WaitUntilDisplayed(By.XPath(string.Format("//td[contains(text(), '" + surveyTaken + "')]")), 30);
                }

                string surveyCopyPattern = surveyTaken + " (COPY";

                var tableElement = testDriver.WaitUntilDisplayed(By.XPath("//table[@class='table table-bordered mt-3']/tbody"),60);
                IList<IWebElement> rows = tableElement.FindElements(By.TagName("tr"));
                IList<IWebElement> columns;

                var rowsCount = rows.Count;

                for (int i = 0; i < rowsCount; i++)
                {                    
                    rows = tableElement.FindElements(By.TagName("tr"));
                    columns = rows[i].FindElements(By.TagName("td"));
                    string title = columns[0].Text;
                    if (title.Contains(surveyCopyPattern))
                    {
                        // More Actions Field Dropdown
                        columns[6].Click();

                        // Wait for dropdown to be displayed
                        testDriver.WaitUntilDisplayed(By.CssSelector("div[class='dropdown-menu show']"));

                        // Selection: Delete
                        var linksWithDelete = testDriver.WebDriver.FindElements(By.XPath("//a[contains(text(), 'Delete')]"));
                        linksWithDelete[i].Click();

                        // Wait for copy confirmation popup dialog to be displayed
                        testDriver.WaitUntilDisplayed(By.XPath("//div[contains(text(), 'Are you sure you want to delete this survey?')]"));

                        // Button: OK
                        var buttonsWithOk = testDriver.WebDriver.FindElements(By.XPath("//button[contains(text(), 'OK')]"));
                        buttonsWithOk[2].Click();

                        WaitForSpinner(30);
                        
                        testDriver.WaitUntilDisplayed(By.XPath(string.Format("//td[contains(text(), '" + surveyTaken + "')]")));

                        canDeleteSurveysResult = true;

                        i = rowsCount;

                    }
                }

            }
            catch (Exception e)
            {
                Globals.CatchAndWrite(e, testDriver, TestName);
            }

            return canDeleteSurveysResult;
        }

        private bool CopySurvey(string surveyTaken)
        {
            bool canCopyResult = false;
            try
            {
                testDriver.WaitUntilDisplayed(By.XPath(string.Format("//td[contains(text(), '" + surveyTaken + "')]")),60);

                var tableElement = testDriver.WaitUntilDisplayed(By.XPath("//table[@class='table table-bordered mt-3']/tbody"),60);
                IList<IWebElement> rows = tableElement.FindElements(By.TagName("tr"));
                IList<IWebElement> columns;
                var rowsCount = rows.Count;

                for (int i = 0; i < rowsCount; i++)
                {
                    columns = rows[i].FindElements(By.TagName("td"));
                    string title = columns[0].Text;
                    if (title == surveyTaken)
                    {
                        // More Actions Field Dropdown
                        columns[6].Click();

                        // Wait for dropdown to be displayed
                        testDriver.WaitUntilDisplayed(By.CssSelector("div[class='dropdown-menu show']")); 
                                                
                        // Selection: Copy
                        var linksWithCopy = testDriver.WebDriver.FindElements(By.XPath("//a[contains(text(), 'Copy')]"));
                        linksWithCopy[i].Click();

                        // Wait for copy confirmation popup dialog to be displayed
                        testDriver.WaitUntilDisplayed(By.XPath("//div[contains(text(), 'Are you sure you want to copy this survey?')]"));

                        // Button: OK
                        var buttonsWithOk = testDriver.WebDriver.FindElements(By.XPath("//button[contains(text(), 'OK')]"));
                        buttonsWithOk[1].Click();

                        WaitForSpinner(3);

                        testDriver.WaitUntilDisplayed(By.XPath(string.Format("//td[contains(text(), '" + surveyTaken + "')]")));

                        i = rowsCount;

                        canCopyResult = true;

                    }
                }
            }
            catch (Exception e)
            {
                Globals.CatchAndWrite(e, testDriver, TestName);
            }

            return canCopyResult;
        }

        public string GetActiveSurvey(string surveyTaken)
        {
            string activeSurvey = "";

            try
            {
                GoToPage();
                var surveyBeingTakenExists = testDriver.WaitUntilDisplayed(By.XPath(string.Format("//td[contains(text(), '" + surveyTaken + "')]")),60);

                var tableElement = testDriver.WaitUntilDisplayed(By.XPath("//table[@class='table table-bordered mt-3']/tbody"));
                IList<IWebElement> rows = tableElement.FindElements(By.TagName("tr"));
                IList<IWebElement> columns = rows[0].FindElements(By.TagName("td"));

                var rowsCount = rows.Count;
                var columnsCount = columns.Count;

                string[,] tableCopy = new string[rowsCount, columnsCount];

                for (int i = 0; i < rowsCount; i++)
                {
                    columns = rows[i].FindElements(By.TagName("td"));

                    for (int j = 0; j < columnsCount; j++)
                    {
                        tableCopy[i, j] = columns[j].Text;
                    }

                }

                for (int i = 0; i < rowsCount; i++)
                {                                 
                    if(tableCopy[i, 4].Contains("Active"))
                    {
                        Console.WriteLine("activeSurvey = " + tableCopy[i, 0]);
                        return tableCopy[i, 0]; // Survey Title
                    }                
                }
 
                // If no survey is active, make the surveyTaken Active
                if (activeSurvey == "")
                {
                    activeSurvey = surveyTaken;
                    ChangeSurveyStatus(activeSurvey, "true");
                }
                
                Console.WriteLine("activeSurvey = " + activeSurvey);

            }
            catch (Exception e)
            {
                Globals.CatchAndWrite(e, testDriver, TestName);
            }

            return activeSurvey;
        }

        public void ChangeSurveyStatus(string surveyName, string status)
        {            
            try
            {
                try
                {
                    GoToPage();
                    
                    // This is the exact location that fails the 1st time after new builds/deployments.
                    testDriver.WaitUntilDisplayed(By.XPath(string.Format("//td[contains(text(), '" + surveyName + "')]")), 11);
                }
                catch (Exception e)
                {
                    Globals.CatchAndWrite(e, testDriver, TestName);
                    GoToPage();
                    testDriver.WaitUntilDisplayed(By.XPath(string.Format("//td[contains(text(), '" + surveyName + "')]")), 60);
                }

                IWebElement table = testDriver.WaitUntilDisplayed(By.XPath("//table[@class='table table-bordered mt-3']/tbody"),60);
                IList<IWebElement> rows = table.FindElements(By.TagName("tr"));

                for (int rowIndex = 1; rowIndex < rows.Count; rowIndex++)
                {
                    IWebElement row = rows[rowIndex];

                    // Find the cells in the current row
                    IList<IWebElement> cells = row.FindElements(By.TagName("td"));

                    // Check if the first column contains the desired string value
                    if (cells[0].Text.Contains(surveyName))
                    {
                        // Find the Edit link in the sixth column and click it
                        IWebElement editLink = testDriver.WaitUntil(ExpectedConditions.ElementToBeClickable(cells[5].FindElement(By.TagName("a"))));

                        editLink.Click();

                        // Select Active or Inactive from Status field dropdown
                        SelectElement statusDropdown = new SelectElement(testDriver.WaitUntilDisplayed(By.Id("status-field")));
                        statusDropdown.SelectByValue(status);

                        // Click the Ok button
                        testDriver.WaitUntilDisplayed(By.XPath("//button[contains(text(), 'OK')]")).Click();

                        break; // Exit the loop if the row with the right Survey is found
                    }
                }

            }
            catch (Exception e)
            {
                Globals.CatchAndWrite(e, testDriver, TestName);
            }
        }

        private void WaitForSpinner(int timeout)
        {
            try
            {
                var spinnerVisible = testDriver.WaitUntilDisplayed(By.CssSelector("div[class='modal fade show']"), timeout);
                if (spinnerVisible != null)
                {
                    testDriver.WaitUntil(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("div[class='modal fade show']")), timeout);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Spinner did not work as expected. ");
                Globals.CatchAndWrite(e, testDriver, TestName);
            }
        }

        public bool[] VerifySurveys(string[] columnNames)
        {

            bool[] result = new bool[8]; // verification checks
            IWebElement table = null;
            string firstRowText = null;

            try
            {
                try
                {
                    table = testDriver.WaitUntilDisplayed(By.XPath("//table[@class='table table-bordered mt-3']/tbody"));
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
                    firstRowText = firstRow.Text;

                }
                catch (Exception e)
                {
                    Globals.CatchAndWrite(e, testDriver, TestName);
                    GoToPage();
                    table = testDriver.WaitUntilDisplayed(By.XPath("//table[@class='table table-bordered mt-3']/tbody"));
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
                    firstRowText = firstRow.Text;

                }

                result[7] = !string.IsNullOrEmpty(firstRowText.Trim());

                result[0] = testDriver.WaitUntilDisplayed(By.XPath($"//table/thead/tr/th[text()='{columnNames[0]}']")).Displayed;
                result[1] = testDriver.WaitUntilDisplayed(By.XPath($"//table/thead/tr/th[text()='{columnNames[1]}']")).Displayed;
                result[2] = testDriver.WaitUntilDisplayed(By.XPath($"//table/thead/tr/th[text()='{columnNames[2]}']")).Displayed;
                result[3] = testDriver.WaitUntilDisplayed(By.XPath($"//table/thead/tr/th[text()='{columnNames[3]}']")).Displayed;
                result[4] = testDriver.WaitUntilDisplayed(By.XPath($"//table/thead/tr/th[text()='{columnNames[4]}']")).Displayed;
                result[5] = testDriver.WaitUntilDisplayed(By.XPath($"//table/thead/tr/th[text()='{columnNames[5]}']")).Displayed;
                result[6] = testDriver.WaitUntilDisplayed(By.XPath($"//table/thead/tr/th[text()='{columnNames[6]}']")).Displayed;

            }
            catch (Exception e)
            {
                Globals.CatchAndWrite(e, testDriver, TestName);
            }

            return result;

        }
    }
    public static class Globals
    {
        static string timestamp = "-1";
        public static string Timestamp
        {
            set { timestamp = value; }
            get { return timestamp; }
        }

        public static void CatchAndWrite(Exception e, WebDriverWrapper wdw, string testName)
        {
            var timeNow = System.DateTime.Now.ToString("h mm ss tt");

            string classNameAndLineNumber = e.StackTrace.Substring(e.StackTrace.LastIndexOf('\\') + 1);
            classNameAndLineNumber = classNameAndLineNumber.Replace(".cs:", "-");

            string exceptionMessage = e.Message;
            exceptionMessage = exceptionMessage.Replace(":", "-");

            if (exceptionMessage.Length > 30)
            {
                exceptionMessage = exceptionMessage.Substring(0, 30);
            }

            string nameSpace = typeof(Globals).Namespace;
            string testApp;
            string firstString = "UI.";
            string lastString = ".PageObjects";
            int Pos1 = nameSpace.IndexOf(firstString) + firstString.Length;
            int Pos2 = nameSpace.IndexOf(lastString);
            testApp = nameSpace.Substring(Pos1, Pos2 - Pos1);

            string p1 = testApp;
            string p2 = testName;
            string p3 = classNameAndLineNumber + "-" + timeNow;
            string p4 = exceptionMessage;

            // file example: p1.p2-TestingInternal-20221007103147-p3-p4.png
            wdw.SaveScreenshot(p4, p1, p2, p3);

            Console.WriteLine("App and Test Name that threw exception: " + testApp + "." + testName);
            Console.WriteLine("Exception page class and line number: " + classNameAndLineNumber);
            Console.WriteLine("Exception time: " + timeNow);
            Console.WriteLine("Exception message: " + e.Message);
            Console.WriteLine("Exception source: " + e.Source);
            Console.WriteLine("Method that threw exception: " + e.TargetSite);
            Console.WriteLine("Exception data: " + e.Data);
            Console.WriteLine("Stack Trace: " + e.StackTrace);
            Console.WriteLine("-   -   -   -   -   -   -   -   -   -   -   -   -   -   -   -");

        }
    }

}
