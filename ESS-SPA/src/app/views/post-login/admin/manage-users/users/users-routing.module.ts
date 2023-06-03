import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { UsersComponent } from "./users.component";
import { UserComponent } from "./user/user.component";

const routes: Routes = [
    { path: '', component: UsersComponent, data: { title: 'Manage Users' } },
    { path: 'new', component: UserComponent, data: { title: 'Manage Users / New' } },
    // { path: 'edit/:id', component: UserComponent, data: { title: 'Users / Edit' }, resolve: {user: UserResolver} },
  ];


  @NgModule({
imports:[RouterModule.forChild(routes)],
exports:[RouterModule]
  })

  export class UsersRoutingModule {}