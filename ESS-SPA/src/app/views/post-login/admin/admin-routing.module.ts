import { NgModule } from "@angular/core";
import { Routes, RouterModule } from '@angular/router';


const routes: Routes = [

    {
        path: 'manage-users',
        loadChildren: () => import('./manage-users/users/users.module').then(m => m.UsersModule)
    }
];



@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports:[RouterModule]
})

export class AdminRoutingModule {}