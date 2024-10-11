import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { GroupName, Module } from 'src/app/common/constants';
import { AdminService } from 'src/app/services/admin.service';

declare const $: any;

@Component({
  selector: 'app-radar-view',
  templateUrl: './radar-view.component.html',
  styleUrls: ['./radar-view.component.scss']
})
export class RadarViewComponent implements OnInit, OnDestroy {
  userId: string;
  cuNumber: number;
  catUrl: SafeResourceUrl;
  seUrl: SafeResourceUrl;
  dosUrl: SafeResourceUrl; 
  cycleDate: string;
  cycleDates;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private sanitizer: DomSanitizer,
    private authSvc: AuthenticationService,
    private adminSvc: AdminService ) {
  }

  ngOnInit(): void {
    this.authSvc.getUser().subscribe(userRole => {
      if (userRole){
        let adminView = userRole.permissions.find(p => p.groupName == GroupName.ADMIN && p.module == Module.SURVEYS_MANAGEMENT);
        let testerView = userRole.permissions.find(p => p.groupName == GroupName.TESTER && p.module == Module.SURVEYS_MANAGEMENT);
        if (adminView || testerView){   
          window.sessionStorage.setItem('userId', userRole.userId.replace('\\', '%5c'));          
          this.adminSvc.getCycleDates().subscribe(cDates => {
            if (cDates) { this.cycleDates = cDates; }
          });
          this.catUrl = this.sanitizer.bypassSecurityTrustResourceUrl('about:blank');
          this.seUrl = this.sanitizer.bypassSecurityTrustResourceUrl('about:blank');
          this.dosUrl = this.sanitizer.bypassSecurityTrustResourceUrl('about:blank');
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

  onView(): void {
    if ($('form')[0].checkValidity() === false) {
      $('form').addClass('was-validated');
      return;
    }

    //save user id, so other components can use it    
    window.sessionStorage.setItem('radarUserId', this.userId.replace('\\', '%5c'));
    // Save this frameCatSeDos to disbale 'Save as Draft' and 'Save and Review'button on dos/se/cat frames.
    window.sessionStorage.setItem('frameCatSeDos', 'YES');
    //load iframes
    if (this.cycleDate == null){
      this.catUrl = this.sanitizer.bypassSecurityTrustResourceUrl(`cat/take/${this.cuNumber}`);
      this.seUrl = this.sanitizer.bypassSecurityTrustResourceUrl(`se/take/${this.cuNumber}`);
      this.dosUrl = this.sanitizer.bypassSecurityTrustResourceUrl(`dos/take/${this.cuNumber}`);
    } else {
      this.catUrl = this.sanitizer.bypassSecurityTrustResourceUrl(`cat/take/${this.cuNumber}/${this.cycleDate}`);
      this.seUrl = this.sanitizer.bypassSecurityTrustResourceUrl(`se/take/${this.cuNumber}/${this.cycleDate}`);
      this.dosUrl = this.sanitizer.bypassSecurityTrustResourceUrl(`dos/take/${this.cuNumber}/${this.cycleDate}`);
    }  
  }

  onCancel(): void {
    this.router.navigate(['../'], { relativeTo: this.route });
  }

  ngOnDestroy(): void {
    window.sessionStorage.setItem('radarUserId', null);
    window.sessionStorage.setItem('frameCatSeDos', null);
  }
}
