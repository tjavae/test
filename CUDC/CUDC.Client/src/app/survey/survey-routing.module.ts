import { NgModule } from '@angular/core';
import { LowerCaseUrlSerialize } from '../admin/admin-serialize.module';
import { RouterModule, Routes } from '@angular/router';
import { FinishComponent } from './finish/finish.component';
import { ReviewComponent } from './review/review.component';
import { SearchComponent } from './search/search.component';
import { TakeComponent } from './take/take.component';
import { MessageComponent } from './message/message.component';

const routes: Routes = [
  { path: 'take/:charterNumber', component: TakeComponent },
  { path: 'take/:charterNumber/:cyleDate', component: TakeComponent },
  { path: 'review/:charterNumber', component: ReviewComponent },
  { path: 'review/:charterNumber/:cyleDate', component: ReviewComponent },
  { path: 'finish', component: FinishComponent },
  { path: 'message/:messageId', component: MessageComponent },
  { path: '**', component: SearchComponent }
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
export class SurveyRoutingModule { }
