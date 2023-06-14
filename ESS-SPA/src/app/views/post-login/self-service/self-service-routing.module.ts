import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SelfServiceComponent } from './self-service.component';

const routes: Routes = [
    { path: '', component: SelfServiceComponent, data: {title: 'Self Service'} },
    { path: '**', redirectTo: '/404' }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SelfServiceRoutingModule {}