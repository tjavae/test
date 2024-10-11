import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Action, GroupName, Module } from 'src/app/common/constants';
import { CharterInfo } from 'src/app/models/auth/charter-info';
import { UserRole } from 'src/app/models/auth/user-role';
import { Answer } from 'src/app/models/survey/answer';
import { _Section } from 'src/app/models/survey/internal';
import { QuestionType } from 'src/app/models/survey/question-type';
import { Response } from 'src/app/models/survey/response';
import { SurveyInfo } from 'src/app/models/survey/survey-info';
import { SurveyOwnerResponse } from 'src/app/models/survey/survey-owner-response';
import { SurveyType } from 'src/app/models/survey/survey-type';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { SearchService } from 'src/app/services/search.service';
import { SecurityService } from 'src/app/services/security.service';
import { SurveyService } from 'src/app/services/survey.service';

declare const $: any;

@Component({
  selector: 'app-take',
  templateUrl: './take.component.html',
  styleUrls: ['./take.component.scss']
})
export class TakeComponent implements OnInit, AfterViewInit {
  surveyInfo: SurveyInfo;
  sections: _Section[];
  formGroup: FormGroup;
  questionType = QuestionType;
  catTake = false;
  seTake = false;
  dosTake = false;
  isSubmitted = false;
  surveyOwner: SurveyOwnerResponse = null;
  userRole: UserRole;
  charterInfo: CharterInfo;
  messageId = 8;
  surveyType: SurveyType = SurveyType.Undefined;
  cycleDate: string;
  charterNumber: number;
  selectedRAnswer: string;
  loading = true;
  isClamed = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private surveySvc: SurveyService,
    private securitySvc: SecurityService,
    private authSvc: AuthenticationService,
    private searchSvc: SearchService) {
  }

  updateSurveyType(catTake: boolean, seTake: boolean, dosTake: boolean): void {
    if (catTake) {
      this.surveyType = SurveyType.CAT;
    } else if (seTake) {
      this.surveyType = SurveyType.SE;
    } else if (dosTake) {
      this.surveyType = SurveyType.DOS;
    } else {
      this.surveyType = SurveyType.CUDC;
    }
  }

  ngOnInit(): void {
    this.catTake = window.location.pathname.indexOf('/cat/take') !== -1;
    this.seTake = window.location.pathname.indexOf('/se/take') !== -1;
    this.dosTake = window.location.pathname.indexOf('/dos/take') !== -1;
    this.updateSurveyType(this.catTake, this.seTake, this.dosTake);
    this.route.paramMap.subscribe(params => {
      let charterNumber: number = parseInt(params.get('charterNumber'));
      let cycleDate: string = params.get('cycleDate');
      this.cycleDate = cycleDate;
      if (this.catTake || this.seTake || this.dosTake) {
        this.initRADAR(charterNumber, cycleDate);
      } else {
        this.initCUDC(charterNumber, cycleDate);
      }
    });
  }

  initCUDC(charterNumber: number, cycleDate: string): void {
    this.surveySvc.getSurveyInfo(charterNumber, SurveyType.CUDC).subscribe(surveyInfo => {
      this.surveyInfo = surveyInfo;
      this.charterNumber = charterNumber;
      this.loading = false;
      this.sections = this.surveySvc.getSections(surveyInfo);
      this.securitySvc.getSurveyOwner(this.surveyInfo.survey.id, charterNumber).subscribe({
        next: (surveyOwner) => {
          this.surveyOwner = surveyOwner;
          this.isSubmitted = this.surveyInfo.answers?.submittedOn !== null;  // check if submitted          
          if ((surveyOwner.hasOwner && surveyOwner.ownerIsMe && this.isSubmitted) || (surveyOwner.hasOwner && !surveyOwner.ownerIsMe)) {
            this.navigateToReview();
          }
        },
        error: (error) => {
          this.navigateToReview();
        }
      });
      this.initFormGroup();
    });
  }

  initRADAR(charterNumber: number, cycleDate: string): void {
    this.charterNumber = charterNumber;

    this.authSvc.getRadarUser().subscribe({
      next: (userRole) => {
        // if user allowed to take survey
        this.userRole = userRole;
        this.loading = false;
        if (userRole === null) {
          this.router.navigate(['/survey/message', 3], { relativeTo: this.route });
          return;
        }
        if (!this.validateCyleDate(cycleDate)) {
          this.router.navigate(['/survey/message', 10], { relativeTo: this.route });
          return;
        }

        this.securitySvc.isClaimedCu(charterNumber, userRole.userId).subscribe(isClaimed => {
          this.isClamed = isClaimed;
        });

        this.surveySvc.getSurveyInfoByCycle(charterNumber, this.surveyType, cycleDate).subscribe(surveyInfo => {
          this.surveyInfo = surveyInfo;
          // can't find survey
          if (!surveyInfo) {
            this.router.navigate(['/survey/message', 10], { relativeTo: this.route });
            return;
          }


          // Check if survey is not active, view survey only 
          if (!surveyInfo.survey.isActive) {
            this.navigateToReview();
            return;
          }

          this.securitySvc.getSurveyOwner(this.surveyInfo.survey.id, charterNumber).subscribe({
            next: (surveyOwner) => {
              this.surveyOwner = surveyOwner;
              this.isSubmitted = this.surveyInfo.answers?.submittedOn !== null;  // check if submitted
              if (surveyOwner.hasOwner) {
                let frameCatSeDos = window.sessionStorage.getItem('frameCatSeDos');
                let goBack = window.sessionStorage.getItem('goBack');
                if (frameCatSeDos === 'YES' || this.isSubmitted || goBack !== 'YES') {
                  this.navigateToReview();
                  return;               
                } else {
                  window.sessionStorage.setItem('goBack', 'NO');
                }
              }
            },
            error: (error) => {
              this.navigateToReview();
            }
          });

          this.sections = this.surveySvc.getSections(surveyInfo);

          this.searchSvc.getCuInformation(charterNumber).subscribe({
            next: (charterInfo) => {
              this.charterInfo = charterInfo;

              if (this.catTake) {
                let editPermissionSe = this.userRole.permissions.filter(p => (
                  p.action == Action.EDIT
                  && (p.groupName == GroupName.DISTRICT_EXAMINER || p.groupName == GroupName.PCO)
                  && (p.module == Module.CAT_SURVEY || p.module == Module.PCO_EDIT_CAT_SURVEY))
                );

                if (editPermissionSe.length > 0) {
                  if (editPermissionSe.filter(p => p.region == parseInt(this.charterInfo.region)).length > 0) {
                    if (editPermissionSe.filter(p => p.se == this.charterInfo.se).length > 0) {
                      this.messageId = 0;
                    } else {
                      this.messageId = 5;  // Member missing permission to the Credit Union based on member se access! 
                    }
                  } else {
                    this.messageId = 4; //Member missing permission to the Credit Union based on member region access
                  }
                }

                if (this.messageId != 0) {
                  let viewCatAll = this.userRole.permissions.filter(p => (
                    p.action == Action.VIEW
                    && (p.groupName == GroupName.DOS_DOT_RSM || p.groupName == GroupName.SURVEY_VIEWER)
                    && p.module == Module.CAT_SURVEY)
                  );
                  let viewCarByRegion = this.userRole.permissions.filter(p => (
                    p.action == Action.VIEW
                    && (p.groupName == GroupName.PCO
                      || p.groupName == GroupName.DSA_SPECIALIST
                      || p.groupName == GroupName.SUPERVISORY_EXAMINER)
                    && p.module == Module.CAT_SURVEY)
                  );
                  let viewCatBySeAndRegion = this.userRole.permissions.filter(p => (
                    p.action == Action.VIEW
                    && (p.groupName == GroupName.DISTRICT_EXAMINER)
                    && p.module == Module.CAT_SURVEY)
                  );
                  if (viewCatAll.length > 0
                    || (viewCarByRegion.length > 0 && viewCarByRegion.filter(p => p.region == parseInt(this.charterInfo.region)).length > 0)
                    || (viewCatBySeAndRegion.length > 0 && viewCatBySeAndRegion.filter(p => p.region == parseInt(this.charterInfo.region)).length > 0
                      && viewCatBySeAndRegion.filter(p => p.se == this.charterInfo.se).length > 0)) {

                    if (this.surveyInfo && !this.surveyInfo.answers) {
                      this.messageId = 2;
                    } else {
                      this.messageId = 0;
                      this.navigateToReview();
                      return;
                    }
                  } else {
                    this.messageId = 8; // No View and Edit permissions
                  }

                  if (cycleDate == null) {
                    this.router.navigate(['../../message', this.messageId], { relativeTo: this.route });
                    return;
                  } else {
                    this.router.navigate(['../../../message', this.messageId], { relativeTo: this.route });
                    return;
                  }
                }
                // SE take
              } else if (this.seTake) {
                if (this.surveyInfo.hasPreSubmitedSurvey) {
                  let editPermissionRegion = this.userRole.permissions.filter(p => (
                    p.action == Action.EDIT
                    && p.groupName == GroupName.SUPERVISORY_EXAMINER
                    && p.module == Module.SE_SURVEY)
                  );
                  if (editPermissionRegion.length > 0) {
                    if (editPermissionRegion.filter(p => p.region == parseInt(this.charterInfo.region)).length > 0) {
                      this.messageId = 0;
                    } else {
                      this.messageId = 4 // Member missing permission to the Credit Union based on member region access
                    }
                  }
                } else {
                  this.messageId = 11;
                }

                if (this.messageId != 0) {
                  let viewSeAll = this.userRole.permissions.filter(p => (
                    p.action == Action.VIEW
                    && (p.groupName == GroupName.DOS_DOT_RSM || p.groupName == GroupName.SURVEY_VIEWER)
                    && p.module == Module.SE_SURVEY)
                  );
                  let viewSeByRegion = this.userRole.permissions.filter(p => (
                    p.action == Action.VIEW
                    && (p.groupName == GroupName.SUPERVISORY_EXAMINER || p.groupName == GroupName.PCO || p.groupName == GroupName.DSA_SPECIALIST)
                    && p.module == Module.SE_SURVEY)
                  );
                  let viewSeByClaimCu = this.userRole.permissions.filter(p => (
                    p.action == Action.VIEW
                    && p.groupName == GroupName.DISTRICT_EXAMINER
                    && p.module == Module.SE_SURVEY)
                  );
                  if (viewSeAll.length > 0
                    || (viewSeByRegion.length > 0 && viewSeByRegion.filter(p => p.region == parseInt(this.charterInfo.region)).length > 0)
                    || (viewSeByClaimCu.length > 0 && this.isClamed)) {

                    if (this.surveyInfo && !this.surveyInfo.answers) {
                      this.messageId = 2;
                    } else {
                      this.messageId = 0;
                      this.navigateToReview();
                      return;
                    }
                  } else {
                    this.messageId = 8; // No View and Edit permissions
                  }

                  if (cycleDate == null) {
                    this.router.navigate(['../../message', this.messageId], { relativeTo: this.route });
                    return;
                  } else {
                    this.router.navigate(['../../../message', this.messageId], { relativeTo: this.route });
                    return;
                  }
                }
                // DOS take
              } else if (this.dosTake) {
                let editDosRegion = this.userRole.permissions.filter(p => (
                  p.action == Action.EDIT
                  && p.groupName == GroupName.DOS_DOT_RSM
                  && p.module == Module.DOS_EDIT_SURVEY)
                );

                if (editDosRegion.length > 0) {
                  if (editDosRegion.filter(p => p.region == parseInt(this.charterInfo.region)).length > 0) {
                    this.messageId = 0;
                  } else {
                    this.messageId = 4 // Member missing permission to the Credit Union based on member region access
                  }
                }
                if (this.messageId != 0) {
                  let viewDosAll = this.userRole.permissions.filter(p => (
                    p.action == Action.VIEW
                    && (p.groupName == GroupName.DOS_DOT_RSM || p.groupName == GroupName.SURVEY_VIEWER)
                    && p.module == Module.DOS_SURVEY)
                  );
                  let viewDosByRegion = this.userRole.permissions.filter(p => (
                    p.action == Action.VIEW
                    && (p.groupName == GroupName.PCO || p.groupName == GroupName.DSA_SPECIALIST)
                    && p.module == Module.DOS_SURVEY)
                  );
                  let viewDosByClaim = this.userRole.permissions.filter(p => (
                    p.action == Action.VIEW
                    && (p.groupName == GroupName.DISTRICT_EXAMINER || p.groupName == GroupName.SUPERVISORY_EXAMINER)
                    && p.module == Module.DOS_SURVEY)
                  );
                  if (viewDosAll.length > 0
                    || (viewDosByRegion.length > 0 && viewDosByRegion.filter(p => p.region == parseInt(this.charterInfo.region)).length > 0)
                    || (viewDosByClaim.length > 0 && this.isClamed)) {

                    if (this.surveyInfo && !this.surveyInfo.answers) {
                      this.messageId = 2;
                    } else {
                      this.messageId = 0;
                      this.navigateToReview();
                      return;
                    }
                  } else {
                    this.messageId = 8; // No View and Edit permissions
                  }

                  if (cycleDate == null) {
                    this.router.navigate(['../../message', this.messageId], { relativeTo: this.route });
                    return;
                  } else {
                    this.router.navigate(['../../../message', this.messageId], { relativeTo: this.route });
                    return;
                  }
                }
              }
            },
            error: (error) => {
              this.messageId = 7;
              this.router.navigate(['../../message', this.messageId], { relativeTo: this.route });
            }
          });

          this.initFormGroup();
        });
      },
      error: (error) => {
        this.router.navigate(['/survey/message', 10], { relativeTo: this.route });
      }
    });

  }

  ngAfterViewInit(): void {
    setTimeout(function () {
      $('.datepicker').datepicker({
        onSelect: function (date) {
        }
      });

      $('input[data-type="number"]').keyup(function (event) {
        //Skip for arrow keys
        if (event.which >= 37 && event.which <= 40) {
          event.preventDefault();
        }
        const $this = $(this);
        const num = $this.val().replace(/\D/g, '');
        const num2 = num.split(/(?=(?:\d{3})+$)/).join(',');
        $this.val(num2);
      });

      $('input[data-type="text"], textarea').keyup(function (event) {
        const element = this;
        checkElementText(element);
      });

      $('input[data-type="text"], textarea').on('paste', function () {
        const element = this;
        setTimeout(function () {
          const warning = checkElementText(element);
          if (warning) {
            $.dialog({
              'body': $('<img src="assets/warning.png"> <span>' + warning + '</span>'),
              'title': 'Warning',
              'show': true,
              'modal': true,
              'footer': '<button class="dialog-button" data-dialog-action="hide">Close</button>'
            });
          }
        }, 250);
      });

      function checkElementText(element) {
        const maxLen = $(element).attr('maxlength');
        let text = $(element).val();
        let warning = '';
        if (text.length > maxLen) {
          text = text.substr(0, maxLen);
          warning = 'The text was truncated to ' + maxLen + ' characters.';
          $(element).val(text);
        }
        const remLen = maxLen - text.length;
        $(element).closest('.answer').find('.remaining-chars span').text(remLen);
        return warning;
      }
    }, 1500);
  }

  initFormGroup(): void {
    let group = {};
    this.sections.forEach(sec => {
      sec.questions.forEach(question => {
        group[question.id] = new FormControl(question.answer ?? '');
      });
    });
    this.formGroup = new FormGroup(group);
  }

  getRemainingChars(questionId: string, maxLength: number): number {
    let text = this.formGroup.get(questionId).value;
    return (maxLength - (text ?? '').length);
  }

  onSubmit(): void {
    this.updateSurveyType(this.catTake, this.seTake, this.dosTake);
    let answers = this.getCurrentSurveyAnswers();
    if (answers !== null) {
      this.surveySvc.saveAnswers(this.charterNumber, this.surveyType, answers).subscribe(() => {
        this.navigateToReview();
      });
    }
  }

  onPreSubmit(): void {
    let frame = window.sessionStorage.getItem('frameCatSeDos');
    if (frame == 'YES') {
      return;
    } else {
      $('#save-review').modal();
    }
  }

  onSave(): void {
    let frame = window.sessionStorage.getItem('frameCatSeDos');
    if (frame == 'YES') {
      return;
    } else {
      $('#save-as-draft-confirm').modal();
    }
  }

  onSaveConfirm(): void {
    this.updateSurveyType(this.catTake, this.seTake, this.dosTake);
    let answers = this.getCurrentSurveyAnswers();
    if (answers !== null) {
      this.surveySvc.saveAnswers(this.charterNumber, this.surveyType, this.getCurrentSurveyAnswers()).subscribe(() => {
        $('#saved-as-draft').modal();
      });
    }
  }

  getCurrentSurveyAnswers(): Response {
    let answers = this.surveyInfo.answers ?? new Response();
    answers.answers = [];
    this.sections.forEach(sec => {
      sec.questions.forEach(question => {
        let answer = new Answer();
        answer.questionId = question.id;
        if (question.type === QuestionType.DropDownList) {
          answer.questionOptionId = this.formGroup.value[question.id];
          if (!answer.questionOptionId) {
            answer.questionOptionId = null;
          }
        } else {
          if (question.type === QuestionType.Calendar || question.type === QuestionType.NumberBox) {
            answer.text = $('.q' + question.id).val();
          } else {
            answer.text = this.formGroup.value[question.id];
          }
          if (!answer.text) {
            answer.text = null;
          }
        }
        answers.answers.push(answer);
      });
    });
    return answers;
  }

  appyQuestionLogic(questionId: string): void {
    let selectOption = this.formGroup.controls[questionId].value;
    let foundReferences = this.surveyInfo.questionReferences.filter(r => r.referenceQuestionId == questionId);
    if (foundReferences.length > 0) {
      foundReferences.forEach(ref => {
        if (ref.referenceOptionId == selectOption) {
          this.formGroup.controls[ref.questionId].setValue(ref.questionOptionId);
        }
        else {
          this.formGroup.controls[ref.questionId].setValue("");
        }
      });
    };
  }

  validateCyleDate(cycleDate: string): boolean {
    if (cycleDate) {
      if (cycleDate.length == 8) {
        return /^\d+$/.test(cycleDate);
      } else {
        return false; // inalid date format
      }
    } else {
      return true; // use current active cycle date
    }
  }

  navigateToReview(): void {
    if (this.cycleDate) {
      this.router.navigate(['../../../review', this.charterNumber, this.cycleDate], { relativeTo: this.route });
    } else {
      this.router.navigate(['../../review', this.charterNumber], { relativeTo: this.route });
    }
  }
}
