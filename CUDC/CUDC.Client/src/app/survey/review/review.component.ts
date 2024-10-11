import { AfterViewInit, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Action, GroupName, Module, SurveyStatus } from 'src/app/common/constants';
import { CharterInfo } from 'src/app/models/auth/charter-info';
import { UserRole } from 'src/app/models/auth/user-role';
import { _Question, _Section } from 'src/app/models/survey/internal';
import { QuestionType } from 'src/app/models/survey/question-type';
import { SurveyInfo } from 'src/app/models/survey/survey-info';
import { SurveyOwnerResponse } from 'src/app/models/survey/survey-owner-response';
import { SurveyType } from 'src/app/models/survey/survey-type';
import { AdminService } from 'src/app/services/admin.service';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { SearchService } from 'src/app/services/search.service';
import { SecurityService } from 'src/app/services/security.service';
import { SurveyService } from 'src/app/services/survey.service';

declare const $: any;

@Component({
  selector: 'app-review',
  templateUrl: './review.component.html',
  styleUrls: ['./review.component.scss']
})
export class ReviewComponent implements OnInit, AfterViewInit {
  surveyInfo: SurveyInfo;
  mySurveyInfo: SurveyInfo;
  sections: _Section[];
  catReview = false;
  seReview = false;
  dosReview = false;
  isSubmitted = false;
  surveyOwner: SurveyOwnerResponse = null;
  userRole: UserRole;
  charterInfo: CharterInfo;
  messageId = 8;
  surveyType: SurveyType = SurveyType.Undefined;
  surveyStatus = 'Has not been taken'; //Saved//Submitted
  surveyManagementView = false;
  adminReView = false;
  canGoBack = false;
  canSubmit = false;
  extractUsers;
  selectedUser;
  prevent = false;
  cudcReview = false;
  isClamed = false;
  cycleDate: string;
  region: number;
  ncuaUsers: UserRole[];
  loading = true;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private surveySvc: SurveyService,
    private securitySvc: SecurityService,
    private authSvc: AuthenticationService,
    private searchSvc: SearchService,
    private adminSvc: AdminService) {
  }

  ngOnInit(): void {
    this.catReview = window.location.pathname.indexOf('/cat/review') !== -1;
    this.seReview = window.location.pathname.indexOf('/se/review') !== -1;
    this.dosReview = window.location.pathname.indexOf('/dos/review') !== -1;
    this.adminReView = window.location.pathname.indexOf('/admin/manage') !== -1;
    this.cudcReview = window.location.pathname.indexOf('/cudc/review') !== -1;
    if (this.catReview) {
      this.surveyType = SurveyType.CAT;
    } else if (this.seReview) {
      this.surveyType = SurveyType.SE;
    } else if (this.dosReview) {
      this.surveyType = SurveyType.DOS;
    } else if (this.cudcReview) {
      this.surveyType = SurveyType.CUDC;
    } else {
      this.surveyType = SurveyType.CUDC;
    }
    this.route.paramMap.subscribe(params => {
      let charterNumber: number = parseInt(params.get('charterNumber'));
      let cycleDate: string = params.get('cycleDate');
      this.cycleDate = cycleDate;
      //Check if one of RADAR
      if (this.catReview || this.seReview || this.dosReview) {
        this.initRADAR(charterNumber, cycleDate);
      } else {
        this.initCUDC(charterNumber);
      }
    });
  }

  initCUDC(charterNumber: number): void {
    this.surveySvc.getSurveyInfo(charterNumber, this.surveyType).subscribe(surveyInfo => {
      this.surveyInfo = surveyInfo;
      this.loading = false;
      this.authSvc.getUser().subscribe(userRole => {
        this.userRole = userRole;
        if (userRole === null) {
          this.router.navigate(['/survey/message', 3], { relativeTo: this.route });
        } else {
          this.securitySvc.getSurveyOwner(this.surveyInfo.survey.id, charterNumber).subscribe(surveyOwner => {
            this.surveyOwner = surveyOwner;
            if (surveyOwner.hasOwner) {
              if (this.surveyInfo.answers != null) {
                if (this.surveyInfo.answers.submittedOn === null) {
                  this.surveyStatus = SurveyStatus.SAVED;
                  this.canSubmit = surveyOwner.ownerIsMe;
                  this.canGoBack = this.canSubmit;
                } else {
                  this.isSubmitted = true;
                  this.surveyStatus = SurveyStatus.SUBMITTED;
                }
              }
            } else {
              this.canSubmit = this.surveyStatus != SurveyStatus.NOT_TAKE;
            }
          });
          this.sections = this.surveySvc.getSections(surveyInfo);
          this.prevent = this.preventSumbit(this.sections);
        }
      })
    });
  }

  initRADAR(charterNumber: number, cycleDate: string): void {
    this.authSvc.getRadarUser().subscribe(userRole => {    // if user allowed to take survey
      this.userRole = userRole;
      this.loading = false;
      if (!userRole) {
        this.router.navigate(['/survey/message', 3], { relativeTo: this.route });
        return;
      }

      this.securitySvc.isClaimedCu(charterNumber, userRole.userId).subscribe(isClaimed => {
        this.isClamed = isClaimed;
      });

      this.surveySvc.getSurveyInfoByCycle(charterNumber, this.surveyType, this.cycleDate).subscribe(surveyInfo => {
        this.surveyInfo = surveyInfo;

        this.searchSvc.getCuInformation(charterNumber).subscribe({
          next: (charterInfo) => {
            this.charterInfo = charterInfo;
            this.region = parseInt(this.charterInfo.region); //1
            this.messageId = 8; //Default value is 8 -- No access

            let surveyManagements = this.userRole.permissions.filter(p => (
              //p.groupName == (GroupName.ADMIN || GroupName.TESTER)
              p.module == Module.SURVEYS_MANAGEMENT)
            );
            if (surveyManagements.length > 0 && this.adminReView) {
              this.surveyManagementView = true;
            }
            //CAT view frame or CAT review url ../cat/review/cu#   
            if (this.catReview) {
              let viewPermissionAll = this.userRole.permissions.filter(p => (
                p.action == Action.VIEW
                && (p.groupName == GroupName.DOS_DOT_RSM || p.groupName == GroupName.SURVEY_VIEWER)
                && p.module == Module.CAT_SURVEY)
              );
              if (viewPermissionAll.length > 0) {
                this.messageId = 0;
              } else { //Not found "SURVEY VIEWER" permission
                let viewCatBySeAndRegion = this.userRole.permissions.filter(p => (
                  p.action == Action.VIEW
                  && p.groupName == GroupName.DISTRICT_EXAMINER
                  && p.module == Module.CAT_SURVEY)
                );
                let viewCatByRegion = this.userRole.permissions.filter(p => (
                  p.action == Action.VIEW
                  && (p.groupName == GroupName.SUPERVISORY_EXAMINER
                    || p.groupName == GroupName.PCO
                    || p.groupName == GroupName.DSA_SPECIALIST)
                  && p.module == Module.CAT_SURVEY)
                );

                if (viewCatByRegion.length > 0) { //Check region exists in collection
                  if (viewCatByRegion.filter(p => p.region == parseInt(this.charterInfo.region)).length > 0) {
                    this.messageId = 0;
                  } else if (viewCatBySeAndRegion.length > 0) {
                    if (viewCatBySeAndRegion.filter(p => p.se == this.charterInfo.se).length > 0) {
                      if (viewCatBySeAndRegion.filter(p => p.region == parseInt(this.charterInfo.region)).length > 0) {
                        this.messageId = 0;
                      } else {
                        this.messageId = 4; //Member missing permission to the Credit Union based on member region access
                      }
                    }
                    else { //redirect to no access page, edit permission only limited to their own se)
                      this.messageId = 5; // Not permitted to access a particular se
                    }
                  } else {
                    //This message will redirect to no access page, view permission only limited to their own region
                    this.messageId = 4; //Member missing permission to the Credit Union based on member region access
                  }

                }
                else { //Not found 'SUPERVISORY EXAMINER' and 'DOS/DOT/RSM' and 'PCO' and 'DSA/SPECIALIST')                
                  if (viewCatBySeAndRegion.length > 0) { //Check if found DISTRICT EXAMINER (vew permission SE group & Region)
                    if (viewCatBySeAndRegion.filter(p => p.se == this.charterInfo.se).length > 0) {
                      if (viewCatBySeAndRegion.filter(p => p.region == parseInt(this.charterInfo.region)).length > 0) {
                        this.messageId = 0;
                      } else {
                        this.messageId = 4; //Member missing permission to the Credit Union based on member region access
                      }
                    }
                    else { //redirect to no access page, edit permission only limited to their own se)
                      this.messageId = 5; // Not permitted to access a particular se
                    }
                  } else { //Not found 'DISTRICT EXAMINER' either                  
                    this.messageId = 8; // You do not have permission to view and edit the credit union.
                  }
                }
              }
              if (this.messageId != 0 && !this.surveyManagementView) {
                if (cycleDate == null) {
                  this.router.navigate(['../../message', this.messageId], { relativeTo: this.route });
                  return;
                } else {
                  this.router.navigate(['../../../message', this.messageId], { relativeTo: this.route });
                  return;
                }
              }

              //SE view frame RADAR View or SE review url ../se/review/cu#
            } else if (this.seReview) {
              if (this.surveyInfo.hasPreSubmitedSurvey) {
                let viewPermissionAll = this.userRole.permissions.filter(p => (
                  p.action == Action.VIEW
                  && (p.groupName == GroupName.DOS_DOT_RSM || p.groupName == GroupName.SURVEY_VIEWER)
                  && p.module == Module.SE_SURVEY)
                );
                if (viewPermissionAll.length > 0) {
                  this.messageId = 0;
                } else { //Not found "SURVEY VIEWER" permission
                  let viewPermissionSe = this.userRole.permissions.filter(p => (
                    p.action == Action.VIEW
                    && p.groupName == GroupName.DISTRICT_EXAMINER)
                  );
                  let viewPermissonRegion = this.userRole.permissions.filter(p => (
                    p.action == Action.VIEW
                    && (p.groupName == GroupName.SUPERVISORY_EXAMINER
                      || p.groupName == GroupName.PCO
                      || p.groupName == GroupName.DSA_SPECIALIST)
                    && p.module == Module.SE_SURVEY)
                  );
                  if (viewPermissonRegion.length > 0) { //Check region exists in collection
                    if (viewPermissonRegion.filter(p => p.region == parseInt(this.charterInfo.region)).length > 0) {
                      this.messageId = 0;
                    } else if (viewPermissionSe.length > 0) {
                      this.messageId = this.isClamed ? 0 : 6;  // View claimed credit union only
                    } else { //This message will redirect to no access page, view permission only limited to their own region
                      this.messageId = 4; //Member missing permission to the Credit Union based on member region access
                    }
                  } else { //Not found SUPERVISORY EXAMINER' or 'DOS/DOT/RSM' or 'PCO' or 'DSA/SPECIALIST')                
                    if (viewPermissionSe.length > 0) { //Check if found DISTRICT EXAMINER 
                      this.messageId = this.isClamed ? 0 : 6;  // View claimed credit union only
                    } else { //Not found 'DISTRICT EXAMINER' either
                      this.messageId = 8; //Direct to no access page, no system access
                    }
                  }
                }
              } else {
                this.messageId = 11;
              }

              if (this.messageId != 0 && !this.surveyManagementView) {
                if (cycleDate == null) {
                  this.router.navigate(['../../message', this.messageId], { relativeTo: this.route });
                  return;
                } else {
                  this.router.navigate(['../../../message', this.messageId], { relativeTo: this.route });
                  return;
                }
              }

              //DOS view frame RADAR View or DOS review url ../dos/review/cu#
            } else if (this.dosReview) {
              let viewPermissionAll = this.userRole.permissions.filter(p => (
                p.action == Action.VIEW
                && (p.groupName == GroupName.DOS_DOT_RSM || p.groupName == GroupName.SURVEY_VIEWER)
                && p.module == Module.DOS_SURVEY)
              );
              if (viewPermissionAll.length > 0) {
                this.messageId = 0;
              } else { //Not found "SURVEY VIEWER" permission 
                let viewPermissonRegion = this.userRole.permissions.filter(p => (
                  p.action == Action.VIEW
                  && (p.groupName == GroupName.PCO
                    || p.groupName == GroupName.DSA_SPECIALIST)
                  && p.module == Module.DOS_SURVEY)
                );
                let viewPermissionClaimedDosCu = this.userRole.permissions.filter(p => (
                  p.action == Action.VIEW
                  && (p.groupName == GroupName.DISTRICT_EXAMINER
                    || p.groupName == GroupName.SUPERVISORY_EXAMINER))
                );
                if (viewPermissonRegion.length > 0) { //Check region exists in collection
                  if (viewPermissonRegion.filter(p => p.region == parseInt(this.charterInfo.region)).length > 0) {
                    this.messageId = 0;
                  } else if (viewPermissionClaimedDosCu.length > 0) {
                    this.messageId = this.isClamed ? 0 : 6;  // View claimed credit union only
                  } else { //This message will redirect to no access page, view permission only limited to their own region
                    this.messageId = 4; //Member missing permission to the Credit Union based on member region access
                  }
                } else { //Not found 'DOS/DOT/RSM' or 'PCO' or 'DSA/SPECIALIST')
                  if (viewPermissionClaimedDosCu.length > 0) { //Check if found DISTRICT EXAMINER or SUPERVISORY EXAMINER
                    this.messageId = this.isClamed ? 0 : 6;  // View claimed credit union only
                  } else { //Not found 'DISTRICT EXAMINER' and 'SUPERVISORY EXAMINER' either
                    this.messageId = 8; //Direct to no access page, no system access
                  }
                }
              }
              if (this.messageId != 0 && !this.surveyManagementView) {
                if (cycleDate == null) {
                  this.router.navigate(['../../message', this.messageId], { relativeTo: this.route });
                  return;
                } else {
                  this.router.navigate(['../../../message', this.messageId], { relativeTo: this.route });
                  return;
                }
              }
            }

            this.securitySvc.getSurveyOwner(this.surveyInfo.survey.id, this.surveyInfo.basicInfo.charterNumber).subscribe(surveyOwner => {
              this.surveyOwner = surveyOwner;
              if (surveyOwner.hasOwner) {
                if (this.surveyInfo.answers != null) {
                  if (this.surveyInfo.answers.submittedOn === null) {
                    this.surveyStatus = SurveyStatus.SAVED;
                    this.canSubmit = surveyOwner.ownerIsMe && !this.surveyManagementView;
                    this.canGoBack = this.canSubmit;
                  } else {
                    this.isSubmitted = true;
                    this.surveyStatus = SurveyStatus.SUBMITTED;
                  }
                }
              } else {
                this.canSubmit = !this.surveyManagementView && (this.surveyStatus != SurveyStatus.NOT_TAKE);
              }
            });
            this.sections = this.surveySvc.getSections(surveyInfo);
            this.prevent = this.preventSumbit(this.sections);
          },
          error: (error) => {
            this.messageId = 7;
            if (cycleDate == null) {
              this.router.navigate(['../../message', this.messageId], { relativeTo: this.route });
            } else {
              this.router.navigate(['../../../message', this.messageId], { relativeTo: this.route });
            }
          }
        });

      });
    });
  }

  ngAfterViewInit(): void {
    if (this.catReview || this.seReview || this.dosReview) {
      setTimeout(() => {
        $('#back-button').css('display', 'block');
      }, 1500);
    } else {

      window.onscroll = function () {
        if (document.body.scrollTop > 250 || document.documentElement.scrollTop > 250) {
          $('#back-button').css('display', 'block');
        } else {
          $('#back-button').css('display', 'none');
        }
      };
    }
  }

  getAnswerText(question: _Question): string {
    if (question.type === QuestionType.DropDownList) {
      return question.options.find(x => x.id === question.answer).text;
    } else {
      return question.answer;
    }
  }

  preventSumbit(sections: _Section[]): boolean {
    sections.forEach(sec => {
      let questions = sec.questions;
      let found = questions.filter(q => q.isRequired && (q.answer == null));
      if (found != null && found.length > 0) {
        this.prevent = true;
      }
    });
    return this.prevent;
  }

  submit(): void {
    this.surveySvc.submitAnswers(this.surveyInfo.answers.id).subscribe(() => {
      if (this.catReview || this.seReview || this.dosReview) {
        this.messageId = 9; //Thank you for taking the time to complete our survey
        if (this.cycleDate == null) {
          this.router.navigate(['../../message', this.messageId], { relativeTo: this.route });
          return;
        } else {
          this.router.navigate(['../../../message', this.messageId], { relativeTo: this.route });
          return;
        }
      } else {
        //For CUDC surveys display finish page instead
        this.router.navigate(['../../finish'], { relativeTo: this.route });
      }
      this.surveyStatus = 'Submitted';
    });
  }

  onSubmit(): void {
    let frame = window.sessionStorage.getItem('frameCatSeDos');
    if (frame == 'YES') {
      return;
    } else if (this.prevent) {
      $('#required-fields-not-meet').modal();
    } else {
      $('#submit-confirm').modal();
    }
  }

  onUnlock(): void {
    $('#unlock-confirm').modal();
  }

  unlock(): void {
    this.surveySvc.unlockSurvey(this.surveyInfo.answers.id).subscribe(() => {
      this.isSubmitted = false;
      this.surveyStatus = 'Saved';
    });
  }

  onTransferOwner(): void {
    $('#spinner').modal({ backdrop: 'static', keyboard: false });
    this.securitySvc.getNcuaUsers(this.region, this.surveyType).subscribe(users => {
      if (users) {
        this.extractUsers = users.map(({ userId, firstName, lastName }) => ({ userId, firstName, lastName }));
        $('#spinner').modal('hide');
        $('#transfer-ownership form').removeClass('was-validated').addClass('needs-validation');
        $('#transfer-ownership').modal();
      }
    });
  }

  transferOwner(): void {
    if ($('#transfer-ownership form')[0].checkValidity() === false) {
      $('#transfer-ownership form').addClass('was-validated');
      return;
    } else if (this.selectedUser && this.selectedUser.userId) {
      this.surveySvc.transferOwnership(this.surveyInfo.answers.id, this.selectedUser.userId).subscribe(() => {
        this.surveyOwner.firstName = this.selectedUser.firstName;
        this.surveyOwner.lastName = this.selectedUser.lastName;
        $("#transfer-ownership").modal("hide");
      });
    }
    $('#atransfer-ownership').modal('toggle');
  }

  goBack(): void {
    let frame = window.sessionStorage.getItem('frameCatSeDos');
    if (frame == 'YES') {
      return;
    } else {
      window.sessionStorage.setItem('goBack', 'YES');
      if (this.cycleDate == null) {
        this.router.navigate(['../../take', this.surveyInfo.basicInfo.charterNumber], { relativeTo: this.route });
        return;
      } else {
        this.router.navigate(['../../../take', this.surveyInfo.basicInfo.charterNumber], { relativeTo: this.route });
        return;
      }
    }
  }
}
