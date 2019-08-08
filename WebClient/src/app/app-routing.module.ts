import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NotFoundComponent } from './not-found/not-found.component';
import { HomeComponent } from './home/home.component';
import { RegisterUserComponent } from './register-user/register-user.component';

const routes: Routes = [
    { path: '', component: HomeComponent, pathMatch: 'full' },
    { path: 'register-user', component: RegisterUserComponent },
    { path: '404', component: NotFoundComponent },
    { path: '**', redirectTo: '404' }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }

export const routingComponents = [
    HomeComponent,
    RegisterUserComponent,
    NotFoundComponent
]
