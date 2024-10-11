using System;
using System.Collections.Generic;
using Ncua.Testing.Utilities;
using OpenQA.Selenium;

namespace Ncua.Testing.UI.Cudc.PageObjects
{
    public class SearchPage : BasePage
    {
        const string PAGEPATH = "admin/survey";

        private By btnSearchLocator = By.XPath("//input[@value='Search']");
        private By txtCuNameLocator = By.Id("search-name");//NAVY FEDERAL CREDIT UNION
        
        public SearchPage(WebDriverWrapper driver, string testName) : base(driver, PAGEPATH, testName)
        {
        }

        public void ClickbtnSearch()
        {
            testDriver.WaitUntilDisplayed(btnSearchLocator, 60).Click();
        }

        public void TypetxtCuName(string cuName)
        {
            testDriver.WaitUntilDisplayed(txtCuNameLocator,60).SendKeys(cuName);
        }

        public void ClicklnkCuName(string cuName)
        {
            testDriver.WaitUntilDisplayed(By.LinkText(cuName)).Click();
        }

        public string GetVersionNumber()
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

        private bool FindSearchResult(string charterNumber)
        {
            return testDriver.WaitUntilDisplayed(By.LinkText(charterNumber)).Displayed;
        }

        public bool SearchForCu(string cuName, string charterNumber)
        {
            var takeSurveyLinkSelector = By.XPath("//a[@routerlink = '/admin/survey']");
            testDriver.LogInformation("Looking for link or CU Name textbox, will wait 30 seconds...");
            testDriver.WaitUntil(p =>
            {
                return p.FindElements(takeSurveyLinkSelector).Count > 0 || p.FindElements(txtCuNameLocator).Count > 0;
            }, 30);
            var takeSurveyLinks = testDriver.WebDriver.FindElements(takeSurveyLinkSelector);
            if (takeSurveyLinks.Count > 0)
            {
                testDriver.LogInformation("Found link, clicking...");
                takeSurveyLinks[0].Click();
            }
            else
            {
                testDriver.LogInformation("Did not find link.");
            }
            TypetxtCuName(cuName);
            ClickbtnSearch();
            return FindSearchResult(charterNumber);
        }

        public CudcconfirmationPage AddCommentsForCu(string cuName, string charterNumber, string commentText)
        {
            SearchForCu(cuName, charterNumber);
            ClicklnkCuName(cuName);
            var textInput = testDriver.WaitUntilDisplayed(By.XPath("//input[@type='text' and @class='ng-untouched ng-pristine ng-valid']"), 30);
            textInput.Clear();
            textInput.SendKeys(commentText);

            // submit button
            testDriver.WaitUntilDisplayed(By.ClassName("btn-primary"), 30).Click();

            // yes button
            testDriver.WaitUntilDisplayed(By.XPath("//button[text() = 'Yes']"), 30).Click();
            return new CudcconfirmationPage(testDriver, TestName);
        }

        public bool SearchCusForTakingSurvey(int[] charters)
        {
            try
            {
                foreach (var charter in charters)
                {
                    GoToPage();
                    TypetxtCuName(charter.ToString());
                    ClickbtnSearch();
                    var tableElement = testDriver.WaitUntilDisplayed(By.XPath("//table[@class='table table-bordered']/tbody"),60);
                    IList<IWebElement> rows = tableElement.FindElements(By.TagName("tr"));
                    if (rows.Count == 0)
                        return false;

                }

            }
            catch (Exception e)
            {
                Globals.CatchAndWrite(e, testDriver, TestName);
            }

            return true;
        }

