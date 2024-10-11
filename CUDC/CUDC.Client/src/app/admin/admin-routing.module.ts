import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LowerCaseUrlSerialize } from '../admin/admin-serialize.module';
import { FinishComponent } from '../survey/finish/finish.component';
import { ReviewComponent } from '../survey/review/review.component';
import { SearchComponent } from '../survey/search/search.component';
import { TakeComponent } from '../survey/take/take.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { SurveyPreviewComponent } from './survey-preview/survey-preview.component';
import { SurveyQuestionsComponent } from './survey-questions/survey-questions.component';
import { SurveyResponsesComponent } from './survey-responses/survey-responses.component';
import { SurveysComponent } from './surveys/surveys.component';
import { MessageComponent } from '../survey/message/message.component'
import { RadarViewComponent } from './radar-view/radar-view.component';
import {SearchUserComponent} from './search-user/search-user.component';

const routes: Routes = [
    { path: 'surveys', component: SurveysComponent },
    { path: 'surveys/questions-edit/:surveyId', component: SurveyQuestionsComponent },
    { path: 'surveys/preview/:surveyId', component: SurveyPreviewComponent },
    { path: 'surveys/responses/:surveyId', component: SurveyResponsesComponent },
    { path: 'survey', component: SearchComponent },
    { path: 'survey/take/:charterNumber', component: TakeComponent },
    { path: 'survey/review/:charterNumber', component: ReviewComponent },
    { path: 'manage', component: SearchComponent },
    { path: 'manage/cat/review/:charterNumber', component: ReviewComponent },
    { path: 'manage/se/review/:charterNumber', component: ReviewComponent },
    { path: 'manage/dos/review/:charterNumber', component: ReviewComponent },
    { path: 'manage/cat/review/:charterNumber/:cycleDate', component: ReviewComponent },
    { path: 'manage/se/review/:charterNumber/:cycleDate', component: ReviewComponent },
    { path: 'manage/dos/review/:charterNumber/:cycleDate', component: ReviewComponent },
    { path: 'survey/cudc/review/:charterNumber', component: ReviewComponent },
    { path: 'manage/cat/message/:messageId', component: MessageComponent },
    { path: 'manage/se/message/:messageId', component: MessageComponent },
    { path: 'manage/dos/message/:messageId', component: MessageComponent },
    { path: 'manage/cudc/message/:messageId', component: MessageComponent },
    { path: 'survey/finish', component: FinishComponent },
    { path: 'message/:messageId', component: MessageComponent },
    { path: 'radar-view', component: RadarViewComponent },
    { path: 'search-user', component: SearchUserComponent },
    { path: '**', component: DashboardComponent }
];

@NgModule({
    imports: [
        RouterModule.forChild(routes)
    ],
    exports: [
        RouterModule
    ],
    providers: [
      {
        provide: LowerCaseUrlSerialize,
        useClass: LowerCaseUrlSerialize
      }
    ]
})
export class AdminRoutingModule { }
