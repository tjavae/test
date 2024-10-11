import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LowerCaseUrlSerialize } from '../admin/admin-serialize.module';
import { CatRoutingModule } from './cat-routing.module';
import { NotFoundComponent } from './not-found/not-found.component';

@NgModule({
  declarations: [
    NotFoundComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    CatRoutingModule
  ],
  providers: [
    {
      provide: LowerCaseUrlSerialize,
      useClass: LowerCaseUrlSerialize
    }
  ],
})
export class CatModule { }
