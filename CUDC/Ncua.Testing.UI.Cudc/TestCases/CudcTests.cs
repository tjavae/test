using Ncua.Testing.UI.Cudc.PageObjects;
using Ncua.Testing.Utilities;
using NUnit.Framework;
using System;

namespace Ncua.Testing.UI.Cudc.TestCases
{
    public class CudcTests : BaseTests
    {
        [Test, NcuaTestCaseSource(typeof(CommonUtilities), nameof(CommonUtilities.TestCaseSource))]
        public void CanGoToPage(string environment, string expectedPageTitle)
        {
            testConfig.Environment = environment;
            
            SearchPage page = new SearchPage(TestDriver, "CanGoToPage");
            page.GoToPage();
            var actualPageTitle = TestDriver.WebDriver.Title;
            Assert.AreEqual(expectedPageTitle, actualPageTitle);
            
        }

        [Test, NcuaTestCaseSource(typeof(CommonUtilities), nameof(CommonUtilities.TestCaseSource))]
        public void CanSeeAdminSurveys(string environment, int numSurveysGreaterEqual)
        {
            testConfig.Environment = environment;

            var page = new AdminSurveyListPage(TestDriver, "CanSeeAdminSurveys");
            page.GoToPage();
            var numRows = page.GetNonLoadingRowCount();

            Assert.Multiple(() =>
            {
                Assert.True(-1 != numRows, "Not able to load surveys.");
                Assert.True(numRows >= numSurveysGreaterEqual, $"Expected at least {numSurveysGreaterEqual} surveys but only found {numRows}");

            });

        }

        [Test, NcuaTestCaseSource(typeof(CommonUtilities), nameof(CommonUtilities.TestCaseSource))]
        public void VerifyVersion(string environment, string expectedVersion)
        {
            testConfig.Environment = environment;
            
            var page = new SearchPage(TestDriver, "VerifyVersion");
            page.GoToPage();

            string foundVersion = page.GetVersionNumber();

            Assert.True(foundVersion.StartsWith(expectedVersion, StringComparison.InvariantCultureIgnoreCase), $"Expected version to start with {expectedVersion} but found {foundVersion}");
            
        }
        
        [Test, NcuaTestCaseSource(typeof(CommonUtilities), nameof(CommonUtilities.TestCaseSource))]
        public void CanSearchForCU(string environment, string cuName, string charterNumber)
        {
            testConfig.Environment = environment;
            
            var page = new SearchPage(TestDriver, "CanSearchForCU");
            page.GoToPage();
            var foundCu = page.SearchForCu(cuName, charterNumber);

            Assert.True(foundCu, $"Unable to find CU with charter number {charterNumber} by the search string {cuName}");
            
        }
        
        [Test, NcuaTestCaseSource(typeof(CommonUtilities), nameof(CommonUtilities.TestCaseSource))]
        public void CanAddCommentsForCU(string environment, string cuName, string charterNumber)
        {
            testConfig.Environment = environment;
            
            SearchPage page = new SearchPage(TestDriver, "CanAddCommentsForCU");
            page.GoToPage();

            var commentText = $"Comments: {DateTime.Now}";
            var detailPage = page.AddCommentsForCu(cuName, charterNumber, commentText);
            var foundText = detailPage.VerifySubmitted(commentText);

            Assert.IsTrue(foundText, $"Unable to find comment text \"{commentText}\" on review page.");            
        }

