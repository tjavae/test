import { Component, OnInit } from '@angular/core';
import { SearchRequest } from 'src/app/models/search/search-request';
import { SearchResult } from 'src/app/models/search/search-result';
import { SelectListItem } from 'src/app/models/select-list-item';
import { SearchService } from 'src/app/services/search.service';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthenticationService } from '../../services/authentication.service';
import { SurveyService } from 'src/app/services/survey.service';
import { SurveyType } from 'src/app/models/survey/survey-type';
import { GroupName, Module } from 'src/app/common/constants';
import { AdminService } from 'src/app/services/admin.service';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent implements OnInit {
  showSearchForm: boolean = true;
  showSearchResults: boolean = false;
  showNotFound: boolean = false;
  states: SelectListItem[];
  model = new SearchRequest();
  searchResults: SearchResult[];
  surveyActive: boolean = false;
  editable: boolean = false;
  manageCatSeDos: boolean = false;
  cycleDate: string;
  cycleDates;

  constructor(
    private searchSvc: SearchService, 
    private authSvc: AuthenticationService, 
    private router: Router, 
    private route: ActivatedRoute, 
    private adminSvc: AdminService,
    private surveySvc: SurveyService) {
  }

  ngOnInit(): void {
    //Start init process...    
    this.manageCatSeDos = window.location.pathname.indexOf('/admin/manage') !== -1;    
    this.checkIfSurveyIsActive();    
  }

  checkIfSurveyIsActive(): void {
    if (this.manageCatSeDos) {
      this.surveyActive = true;
      this.checkRolesAndPermissions();      
    } else {
      this.surveySvc.isSurveyActive(SurveyType.CUDC).subscribe(isActive => {
        this.surveyActive = isActive;     
        //Proceed next, only if survey is active
        if (isActive)
          this.checkRolesAndPermissions();
      });
    }
  }

  checkRolesAndPermissions(): void {
    this.authSvc.getUser().subscribe(userRole => {
      if (userRole){
        let testerView = userRole.permissions.find(p => p.groupName == GroupName.TESTER && p.module == Module.SURVEYS_MANAGEMENT);
        let adminView = userRole.permissions.find(p => p.groupName == GroupName.ADMIN && p.module == Module.SURVEYS_MANAGEMENT);
        this.searchSvc.getStates().subscribe(states => {
          this.states = states;
        });
        this.adminSvc.getCycleDates().subscribe(cDates => {
          if (cDates) { this.cycleDates = cDates; }
        });
        if (!adminView && !testerView){ 
          this.router.navigate(['/survey']);
        }
      } else {
        this.router.navigate(['/survey/message', 1], { relativeTo: this.route });
      }
    }, (error) => {
        this.router.navigate(['/survey/message', 1], { relativeTo: this.route });  
        return;
    });
  }

  search(): void {
    this.searchSvc.getSearchResults(this.model).subscribe(searchResults => {
      if (searchResults == null || searchResults.length == 0) {
        this.showSearchForm = false;
        this.showNotFound = true;
      } else {
        this.searchResults = searchResults;
        this.showSearchForm = false;
        this.showSearchResults = true;
      }
    });
  }

  reset(): void {
    this.showSearchForm = true;
    this.showSearchResults = false;
    this.showNotFound = false;
  }

  resetCuSearch(): void {
    this.showNotFound = true;
    this.showSearchResults = false;
    this.showNotFound = false;
    Object.keys(this.model).forEach(key => this.model[key] = null);    
  }
}
