import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Survey } from 'src/app/models/survey/survey';
import { SurveyType } from 'src/app/models/survey/survey-type';
import { AdminService } from 'src/app/services/admin.service';
import { Response } from '../../models/survey/response';

declare const $: any;

@Component({
  selector: 'app-survey-responses',
  templateUrl: './survey-responses.component.html',
  styleUrls: ['./survey-responses.component.scss']
})
export class SurveyResponsesComponent implements OnInit {
  survey: Survey;
  surveyTypes = SurveyType;
  responses: Response[];
  loading = true;
  selectedId: string;

  constructor(private route: ActivatedRoute, private adminSvc: AdminService, private datePipe: DatePipe) {
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      let surveyId: string = params.get('surveyId');
      this.adminSvc.getSurvey(surveyId).subscribe(survey => {
        this.survey = survey;
      });
      this.adminSvc.getResponses(surveyId).subscribe(responses => {
        this.responses = responses;
        this.loading = false;
      });
    });
  }

  formatDate(date: any): string {
    try {
      return (date ? this.datePipe.transform(date, 'MM/dd/yyyy') : '');
    } catch (ex) {
      return '';
    }
  }

  isRejected(response: Response): boolean {
    return `${response.isRejected}` === 'true';
  }

  onReject(id: string): void {
    this.selectedId = id;
    $('#reject-confirmation').modal();
  }

  onRejectConfirmation(): void {
    let response = this.responses.find(x => x.id === this.selectedId);
    this.adminSvc.rejectResponse(response.id).subscribe(() => {
      response.isRejected = true;
    });
  }
}
