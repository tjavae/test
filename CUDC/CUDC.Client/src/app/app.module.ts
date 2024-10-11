import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { LowerCaseUrlSerialize } from './admin/admin-serialize.module';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { AdminModule } from './admin/admin.module';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CatModule } from './cat/cat.module';
import { HomeComponent } from './home/home.component';
import { AuthInterceptor } from './services/auth-interceptor';
import { SurveyModule } from './survey/survey.module';
import { AdminLayoutComponent } from './_layout/admin-layout/admin-layout.component';
import { CatLayoutComponent } from './_layout/cat-layout/cat-layout.component';
import { EmptyLayoutComponent } from './_layout/empty-layout/empty-layout.component';
import { SurveyLayoutComponent } from './_layout/survey-layout/survey-layout.component';

@NgModule({
  declarations: [
    AppComponent,
    SurveyLayoutComponent,
    CatLayoutComponent,
    HomeComponent,
    EmptyLayoutComponent,
    AdminLayoutComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    SurveyModule,
    CatModule,
    AdminModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    {
      provide: LowerCaseUrlSerialize,
      useClass: LowerCaseUrlSerialize
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
