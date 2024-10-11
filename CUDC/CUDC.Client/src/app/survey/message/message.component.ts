import { Component, OnInit, AfterViewInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
  selector: 'app-finish',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.scss']
})
export class MessageComponent implements OnInit {

  titleMessage = '';
  displayMessage = '';
  displayMessage2 = '';
  msgId = false;

  constructor(private route: ActivatedRoute, private router: Router, private authSvc: AuthenticationService) { }

  ngOnInit(): void {   
        this.route.paramMap.subscribe(params => {
          let messageId : number = parseInt(params.get('messageId'));
          this.msgId = messageId == 3? true: false; 
          if (messageId == 1){  //No Admin Access
            this.titleMessage = 'System Notification';
            this.displayMessage = 'You are not currently authorized to perform administrative actions on this site. ';  
            this.displayMessage2 = 'If you feel as though you have received this message in error, please contact your Regional Administrator.' ;
          }  else if (messageId == 2){ // Notify that survey search for has not been created
            this.titleMessage = 'Survey Status';
            this.displayMessage = 'Survey has not been created.';
          } else if (messageId == 3){
            this.titleMessage = 'You are not currently authorized to access this site';
            this.displayMessage = 'Please submit a OneStop ticket with supervisor approval attached.';
          }else if (messageId == 4){  //Not permitted to access a particular region
            this.titleMessage = 'Permission Limitations';
            this.displayMessage = 'You do not have permission to view the credit union based on your assigned region.';
          } else if (messageId == 5){  //Not permitted to access a particular se
            this.titleMessage = 'Permission Limitations';
            this.displayMessage = 'You do not have permission to view the credit union based on your SE group.';
          } else if (messageId == 6){  //Not permitted to access a particular se
            this.titleMessage = 'Unclaimed Credit Union';
            this.displayMessage = 'The credit union you have selected is within your Region, SE group, and/or District, but only the survey owner may view the responses to the selected survey.';
            this.displayMessage2 = 'If you feel as though you have received this message in error, please contact your Regional Administrator.';
          }else if (messageId == 7){
            this.titleMessage = 'Credit Union Does Not Exist';
            this.displayMessage = 'Please check the Charter Number.';
          }else if (messageId == 8){
            this.titleMessage = 'Permission Limitations';
            this.displayMessage = 'You do not have permission to view and edit the credit union.';
          } else if (messageId == 9){   
            this.titleMessage = 'Submission Confirmation';
            this.displayMessage = 'Thank you for completing the survey.';
          } else if (messageId == 10){   
            this.titleMessage = 'Survey Status';
            this.displayMessage = 'There is no prior survey data associated with the cycle you have selected.';
            this.displayMessage2 = 'Please select another cycle. If you believe this is an error, please contact OneStop.';
          } else if (messageId == 11){ // CAT survey must be submitted prior to start SE survey.  
            this.titleMessage = 'Survey Status';
            this.displayMessage = 'The Examiner Survey for this credit union has not been submitted. Please contact the assigned examiner to request they submit the survey for SE review.';
          }
        });
  }

}
