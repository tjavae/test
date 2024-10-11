import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class VersionService extends ApiService {
  constructor(httpClient: HttpClient) {
    super(httpClient);
  }

  getVersion(): Observable<string> {
    return this.httpClient.get(this.url('Version'), { responseType: 'text' });
  }
}
