import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CycleDate } from '../models/auth/cycle-date';
import { UserRole } from '../models/auth/user-role';
import { SearchUserRequest } from '../models/search/search-user-request';
import { SearchUserResult } from '../models/search/search-user-result';
import { QuestionsEdit } from '../models/survey/questions-edit';
import { Response } from '../models/survey/response';
import { Survey } from '../models/survey/survey';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class AdminService extends ApiService {
  constructor(httpClient: HttpClient) {
    super(httpClient);
  }

  getSurveys(): Observable<Survey[]> {
    return this.httpClient.get<Survey[]>(this.url('Admin/GetSurveys'));
  }

  getSurvey(surveyId: string): Observable<Survey> {
    return this.httpClient.get<Survey>(this.url(`Admin/GetSurvey/${surveyId}`));
  }

  createSurvey(survey: Survey): Observable<Survey> {
    return this.httpClient.post<Survey>(this.url('Admin/CreateSurvey'), survey);
  }

  updateSurvey(survey: Survey): Observable<Survey> {
    return this.httpClient.put<Survey>(this.url('Admin/UpdateSurvey'), survey);
  }

  copySurvey(surveyId: string) {
    return this.httpClient.get(this.url(`Admin/CopySurvey/${surveyId}`));
  }

  getQuestions(surveyId: string): Observable<QuestionsEdit> {
    return this.httpClient.get<QuestionsEdit>(this.url(`Admin/GetQuestions/${surveyId}`));
  }

  // createQuestions(questions: QuestionsEdit) {
  //   return this.httpClient.post(this.url('Admin/CreateQuestions'), questions);
  // }

  updateQuestions(questions: QuestionsEdit) {
    return this.httpClient.put(this.url('Admin/UpdateQuestions'), questions);
  }

  getResponses(surveyId: string): Observable<Response[]> {
    return this.httpClient.get<Response[]>(this.url(`Admin/GetResponses/${surveyId}`));
  }

  rejectResponse(responseId: string) {
    return this.httpClient.patch(this.url(`Admin/RejectResponse/${responseId}`), {});
  }

  getUsers(): Observable<UserRole[]> {
    return this.httpClient.get<UserRole[]>(this.url('Admin/GetUsers'));
  }

  createUser(user: UserRole): Observable<UserRole> {
    return this.httpClient.post<UserRole>(this.url('Admin/CreateUser'), user);
  }

  updateUser(user: UserRole): Observable<UserRole> {
    return this.httpClient.put<UserRole>(this.url('Admin/UpdateUser'), user);
  }

  deleteUser(id: string) {
    return this.httpClient.delete(this.url(`Admin/DeleteUser/${id}`));
  }

  getCycleDates(): Observable<CycleDate[]> {
    return this.httpClient.get<CycleDate[]>(this.url('Admin/GetCycleDates'));
  }  

  deleteSurvey(surveyId: string) {
    return this.httpClient.get(this.url(`Admin/DeleteSurvey/${surveyId}`));
  }

  
  getSearchUserResults(request: SearchUserRequest): Observable<SearchUserResult[]> {
    return this.httpClient.post<SearchUserResult[]>(this.url('Admin/GetSearchUserResults'), request);
  }
}
