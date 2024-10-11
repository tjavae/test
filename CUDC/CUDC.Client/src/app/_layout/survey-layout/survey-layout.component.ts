import { Component, OnInit } from '@angular/core';
import { UserRole } from 'src/app/models/auth/user-role';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { VersionService } from 'src/app/services/version.service';

@Component({
  selector: 'app-survey-layout',
  templateUrl: './survey-layout.component.html',
  styleUrls: ['./survey-layout.component.scss']
})
export class SurveyLayoutComponent implements OnInit {
  userRole: UserRole;
  version: string;

  constructor(private authSvc: AuthenticationService, private versionSvc: VersionService) {
  }

  ngOnInit() {
    this.authSvc.getUser().subscribe(userRole => {
      this.userRole = userRole;
    });
    this.versionSvc.getVersion().subscribe(version => {
      this.version = version;
    });
  }
}