        // Method fills up charters array with corp and noncorp charter numbers based on method argument chartersPerCuType
        public int[] GetCorpAndNonCorpCharters(int chartersPerCuType)
        {
            int[] corpAndNonCorpCharters = new int[chartersPerCuType * 2];
            IWebElement tableElement;
            try
            {
                try
                {
                    GoToPage();
                    testDriver.WaitUntilDisplayed(By.CssSelector("a[class='navbar-brand'"), 30);

                }
                catch 
                {
                    GoToPage();
                }

                ClickbtnSearch();

                try
                {
                    tableElement = testDriver.WaitUntilDisplayed(By.XPath("//table[@class='table table-bordered']/tbody"), 121);
                }
                catch
                {
                    GoToPage();
                    testDriver.WaitUntilDisplayed(By.CssSelector("a[class='navbar-brand'"), 30);
                    ClickbtnSearch();

                    tableElement = testDriver.WaitUntilDisplayed(By.XPath("//table[@class='table table-bordered']/tbody"), 122);
                }                

                IList<IWebElement> rows = tableElement.FindElements(By.TagName("tr"));
                IList<IWebElement> columns;

                int corpChartersLimit = 0;
                int nonCorpChartersLimit = 0;
                int x = 0;

                for (int i = rows.Count; i > 0; i--)
                {
                    if (rows[i - 1].Text.Contains("CORPORATE") && corpChartersLimit < chartersPerCuType)
                    {
                        columns = rows[i - 1].FindElements(By.TagName("td"));
                        corpAndNonCorpCharters[x++] = int.Parse(columns[1].Text);
                        corpChartersLimit++;
                    }
                    else if (nonCorpChartersLimit < chartersPerCuType)
                    {
                        columns = rows[i - 1].FindElements(By.TagName("td"));
                        corpAndNonCorpCharters[x++] = int.Parse(columns[1].Text);
                        nonCorpChartersLimit++;
                    }

                    if (x == chartersPerCuType * 2)
                    {
                        return corpAndNonCorpCharters;
                    }
                }


            }
            catch (Exception e)
            {
                Globals.CatchAndWrite(e, testDriver, TestName);
            }

            return corpAndNonCorpCharters;
        }

