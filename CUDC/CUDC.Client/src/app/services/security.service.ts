import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserRole } from '../models/auth/user-role';
import { SurveyOwnerResponse } from '../models/survey/survey-owner-response';
import { ApiService } from './api.service';

@Injectable({
    providedIn: 'root'
})
export class SecurityService extends ApiService {
    constructor(httpClient: HttpClient) {
        super(httpClient);
    }

    getSurveyOwner(surveyId: string, cuNumber: number): Observable<SurveyOwnerResponse> {
        let userId = window.sessionStorage.getItem('userId');      
        let radarUserId = window.sessionStorage.getItem('radarUserId');
        let frameCatSeDos = window.sessionStorage.getItem('frameCatSeDos');
        if (frameCatSeDos == 'YES') {
        userId = radarUserId;
        }
        if (userId) {
            return this.httpClient.get<SurveyOwnerResponse>(this.url(`Security/${surveyId}/${cuNumber}/${userId}/GetSurveyOwnerAdminView`));
        }
        return this.httpClient.get<SurveyOwnerResponse>(this.url(`Security/${surveyId}/${cuNumber}/GetSurveyOwner`));
    }

    isSeUser(): Observable<boolean> {
        return this.httpClient.get<boolean>(this.url('Security/IsSeUser'));
    }

    isClaimedCu(cuNumber: number, userId: string): Observable<boolean> {
        let httpUserId = userId.replace(/\\/g, "%5C");
        return this.httpClient.get<boolean>(this.url(`Security/${cuNumber}/${httpUserId}/IsClaimedCU`));
    }

    getNcuaUsers(region: number, surveyType: number): Observable<UserRole[]> {
        return this.httpClient.get<UserRole[]>(this.url(`Security/${region}/${surveyType}/GetNcuaUsers`));
    }
}
