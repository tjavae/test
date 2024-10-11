import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { GroupName, Module } from '../common/constants';
import { AuthenticationService } from '../services/authentication.service';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  constructor(private authSvc: AuthenticationService, private router: Router, private route: ActivatedRoute ) {
  }

  ngOnInit(): void {
    this.authSvc.getUser().subscribe(userRole => {
      if (userRole){
        let adminView = userRole.permissions.find(p => p.groupName == GroupName.ADMIN && p.module == Module.SURVEYS_MANAGEMENT);
        let testerView = userRole.permissions.find(p => p.groupName == GroupName.TESTER && p.module == Module.SURVEYS_MANAGEMENT);
        if (adminView || testerView){
          this.router.navigate(['/admin']);
        } else {
          this.router.navigate(['/survey']);
        }
      } else {
        this.router.navigate(['/survey/message', 1], { relativeTo: this.route });   
      }     
    }, (error) => {
        this.router.navigate(['/survey/message', 1], { relativeTo: this.route });   
        return;
    });
  }
}
