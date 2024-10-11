import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserRole } from '../models/auth/user-role';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService extends ApiService {
  constructor(httpClient: HttpClient) {
    super(httpClient);
  }

  getUser(): Observable<UserRole> {
    return this.httpClient.get<UserRole>(this.url('Auth/GetUser'));
  }

  getRadarUser(): Observable<UserRole> {
    let userId = window.sessionStorage.getItem('userId');
    let radarUserId = window.sessionStorage.getItem('radarUserId');
    let frameCatSeDos = window.sessionStorage.getItem('frameCatSeDos');
    if (frameCatSeDos == 'YES') {
      userId = radarUserId;
    }
    if (userId) {
      return this.httpClient.get<UserRole>(this.url(`Auth/${userId}/GetUserAdminView`));
    } else {
      return this.httpClient.get<UserRole>(this.url('Auth/GetUser'));
    }
  }

  getUserById(userId: string): Observable<UserRole> {
    return this.httpClient.get<UserRole>(this.url(`Auth/${userId}/GetUserById`));
  }
}