        public bool[] TakeSurvey(int charterNumber, string surveyBeingTaken)
        {
            bool[] result = new bool[8]; // holds result of verification checks

            try
            {
                GoToPage();
                TypetxtCuName(charterNumber.ToString());
                ClickbtnSearch();
                testDriver.WaitUntilDisplayed(By.XPath(string.Format("//td[contains(text(), '" + charterNumber.ToString() + "')]")),120);

                var tableElement = testDriver.WaitUntilDisplayed(By.XPath("//table[@class='table table-bordered']/tbody"));
                IList<IWebElement> rows = tableElement.FindElements(By.TagName("tr"));
                
                if (rows.Count > 0)
                {
                    result[0] = true;
                    
                    var takeSurveyLink = testDriver.WaitUntilDisplayed(By.LinkText("Take Survey"), 60);
                    takeSurveyLink.Click();

                    // Ensure all parts of survey are displayed
                    var charter = testDriver.WaitUntilDisplayed(By.XPath(string.Format("//div[contains(text(), 'Charter Number: " + charterNumber + "')]")),60);
                    var lastStatement = testDriver.WaitUntilDisplayed(By.XPath("//span[contains(text(), 'This concludes the survey. You may save or submit.')]"),60);
                    var saveReviewButton = testDriver.WaitUntilDisplayed(By.XPath("//button[contains(text(), 'Save and Review')]"),60);
                    result[1] = charter.Displayed && lastStatement.Displayed && saveReviewButton.Displayed;

                    var inputFields = testDriver.WebDriver.FindElements(By.CssSelector("input[type='text']"));

                    TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                    Globals.Timestamp = Convert.ToInt64(ts.TotalMilliseconds).ToString();

                    string todaysDate = DateTime.Now.ToString("M/d/y");
                    inputFields[0].SendKeys(todaysDate);

                    IList<IWebElement> statusDropdown = testDriver.WebDriver.FindElements(By.XPath("//select[@class='ng-untouched ng-pristine ng-valid']//option"));
                    statusDropdown[1].Click();

                    inputFields[1].SendKeys(Globals.Timestamp);
                   
                    var answerFields = testDriver.WaitUntil(d =>
                    {
                        try
                        {
                            return d.FindElements(By.ClassName("answer"));
                        }
                        catch (NoSuchElementException)
                        {
                            return null;
                        }
                    });

                    IWebElement fourthAnswerField = answerFields[3];                    
                    IWebElement textareaElement = testDriver.WaitUntil(testDriver =>
                    {
                        try
                        {
                            return fourthAnswerField.FindElement(By.TagName("textarea"));
                        }
                        catch (NoSuchElementException)
                        {
                            return null;
                        }
                    });

                    textareaElement.SendKeys("Unique text being entered in textarea field: " + Globals.Timestamp);

                    inputFields[2].SendKeys("Unique text being entered in input field: " + Globals.Timestamp);

                    var saveAndReviewButton = testDriver.WaitUntilDisplayed(By.CssSelector("button[type='submit'"));
                    saveAndReviewButton.Click();

                    testDriver.WaitUntilDisplayed(By.Id("save-review"));
                    var yesButton = testDriver.WaitUntilDisplayed(By.XPath("//button[contains(text(), 'Yes')]"));

                    IJavaScriptExecutor executor = (IJavaScriptExecutor)testDriver.WebDriver;
                    executor.ExecuteScript("arguments[0].click();", yesButton);

                    System.Threading.Thread.Sleep(1000);

                    testDriver.WaitUntilDisplayed(By.XPath("//button[contains(text(), 'Submit')]"),30);

                    System.Threading.Thread.Sleep(1000);

                    string headerCheck = surveyBeingTaken + " (COPY " + todaysDate;
                    result[2] = testDriver.WaitUntilDisplayed(By.XPath(string.Format("//h3[contains(text(), '" + headerCheck + "')]"))).Displayed;

                    string charterCheck = "Charter Number: " + charterNumber.ToString();
                    result[3] = testDriver.WaitUntilDisplayed(By.XPath(string.Format("//div[contains(text(), '" + charterCheck + "')]"))).Displayed;

                    string questionCheck = "Unique text being entered in input field: " + Globals.Timestamp;
                    result[4] = testDriver.WaitUntilDisplayed(By.XPath(string.Format("//span[contains(text(), '" + questionCheck + "')]"))).Displayed;

                    var submitButton = testDriver.WaitUntilDisplayed(By.XPath("//button[contains(text(), 'Submit')]"));
                    submitButton.Click();

                    string submitConfirmation = " Are you sure you wish to submit this survey? ";
                    result[5] = testDriver.WaitUntilDisplayed(By.XPath(string.Format("//div[contains(text(), '" + submitConfirmation + "')]"))).Displayed;

                    var yesButton2 = testDriver.WaitUntilDisplayed(By.XPath("//button[contains(text(), 'Yes')]"));
                    yesButton2.Click();

                    string surveySubmissionConfirmation = "Thank you for taking the time to complete our survey.";
                    result[6] = testDriver.WaitUntilDisplayed(By.XPath(string.Format("//h1[contains(text(), '" + surveySubmissionConfirmation + "')]"))).Displayed;

                    string surveySubmissionConfirmation2 = "Your data was submitted successfully!";
                    result[7] = testDriver.WaitUntilDisplayed(By.XPath(string.Format("//h3[contains(text(), '" + surveySubmissionConfirmation2 + "')]"))).Displayed;

                }
                else result[0] = false;

            }
            catch (Exception e)
            {
                Globals.CatchAndWrite(e, testDriver, TestName);                
            }

            return result;

        }

        public bool[] VerifyTakeSurvey(string[] fieldNames, string[] columnNames, string charter)
        {

            bool[] result = new bool[11]; // verification checks

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

            }
            catch (Exception e)
            {
                Globals.CatchAndWrite(e, testDriver, TestName);
            }

            return result;

        }

    }
}

