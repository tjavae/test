import { Component, OnInit } from '@angular/core';
import { UserPermission } from 'src/app/models/auth/user-type';
import { SearchUserRequest } from 'src/app/models/search/search-user-request';
import { SearchUserResult } from 'src/app/models/search/search-user-result';
import { AdminService } from 'src/app/services/admin.service';
import { AuthenticationService } from 'src/app/services/authentication.service';

declare const $: any;

@Component({
  selector: 'app-search-user',
  templateUrl: './search-user.component.html',
  styleUrls: ['./search-user.component.scss'] 
})
export class SearchUserComponent implements OnInit {  
  modalTitle: string;
  selectedId: string; 
  model = new SearchUserRequest();
  searchResults: Array<SearchUserResult> = [];   
  showNotFound: boolean = true;
  showPermissionNotFound: boolean = false;
  userPermissions: Array<UserPermission> = [];
  currentUserId: string;

  constructor(
    private adminSvc: AdminService,
    private authSvc: AuthenticationService
  ) { }

  ngOnInit(): void {
    this.currentUserId = null;
  }

  search(): void {       
    
    this.userPermissions.length = 0;
    this.searchResults.length = 0;
    this.adminSvc.getSearchUserResults(this.model).subscribe(searchResults => {
      if (searchResults.length > 0) {
        this.searchResults = searchResults;   
        this.showNotFound = false;        
      } else {       
        this.showNotFound = true;
      };
    });
  }

  onViewPermissions(id: string): void {
    this.currentUserId = id;
    this.userPermissions.length = 0; 
    this.authSvc.getUserById(id).subscribe(userRole => {            
      if (userRole && userRole.permissions.length > 0 )
      {   
        this.userPermissions = userRole.permissions;
        this.showPermissionNotFound = false;        
      } else {
        this.showPermissionNotFound = true;
      }      
    }, 
    (error) => {
      this.showPermissionNotFound = true;      
    }); 
  }

   
  resetSearch(): void {
    this.showNotFound = true;       
    this.searchResults.length = 0;
    this.userPermissions.length = 0;    
    Object.keys(this.model).forEach(key => this.model[key]=null);
    this.currentUserId = null;    
  }  
  
}
