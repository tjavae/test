import { Component, OnInit } from '@angular/core';
import { UserRole } from 'src/app/models/auth/user-role';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { VersionService } from 'src/app/services/version.service';

@Component({
  selector: 'app-admin-layout',
  templateUrl: './admin-layout.component.html',
  styleUrls: ['./admin-layout.component.scss']
})
export class AdminLayoutComponent implements OnInit {
  userRole: UserRole;
  version: string;

  constructor(private authSvc: AuthenticationService, private versionSvc: VersionService) {
  }

  ngOnInit(): void {
    this.authSvc.getUser().subscribe(userRole => {
      this.userRole = userRole;
    });
    this.versionSvc.getVersion().subscribe(version => {
      this.version = version;
    });
  }
}
