import { CommonModule, DatePipe } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LowerCaseUrlSerialize } from './admin-serialize.module';
import { AdminRoutingModule } from './admin-routing.module';
import { DashboardComponent } from './dashboard/dashboard.component';
import { SurveyPreviewComponent } from './survey-preview/survey-preview.component';
import { SurveyQuestionsComponent } from './survey-questions/survey-questions.component';
import { SurveyResponsesComponent } from './survey-responses/survey-responses.component';
import { SurveysComponent } from './surveys/surveys.component';
import { UsersComponent } from './users/users.component';
import { RadarViewComponent } from './radar-view/radar-view.component';
import { SearchUserComponent } from './search-user/search-user.component';

@NgModule({
  declarations: [
    DashboardComponent,
    UsersComponent,
    SurveysComponent,
    SurveyQuestionsComponent,
    SurveyPreviewComponent,
    SurveyResponsesComponent,
    RadarViewComponent,
    SearchUserComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    AdminRoutingModule
  ],
  providers: [
    { provide: DatePipe },
    {
      provide: LowerCaseUrlSerialize,
      useClass: LowerCaseUrlSerialize
    }
  ]

})
export class AdminModule { }
