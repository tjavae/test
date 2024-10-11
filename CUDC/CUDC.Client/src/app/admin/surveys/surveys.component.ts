import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { GroupName, Module } from 'src/app/common/constants';
import { UserRole } from 'src/app/models/auth/user-role';
import { Survey } from 'src/app/models/survey/survey';
import { SurveyType } from 'src/app/models/survey/survey-type';
import { AdminService } from 'src/app/services/admin.service';
import { AuthenticationService } from 'src/app/services/authentication.service';

declare const $: any;

@Component({
  selector: 'app-surveys',
  templateUrl: './surveys.component.html',
  styleUrls: ['./surveys.component.scss']
})
export class SurveysComponent implements OnInit {
  surveys: Survey[]; 
  loading = true;
  surveyTypes = SurveyType;
  modalTitle: string;
  isEdit: boolean;
  selectedId: string;
  survey: Survey;
  isTester: boolean;
  preEditTitle: string;
  preEditType:number;
  preEditIsActive: boolean;
  preEditStartDate: Date;
  preEditEndDate: Date;
  preEditDescription: string;
  preEditIInformationText: string;  
  currentUser: UserRole;

  constructor(
    private adminSvc: AdminService, 
    private datePipe: DatePipe,
    private authSvc: AuthenticationService,
    private router: Router,
    private route: ActivatedRoute,) {
  }

  ngOnInit(): void {
    this.authSvc.getUser().subscribe(userRole => {
      if (userRole){
        this.currentUser = userRole;        
        let testerView = userRole.permissions.find(p => p.groupName == GroupName.TESTER && p.module == Module.SURVEYS_MANAGEMENT);
        if (testerView || userRole.employeeNumber == '99999') {
          this.isTester = true;
        }
        let adminView = userRole.permissions.find(p => p.groupName == GroupName.ADMIN && p.module == Module.SURVEYS_MANAGEMENT);
        if (adminView || testerView){         
          this.adminSvc.getSurveys().subscribe(surveys => {
            if (this.isTester && !adminView) {              
              this.surveys = surveys.filter(s => s.type == 0);
            } else {
              this.surveys = surveys;
            }            
            this.loading = false;            
          });
        } else {
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

  formatDate(date: any): string {
    try {
      return (date ? this.datePipe.transform(date, 'MM/dd/yyyy') : '');
    } catch (ex) {
      return '';
    }
  }

  onCreate(): void {
    this.modalTitle = 'Create New Survey';
    this.isEdit = false;
    this.survey = new Survey();    
    $('#create-or-edit form').removeClass('was-validated').addClass('needs-validation');
    $('#create-or-edit').modal();    
    //*** Needs a small delay for this to work ***
    setTimeout(function () {
      $('#start-date-field').datepicker();
      $('#end-date-field').datepicker();
    }, 500);
  }

  onEdit(id: string): void {    
    this.modalTitle = 'Update Survey';
    this.isEdit = true;
    this.selectedId = id;
    this.survey = this.surveys.find(x => x.id === id);    
    this.preEditTitle =  this.survey.title; 
    this.preEditType =  this.survey.type;
    this.preEditIsActive =  this.survey.isActive;
    this.preEditStartDate =  this.survey.startDate;   
    this.preEditEndDate =  this.survey.endDate;
    this.preEditDescription =  this.survey.description;
    this.preEditIInformationText =  this.survey.informationText;

    $('#create-or-edit form').removeClass('was-validated').addClass('needs-validation');
    $('#create-or-edit').modal();
    
    //*** Needs a small delay for this to work ***
    setTimeout(function () {
      $('#start-date-field').datepicker();
      $('#end-date-field').datepicker();
    }, 500);
  }

  onCancel(): void{   
    if (this.isEdit)
    { 
      this.survey.title = this.preEditTitle; 
      this.survey.type = this.preEditType;
      this.survey.isActive = this.preEditIsActive;
      this.survey.startDate = this.preEditStartDate;      
      this.survey.endDate = this.preEditEndDate;
      this.survey.description = this.preEditDescription;
      this.survey.informationText = this.preEditIInformationText;      
    }
  }

  onCreateOrEdit(): void {     
    
    if (this.isEdit) { 
      let foundSurvey = this.surveys.filter(s => s.title === this.survey.title && s.title !== this.preEditTitle);      
      if (foundSurvey.length > 1){         
        alert("Survey Title is existing. Please enter a difference title.");
        this.onCancel(); 
        $('#create-or-edit form').addClass('was-validated');
        return;
      };
    } else {
      let foundSurvey = this.surveys.filter(s => s.title === this.survey.title);      
      if (foundSurvey.length > 0){         
        alert("Survey Title is existing. Please enter a difference title.");
        this.survey.title="";
        $('#create-or-edit form').addClass('was-validated');
        return;
      };
    };    

    if ($('#create-or-edit form')[0].checkValidity() === false) {      
      $('#create-or-edit form').addClass('was-validated'); 
      return;      
    }

    $('#create-or-edit').modal('toggle'); 
    this.survey.type = parseInt(<any>this.survey.type);
    this.survey.isActive = `${this.survey.isActive}` == 'true';
    this.survey.startDate = ($('#start-date-field').val() ? $('#start-date-field').val() : null);
    this.survey.endDate = ($('#end-date-field').val() ? $('#end-date-field').val() : null);
    this.survey.startDate = new Date(Date.parse(<any>this.survey.startDate));
    this.survey.endDate = new Date(Date.parse(<any>this.survey.endDate));
    $('#spinner').modal({backdrop: 'static', keyboard: false});
    if (this.isEdit) { 
      this.adminSvc.updateSurvey(this.survey).subscribe(survey => {      
          this.updateActiveInactive(survey);          
      });
    } else {      
      this.adminSvc.createSurvey(this.survey).subscribe(survey => {
        this.surveys.push(survey);
        this.surveys.sort((a,b) => a.title.localeCompare(b.title));
        this.updateActiveInactive(survey);        
      });
    };
    window.location.reload();  
    $('#spinner').modal('hide');
  }

  updateActiveInactive(survey: Survey): void {
    if (survey.isActive) {
      this.surveys.filter(x => x.id !== survey.id && x.type === survey.type && x.isActive).forEach(x => {
        x.isActive = false;
      });
    }
  }

  onCopy(id: string): void {    
    this.selectedId = id;    
    $('#copy-confirmation').modal();
  }

  onCopyConfirmation(): void {
    $('#spinner').modal({backdrop: 'static', keyboard: false}); 
    this.adminSvc.copySurvey(this.selectedId).subscribe(() => {
      window.location.reload();
      $('#spinner').modal('hide');
    });
   
  }

  onDelete(id: string): void {    
    this.selectedId = id;
    let currentSurvey = this.surveys.find(s => s.id === id );   
    if (currentSurvey.createdBy.includes(this.currentUser.userId) || currentSurvey.createdBy.includes('AutomatedTestUser01')){
      $('#delete-confirmation').modal();
    }
    else
    {
      $('#delete-cancel').modal();
    }
  }

  onDeleteConfirmation(): void { 
    $('#spinner').modal({backdrop: 'static', keyboard: false});
    this.adminSvc.deleteSurvey(this.selectedId).subscribe(() => { 
      window.location.reload();  
      $('#spinner').modal('hide');    
    });
  }
}
