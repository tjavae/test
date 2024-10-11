import { NgModule } from '@angular/core';
import { LowerCaseUrlSerialize } from '../admin/admin-serialize.module';
import { RouterModule, Routes } from '@angular/router';
import { ReviewComponent } from '../survey/review/review.component';
import { TakeComponent } from '../survey/take/take.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { FinishComponent } from '../survey/finish/finish.component';
import { MessageComponent } from '../survey/message/message.component';

const routes: Routes = [
    { path: 'take/:charterNumber', component: TakeComponent },
    { path: 'take/:charterNumber/:cycleDate', component: TakeComponent },
    { path: 'review/:charterNumber', component: ReviewComponent },
    { path: 'review/:charterNumber/:cycleDate', component: ReviewComponent },
    { path: 'finish', component: FinishComponent},
    { path: 'message/:messageId', component: MessageComponent},
    { path: '**', component: NotFoundComponent }
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
export class CatRoutingModule { }
