import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BasicInfo } from '../models/search/basic-info';
import { SearchRequest } from '../models/search/search-request';
import { SearchResult } from '../models/search/search-result';
import { SelectListItem } from '../models/select-list-item';
import { ApiService } from './api.service';
import { CharterInfo } from '../models/auth/charter-info';

@Injectable({
  providedIn: 'root'
})
export class SearchService extends ApiService {
  constructor(httpClient: HttpClient) {
    super(httpClient);
  }

  getStates(): Observable<SelectListItem[]> {
    return this.httpClient.get<SelectListItem[]>(this.url('Search/GetStates'));
  }

  getSearchResults(request: SearchRequest): Observable<SearchResult[]> {
    return this.httpClient.post<SearchResult[]>(this.url('Search/GetSearchResults'), request);
  }

  getBasicInfo(charterNumber: number): Observable<BasicInfo> {
    return this.httpClient.get<BasicInfo>(this.url(`Search/${charterNumber}/GetBasicInfo`));
  }

  getCuInformation(charterNumber: number): Observable<CharterInfo>{
    return this.httpClient.get<CharterInfo>(this.url(`Search/${charterNumber}/GetCreditUnionInformation`))
  }
}
