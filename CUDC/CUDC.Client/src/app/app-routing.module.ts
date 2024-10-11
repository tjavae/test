import { NgModule } from '@angular/core';
import { LowerCaseUrlSerialize } from './admin/admin-serialize.module';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AdminLayoutComponent } from './_layout/admin-layout/admin-layout.component';
import { CatLayoutComponent } from './_layout/cat-layout/cat-layout.component';
import { EmptyLayoutComponent } from './_layout/empty-layout/empty-layout.component';
import { SurveyLayoutComponent } from './_layout/survey-layout/survey-layout.component';

const routes: Routes = [
  {
    path: '',
    component: EmptyLayoutComponent,
    children: [
      {
        path: '',
        component: HomeComponent,
        pathMatch: 'full'
      }
    ]
  },
  {
    path: '',
    component: SurveyLayoutComponent,
    children: [
      {
        path: 'survey',
        loadChildren: () => import('./survey/survey.module').then(m => m.SurveyModule)
      }
    ]
  },
  {
    path: '',
    component: CatLayoutComponent,    
    children: [
      {
        path: 'cat',
        loadChildren: () => import('./cat/cat.module').then(m => m.CatModule)
      }
    ]
  },
  {
    path: '',
    component: CatLayoutComponent,
    children: [
      {
        path: 'se',
        loadChildren: () => import('./cat/cat.module').then(m => m.CatModule)
      }
    ]
  }, 
  {
    path: '',
    component: CatLayoutComponent,
    children: [
      {
        path: 'dos',
        loadChildren: () => import('./cat/cat.module').then(m => m.CatModule)
      }
    ]
  },
  {
    path: '',
    component: AdminLayoutComponent,
    children: [
      {
        path: 'admin',
        loadChildren: () => import('./admin/admin.module').then(m => m.AdminModule)
      }
    ]
  },
  { path: '**', redirectTo: './survey'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  providers: [
    {
      provide: LowerCaseUrlSerialize,
      useClass: LowerCaseUrlSerialize
    },
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
