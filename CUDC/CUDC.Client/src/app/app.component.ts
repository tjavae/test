import { Component, OnInit } from '@angular/core';
import { AppInsightsService } from './services/app-insights.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  environmentName = "";
  lowerEnvironmentBool = false;

  constructor(private appInsightsService: AppInsightsService) {
  }

  ngOnInit(): void {
    let hostname = window.location.hostname;
    if (hostname.includes("localhost")) {
      this.environmentName = "Local";
      this.lowerEnvironmentBool = true;
    }
    else if (hostname.includes("devcudc")) {
      this.environmentName = "Dev";
      this.lowerEnvironmentBool = true;
    }
    else if (hostname.includes("testcudc")) {
      this.environmentName = "Test";
      this.lowerEnvironmentBool = true;
    }
    else if (hostname.includes("stgcudc")) {
      this.environmentName = "Stage";
      this.lowerEnvironmentBool = true;
    }
  }
}
