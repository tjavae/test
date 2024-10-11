import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { LowerCaseUrlSerialize } from '../admin/admin-serialize.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FinishComponent } from './finish/finish.component';
import { ReviewComponent } from './review/review.component';
import { SearchComponent } from './search/search.component';
import { SurveyRoutingModule } from './survey-routing.module';
import { TakeComponent } from './take/take.component';
import { MessageComponent } from '../survey/message/message.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    SurveyRoutingModule
  ],
  declarations: [
    SearchComponent,
    TakeComponent,
    ReviewComponent,
    FinishComponent,
    MessageComponent
  ],
  providers: [
    {
      provide: LowerCaseUrlSerialize,
      useClass: LowerCaseUrlSerialize
    }
  ]
})
export class SurveyModule { }
