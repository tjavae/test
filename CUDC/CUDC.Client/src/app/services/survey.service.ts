import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { _Question, _QuestionReference, _Section } from '../models/survey/internal';
import { QuestionType } from '../models/survey/question-type';
import { Response } from '../models/survey/response';
import { Section } from '../models/survey/section';
import { SurveyInfo } from '../models/survey/survey-info';
import { SurveyType } from '../models/survey/survey-type';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class SurveyService extends ApiService {
  constructor(httpClient: HttpClient) {
    super(httpClient);
  }

  isSurveyActive(surveyType: SurveyType): Observable<boolean> {
    return this.httpClient.get<boolean>(this.url(`Survey/${surveyType}/IsSurveyActive`));
  }

  getSurveyInfoByCycle(charterNumber: number, surveyType: SurveyType, cycleDate: string): Observable<SurveyInfo> {
    let userId = window.sessionStorage.getItem('userId');
    if (cycleDate == null) {
      if (userId) {
        return this.httpClient.get<SurveyInfo>(this.url(`Survey/${charterNumber}/${surveyType}/${userId}/GetSurveyInfoAdminView`));
      } else {
        return this.httpClient.get<SurveyInfo>(this.url(`Survey/${charterNumber}/${surveyType}/GetSurveyInfo`));
      }
    } else if (userId) {
      return this.httpClient.get<SurveyInfo>(this.url(`Survey/${charterNumber}/${surveyType}/${userId}/${cycleDate}/GetSurveyInfoAdminViewByCycle`));
    } else {
      return this.httpClient.get<SurveyInfo>(this.url(`Survey/${charterNumber}/${surveyType}/${cycleDate}/GetSurveyInfoByCycle`));
    }
  }

  getSurveyInfo(charterNumber: number, surveyType: SurveyType): Observable<SurveyInfo> {
    let userId = window.sessionStorage.getItem('userId');
    if (userId) {
      return this.httpClient.get<SurveyInfo>(this.url(`Survey/${charterNumber}/${surveyType}/${userId}/GetSurveyInfoAdminView`));
    }
    return this.httpClient.get<SurveyInfo>(this.url(`Survey/${charterNumber}/${surveyType}/GetSurveyInfo`));
  }

  saveAnswers(charterNumber: number, surveyType: SurveyType, answers: Response) {
    return this.httpClient.post(this.url(`Survey/${charterNumber}/${surveyType}/SaveAnswers`), answers);
  }

  submitAnswers(responseId: string) {
    return this.httpClient.post(this.url(`Survey/${responseId}/SubmitAnswers`), {});
  }

  unlockSurvey(responseId: string) {
    return this.httpClient.post(this.url(`Survey/${responseId}/UnlockSurvey`), {});
  }

  transferOwnership(responseId: string, userId: string) {
    let httpUserId = userId.replace(/\\/g, "%5C");
    return this.httpClient.post(this.url(`Survey/${responseId}/${httpUserId}/TransferOwnership`), {});
  }

  getSections(surveyInfo: SurveyInfo): _Section[] {
    let sections: Section[] = [];
    if (surveyInfo.sections.length) {
      sections = surveyInfo.sections;
    } else {
      let sec = new Section();
      sec.position = 1;
      sections.push(sec);
    }
    let _sections: _Section[] = [];
    let sortSections = sections.toSorted((x, y) => x.position - y.position);
    sortSections.forEach(sec => {
      let _sec = new _Section();
      _sec.id = sec.id;
      _sec.title = sec.title;
      _sec.description = sec.description;
      _sec.position = sec.position;
      _sec.questions = [];
      _sec.references = [];
      surveyInfo.questions.filter(x => x.sectionId == sec.id).sort((x, y) => x.position - y.position).forEach(question => {
        let _question = new _Question();
        _question.id = question.id;
        _question.number = question.number;
        _question.type = question.revision.type;
        _question.text = question.revision.text;
        _question.maxLength = question.revision.maxLength || 250;
        _question.position = question.position;
        _question.isRequired = question.isRequired;
        let answer = surveyInfo.answers?.answers?.find(x => x.questionId === question.id);
        if (_question.type === QuestionType.DropDownList) {
          _question.options = [];
          _question.answer = answer?.questionOptionId;
          surveyInfo.questionOptions.filter(x => x.questionId === question.id).sort((x, y) => x.position - y.position).forEach(option => {
            _question.options.push(option);
          });
          //push references
          if (surveyInfo.questionReferences) {
            surveyInfo.questionReferences.filter(q => q.referenceQuestionId === _question.id).forEach(qr => {
              let _ref = new _QuestionReference();
              _ref.id = qr.id;
              _ref.referenceQuestionId = qr.referenceQuestionId;
              _ref.questionId = qr.questionId;
              _ref.questionOptionId = qr.questionOptionId;
              _ref.referenceOptionId = qr.referenceOptionId;
              _ref.rNumber = _question.number;
              _ref.qNumber = surveyInfo.questions.filter(q => q.id === qr.questionId)[0].number;
              _ref.rOption = surveyInfo.questionOptions.filter(opt => opt.id === qr.referenceOptionId)[0].text;
              _ref.qOption = surveyInfo.questionOptions.filter(opt => opt.id === qr.questionOptionId)[0].text;
              _sec.references.push(_ref);
            });
          }
        } else {
          _question.answer = answer?.text;
        }
        _sec.questions.push(_question);
      });
      _sections.push(_sec);
    });
    return _sections;
  }
}