        [Test, NcuaTestCaseSource(typeof(CommonUtilities), nameof(CommonUtilities.TestCaseSource))]
        public void CanSearchCorpAndNonCorpCu(string environment, string surveyTaken)
        {
            // Arrange
            
            testConfig.Environment = environment;

            // Act

            AdminSurveyListPage adminSurveyListPage = new AdminSurveyListPage(TestDriver, "CanSearchCorpAndNonCorpCu");
            string originalActiveSurvey = adminSurveyListPage.GetActiveSurvey(surveyTaken);

            SearchPage searchPage = new SearchPage(TestDriver, "CanSearchCorpAndNonCorpCu");
            int[] corpAndNonCorpCharters = searchPage.GetCorpAndNonCorpCharters(2); // argument is how many pairs of Corp and Noncorp it searches and returns
            bool searchSuccessfull = searchPage.SearchCusForTakingSurvey(corpAndNonCorpCharters);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.IsTrue(searchSuccessfull, "CUDC Search for Credit Union did not return at least one record or charter. ");
                Assert.That(corpAndNonCorpCharters[0], Is.InRange(9999, 100000), "CUDC Search for Credit Union did not return a 5-digit charter");
                Assert.That(corpAndNonCorpCharters[1], Is.InRange(9999, 100000), "CUDC Search for Credit Union did not return a 5-digit charter");
                Assert.That(corpAndNonCorpCharters[2], Is.InRange(9999, 100000), "CUDC Search for Credit Union did not return a 5-digit charter");
                Assert.That(corpAndNonCorpCharters[3], Is.InRange(9999, 100000), "CUDC Search for Credit Union did not return a 5-digit charter");
            });
        }

        [Test, NcuaTestCaseSource(typeof(CommonUtilities), nameof(CommonUtilities.TestCaseSource))]
        public void CanTakeCudcSurvey(string environment, string surveyTaken)
        {
            // Arrange
            
            testConfig.Environment = environment;

            // Act

            AdminSurveyListPage adminSurveyListPage = new AdminSurveyListPage(TestDriver, "CanTakeCudcSurvey");
            adminSurveyListPage.DeleteSurveyCopies(surveyTaken); // initial cleanup to remove any copies leftover from previous runs
            string originalActiveSurvey = adminSurveyListPage.GetActiveSurvey(surveyTaken);
            bool[] CopySurveyAndMakeActiveSuccess = adminSurveyListPage.CopySurveyAndMakeActive(surveyTaken);

            SearchPage searchPage = new SearchPage(TestDriver, "CanTakeCudcSurvey");
            int[] corpAndNonCorpCharters = searchPage.GetCorpAndNonCorpCharters(1); // argument is how many pairs of Corp and Noncorp it searches and returns

            bool[] takeSurvey1 = searchPage.TakeSurvey(corpAndNonCorpCharters[0], surveyTaken);
            bool[] takeSurvey2 = searchPage.TakeSurvey(corpAndNonCorpCharters[1], surveyTaken);

            bool deleteSurveyCopies = adminSurveyListPage.DeleteSurveyCopies(surveyTaken); 
            adminSurveyListPage.ChangeSurveyStatus(originalActiveSurvey, "true");

            // Assert

            Assert.Multiple(() =>
            {
                Assert.IsTrue(CopySurveyAndMakeActiveSuccess[0], "Surveys page did not have the survey \"" + surveyTaken + "\" displayed on the page. ");
                Assert.IsTrue(CopySurveyAndMakeActiveSuccess[1], "Copy Survey function did not complete successfully. ");

                Assert.IsTrue(takeSurvey1[0], "Survey 1: Search For A Credit Union for taking CUDC Survey did not produce a Charter to \"Take Survey\". ");
                Assert.IsTrue(takeSurvey1[1], "Survey 1: Survey form page did not display one or more required items on the page.");
                Assert.IsTrue(takeSurvey1[2], "Survey 1: Save and Review page did not display the Survey Name in big bold letters at the top of page. ");
                Assert.IsTrue(takeSurvey1[3], "Survey 1: Save and Review page did not display the Charter Number the survey was taken for. ");
                Assert.IsTrue(takeSurvey1[4], "Survey 1: Save and Review page did not display the correct answer given when taking the survey. ");
                Assert.IsTrue(takeSurvey1[5], "Survey 1: After submitting survey, popup dialog stating, \"Are you sure you wish to submit this survey\" was not displayed. ");
                Assert.IsTrue(takeSurvey1[6], "Survey 1: After completing survey submission, page did not display, \"Thank you for taking the time to complete our survey.\". ");
                Assert.IsTrue(takeSurvey1[7], "Survey 1: After completing survey submission, page did not display, \"Your data was submitted successfully!\". ");

                Assert.IsTrue(takeSurvey2[0], "Survey 2: Search For A Credit Union for taking CUDC Survey did not produce a Charter to \"Take Survey\". ");
                Assert.IsTrue(takeSurvey2[1], "Survey 2: Survey form page did not display one or more required items on the page.");
                Assert.IsTrue(takeSurvey2[2], "Survey 2: Save and Review page did not display the Survey Name in big bold letters at the top of page. ");
                Assert.IsTrue(takeSurvey2[3], "Survey 2: Save and Review page did not display the Charter Number the survey was taken for. ");
                Assert.IsTrue(takeSurvey2[4], "Survey 2: Save and Review page did not display the correct answer given when taking the survey. ");
                Assert.IsTrue(takeSurvey2[5], "Survey 2: After submitting survey, popup dialog stating, \"Are you sure you wish to submit this survey\" was not displayed. ");
                Assert.IsTrue(takeSurvey2[6], "Survey 2: After completing survey submission, page did not display, \"Thank you for taking the time to complete our survey.\". ");
                Assert.IsTrue(takeSurvey2[7], "Survey 2: After completing survey submission, page did not display, \"Your data was submitted successfully!\". ");

                Assert.IsTrue(deleteSurveyCopies, "Deleting Survey copy used for taking a survey did not complete successfully. ");

            });
        }

        [Test, NcuaTestCaseSource(typeof(CommonUtilities), nameof(CommonUtilities.TestCaseSource))]
        public void CanVerifyDashboard(string environment)
        {
            // Arrange
            testConfig.Environment = environment;

            string systemNtid = "HQNT\\" + Environment.UserName;
            Console.WriteLine("systemNTID = " + systemNtid);

            string[] menuNames = { "Dashboard", "Surveys", "Take CUDC Survey", "Manage Survey", "RADAR View", "Search User" };
            string[] sectionNames = { "SURVEYS", "TAKE CUDC SURVEY", "MANAGE SURVEY", "RADAR VIEW", "SEARCH USER" };

            string menuItemSelected = "Dashboard";

            var dashboardLandingPage = new DashboardLandingPage(TestDriver, "CanVerifyDashboard");

            // Act
            dashboardLandingPage.DoGoToPage();
            
            bool[] verifyDashboardSuccess = dashboardLandingPage.VerifyDashboard(menuNames, sectionNames, systemNtid);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.IsTrue(verifyDashboardSuccess[0], $"CUDC homepage or '{menuItemSelected}' page did not display the '{menuNames[0]}' selection in the menu. ");
                Assert.IsTrue(verifyDashboardSuccess[1], $"CUDC homepage or '{menuItemSelected}' page did not display the '{menuNames[1]}' selection in the menu. ");
                Assert.IsTrue(verifyDashboardSuccess[2], $"CUDC homepage or '{menuItemSelected}' page did not display the '{menuNames[2]}' selection in the menu. ");
                Assert.IsTrue(verifyDashboardSuccess[3], $"CUDC homepage or '{menuItemSelected}' page did not display the '{menuNames[3]}' selection in the menu. ");
                Assert.IsTrue(verifyDashboardSuccess[4], $"CUDC homepage or '{menuItemSelected}' page did not display the '{menuNames[4]}' selection in the menu. ");
                Assert.IsTrue(verifyDashboardSuccess[5], $"CUDC homepage or '{menuItemSelected}' page did not display the '{menuNames[5]}' selection in the menu. ");

                Assert.IsTrue(verifyDashboardSuccess[6], $"CUDC homepage or '{menuItemSelected}' page did not display the '{sectionNames[0]}' section name. ");
                Assert.IsTrue(verifyDashboardSuccess[7], $"CUDC homepage or '{menuItemSelected}' page did not display the '{sectionNames[1]}' section name. ");
                Assert.IsTrue(verifyDashboardSuccess[8], $"CUDC homepage or '{menuItemSelected}' page did not display the '{sectionNames[2]}' section name. ");
                Assert.IsTrue(verifyDashboardSuccess[9], $"CUDC homepage or '{menuItemSelected}' page did not display the '{sectionNames[3]}' section name. ");
                Assert.IsTrue(verifyDashboardSuccess[10], $"CUDC homepage or '{menuItemSelected}' page did not display the '{sectionNames[4]}' section name. ");

                Assert.IsTrue(verifyDashboardSuccess[11], $"CUDC homepage or '{menuItemSelected}' page did not display the '{systemNtid}' username in the top right. ");
                Assert.IsTrue(verifyDashboardSuccess[12], $"CUDC homepage or '{menuItemSelected}' page did not display a version number at the bottom. ");

            });
        }

        [Test, NcuaTestCaseSource(typeof(CommonUtilities), nameof(CommonUtilities.TestCaseSource))]
        public void CanVerifySurveys(string environment)
        {
            // Arrange
            testConfig.Environment = environment;

            string[] columnNames = { "Title", "Type", "Cycle Date", "End Date", "Status", "Edit", "More Actions" };
            string menuItemSelected = "Surveys";

            var dashboardLandingPage = new DashboardLandingPage(TestDriver, "CanVerifySurveys");

            // Act
            dashboardLandingPage.DoGoToPage();
            dashboardLandingPage.SelectFromMenu(menuItemSelected.ToLower());

            var surveysPage = new AdminSurveyListPage(TestDriver, "CanVerifySurveys");
            
            bool[] verifySurveysSuccess = surveysPage.VerifySurveys(columnNames);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.IsTrue(verifySurveysSuccess[0], $"'{menuItemSelected}' page did not display the '{columnNames[0]}' table column header. ");
                Assert.IsTrue(verifySurveysSuccess[1], $"'{menuItemSelected}' page did not display the '{columnNames[1]}' table column header. ");
                Assert.IsTrue(verifySurveysSuccess[2], $"'{menuItemSelected}' page did not display the '{columnNames[2]}' table column header. ");
                Assert.IsTrue(verifySurveysSuccess[3], $"'{menuItemSelected}' page did not display the '{columnNames[3]}' table column header. ");
                Assert.IsTrue(verifySurveysSuccess[4], $"'{menuItemSelected}' page did not display the '{columnNames[4]}' table column header. ");
                Assert.IsTrue(verifySurveysSuccess[5], $"'{menuItemSelected}' page did not display the '{columnNames[5]}' table column header. ");
                Assert.IsTrue(verifySurveysSuccess[6], $"'{menuItemSelected}' page did not display the '{columnNames[6]}' table column header. ");
                Assert.IsTrue(verifySurveysSuccess[7], $"'{menuItemSelected}' page did not display any data in the first row. ");

            });
        }

        [Test, NcuaTestCaseSource(typeof(CommonUtilities), nameof(CommonUtilities.TestCaseSource))]
        public void CanVerifyTakeCudcSurvey(string environment)
        {
            // Arrange
            testConfig.Environment = environment;

            string[] inputFieldNames = { "Name or Charter Number", "Region", "SE", "District", "State", "Address", "City" };
            string[] tableColumns = { "Name", "Charter Number", "Action" };
            string charterNumber = "227";

            string menuItemSelected = "Survey";

            var dashboardLandingPage = new DashboardLandingPage(TestDriver, "CanVerifyTakeCudcSurvey");

            // Act
            dashboardLandingPage.DoGoToPage();
            dashboardLandingPage.SelectFromMenu(menuItemSelected.ToLower());
            menuItemSelected = "Take CUDC " + menuItemSelected;

            var takeSurveyPage = new SearchPage(TestDriver, "CanVerifyTakeCudcSurvey");

            bool[] verifyTakeSurveySuccess = takeSurveyPage.VerifyTakeSurvey(inputFieldNames, tableColumns, charterNumber);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.IsTrue(verifyTakeSurveySuccess[0], $"'{menuItemSelected}' page did not display the '{inputFieldNames[0]}' input field name. ");
                Assert.IsTrue(verifyTakeSurveySuccess[1], $"'{menuItemSelected}' page did not display the '{inputFieldNames[1]}' input field name. ");
                Assert.IsTrue(verifyTakeSurveySuccess[2], $"'{menuItemSelected}' page did not display the '{inputFieldNames[2]}' input field name. ");
                Assert.IsTrue(verifyTakeSurveySuccess[3], $"'{menuItemSelected}' page did not display the '{inputFieldNames[3]}' input field name. ");
                Assert.IsTrue(verifyTakeSurveySuccess[4], $"'{menuItemSelected}' page did not display the '{inputFieldNames[4]}' input field name. ");
                Assert.IsTrue(verifyTakeSurveySuccess[5], $"'{menuItemSelected}' page did not display the '{inputFieldNames[5]}' input field name. ");
                Assert.IsTrue(verifyTakeSurveySuccess[6], $"'{menuItemSelected}' page did not display the '{inputFieldNames[6]}' input field name. ");
                Assert.IsTrue(verifyTakeSurveySuccess[7], $"'{menuItemSelected}' page did not display the '{tableColumns[0]}' table column name after searching with charter {charterNumber}. ");
                Assert.IsTrue(verifyTakeSurveySuccess[8], $"'{menuItemSelected}' page did not display the '{tableColumns[1]}' table column name after searching with charter {charterNumber}. ");
                Assert.IsTrue(verifyTakeSurveySuccess[9], $"'{menuItemSelected}' page did not display the '{tableColumns[2]}' table column name after searching with charter {charterNumber}. ");
                Assert.IsTrue(verifyTakeSurveySuccess[10], $"'{menuItemSelected}' page did not display any data in the first row after searching with charter {charterNumber}. ");

            });
        }

        [Test, NcuaTestCaseSource(typeof(CommonUtilities), nameof(CommonUtilities.TestCaseSource))]
        public void CanVerifyManageSurvey(string environment)
        {
            // Arrange
            testConfig.Environment = environment;

            string[] inputFieldNames = { "Name or Charter Number", "Region", "SE", "District", "State", "Address", "City" };
            string[] tableColumns = { "Name", "Charter Number", "Action" };
            string charterNumber = "227";
            string[] questionFieldNames = { "Credit Union Name: ", "Charter Number: ", "Charter Region: ", "Charter SE group: " };

            string menuItemSelected = "Manage";

            var dashboardLandingPage = new DashboardLandingPage(TestDriver, "CanVerifyManageSurvey");

            // Act
            dashboardLandingPage.DoGoToPage();
            dashboardLandingPage.SelectFromMenu(menuItemSelected.ToLower());
            menuItemSelected = menuItemSelected + " Survey";

            var verifyManageSurveyPage = new ManageSurveyPage(TestDriver, "CanVerifyManageSurvey");

            bool[] verifyManageSurveySuccess = verifyManageSurveyPage.VerifyManageSurvey(inputFieldNames, tableColumns, charterNumber, questionFieldNames);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.IsTrue(verifyManageSurveySuccess[0], $"'{menuItemSelected}' page did not display the '{inputFieldNames[0]}' input field name. ");
                Assert.IsTrue(verifyManageSurveySuccess[1], $"'{menuItemSelected}' page did not display the '{inputFieldNames[1]}' input field name. ");
                Assert.IsTrue(verifyManageSurveySuccess[2], $"'{menuItemSelected}' page did not display the '{inputFieldNames[2]}' input field name. ");
                Assert.IsTrue(verifyManageSurveySuccess[3], $"'{menuItemSelected}' page did not display the '{inputFieldNames[3]}' input field name. ");
                Assert.IsTrue(verifyManageSurveySuccess[4], $"'{menuItemSelected}' page did not display the '{inputFieldNames[4]}' input field name. ");
                Assert.IsTrue(verifyManageSurveySuccess[5], $"'{menuItemSelected}' page did not display the '{inputFieldNames[5]}' input field name. ");
                Assert.IsTrue(verifyManageSurveySuccess[6], $"'{menuItemSelected}' page did not display the '{inputFieldNames[6]}' input field name. ");
                Assert.IsTrue(verifyManageSurveySuccess[7], $"'{menuItemSelected}' page did not display the '{tableColumns[0]}' table column name after searching with charter {charterNumber}. ");
                Assert.IsTrue(verifyManageSurveySuccess[8], $"'{menuItemSelected}' page did not display the '{tableColumns[1]}' table column name after searching with charter {charterNumber}. ");
                Assert.IsTrue(verifyManageSurveySuccess[9], $"'{menuItemSelected}' page did not display the '{tableColumns[2]}' table column name after searching with charter {charterNumber}. ");
                Assert.IsTrue(verifyManageSurveySuccess[10], $"'{menuItemSelected}' page did not display any data in the first row after searching with charter {charterNumber}. ");
                Assert.IsTrue(verifyManageSurveySuccess[11], $"'{menuItemSelected}' page did not display the '{questionFieldNames[0]}' field along with data after searching with charter {charterNumber} and selecting 'CAT Survey' from the first row. ");
                Assert.IsTrue(verifyManageSurveySuccess[12], $"'{menuItemSelected}' page did not display the '{questionFieldNames[1]}' field along with data after searching with charter {charterNumber} and selecting 'CAT Survey' from the first row. ");
                Assert.IsTrue(verifyManageSurveySuccess[13], $"'{menuItemSelected}' page did not display the '{questionFieldNames[2]}' field along with data after searching with charter {charterNumber} and selecting 'CAT Survey' from the first row. ");
                Assert.IsTrue(verifyManageSurveySuccess[14], $"'{menuItemSelected}' page did not display the '{questionFieldNames[3]}' field along with data after searching with charter {charterNumber} and selecting 'CAT Survey' from the first row. ");

            });
        }

        [Test, NcuaTestCaseSource(typeof(CommonUtilities), nameof(CommonUtilities.TestCaseSource))]
        public void CanVerifyRadarView(string environment)
        {
            // Arrange
            testConfig.Environment = environment;

            string[] fieldNames = { "User ID:", "CU Number:", "Cycle Date:" };
            string menuItemSelected = "RADAR-View";

            var dashboardLandingPage = new DashboardLandingPage(TestDriver, "CanVerifyRadarView");

            // Act
            dashboardLandingPage.DoGoToPage();
            dashboardLandingPage.SelectFromMenu(menuItemSelected.ToLower());

            var radarViewPage = new RadarViewPage(TestDriver, "CanVerifyRadarView");

            bool[] verifyRadarViewPageSuccess = radarViewPage.VerifyRadarView(fieldNames);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.IsTrue(verifyRadarViewPageSuccess[0], $"'{menuItemSelected}' page did not display the '{fieldNames[0]}' field names. ");
                Assert.IsTrue(verifyRadarViewPageSuccess[1], $"'{menuItemSelected}' page did not display the '{fieldNames[1]}' field names. ");
                Assert.IsTrue(verifyRadarViewPageSuccess[2], $"'{menuItemSelected}' page did not display the '{fieldNames[2]}' field names. ");
                
            });
        }

        [Test, NcuaTestCaseSource(typeof(CommonUtilities), nameof(CommonUtilities.TestCaseSource))]
        public void CanVerifySearchUser(string environment)
        {
            // Arrange
            testConfig.Environment = environment;

            string[] topTableColumnNames = { "User ID", "Employee #", "First Name", "Last Name", "Email", "View Permissions" };
            string[] bottomTableColumnNames = { "UserID", "Group", "Module", "Action", "Region", "SE Group", "Credit Union" };
            string searchUser = "Tyra";

            string menuItemSelected = "Search-User";

            var dashboardLandingPage = new DashboardLandingPage(TestDriver, "CanVerifySearchUser");

            // Act
            dashboardLandingPage.DoGoToPage();
            dashboardLandingPage.SelectFromMenu(menuItemSelected.ToLower());

            var searchUserPage = new SearchUserPage(TestDriver, "CanVerifySearchUser");

            bool[] verifySearchUserPageSuccess = searchUserPage.VerifySearchUser(topTableColumnNames, bottomTableColumnNames, searchUser);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.IsTrue(verifySearchUserPageSuccess[0], $"'{menuItemSelected}' page did not display the '{topTableColumnNames[0]}' column name in the Search Results table. ");
                Assert.IsTrue(verifySearchUserPageSuccess[1], $"'{menuItemSelected}' page did not display the '{topTableColumnNames[1]}' column name in the Search Results table. ");
                Assert.IsTrue(verifySearchUserPageSuccess[2], $"'{menuItemSelected}' page did not display the '{topTableColumnNames[2]}' column name in the Search Results table. ");
                Assert.IsTrue(verifySearchUserPageSuccess[3], $"'{menuItemSelected}' page did not display the '{topTableColumnNames[3]}' column name in the Search Results table. ");
                Assert.IsTrue(verifySearchUserPageSuccess[4], $"'{menuItemSelected}' page did not display the '{topTableColumnNames[4]}' column name in the Search Results table. ");
                Assert.IsTrue(verifySearchUserPageSuccess[5], $"'{menuItemSelected}' page did not display the '{topTableColumnNames[5]}' column name in the Search Results table. ");

                Assert.IsTrue(verifySearchUserPageSuccess[6], $"'{menuItemSelected}' page did not display the '{bottomTableColumnNames[0]}' column name in the View Permissions table. ");
                Assert.IsTrue(verifySearchUserPageSuccess[7], $"'{menuItemSelected}' page did not display the '{bottomTableColumnNames[1]}' column name in the View Permissions table. ");
                Assert.IsTrue(verifySearchUserPageSuccess[8], $"'{menuItemSelected}' page did not display the '{bottomTableColumnNames[2]}' column name in the View Permissions table. ");
                Assert.IsTrue(verifySearchUserPageSuccess[9], $"'{menuItemSelected}' page did not display the '{bottomTableColumnNames[3]}' column name in the View Permissions table. ");
                Assert.IsTrue(verifySearchUserPageSuccess[10], $"'{menuItemSelected}' page did not display the '{bottomTableColumnNames[4]}' column name in the View Permissions table. ");
                Assert.IsTrue(verifySearchUserPageSuccess[11], $"'{menuItemSelected}' page did not display the '{bottomTableColumnNames[5]}' column name in the View Permissions table. ");
                Assert.IsTrue(verifySearchUserPageSuccess[12], $"'{menuItemSelected}' page did not display the '{bottomTableColumnNames[6]}' column name in the View Permissions table. ");

                Assert.IsTrue(verifySearchUserPageSuccess[13], $"'{menuItemSelected}' page did not display any data in the first row of the View Permissions table after selecting 'View Permissions' link from Search Results table. ");
            });
        }



    }
}