import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './_guards/auth.guard';
import { LoginGuard } from './_guards/login.guard';
import { UsersComponent } from './home/users/users.component';
import { PurchasesComponent } from './home/users/purchases/purchases.component';
import { SubscriptionsComponent } from './home/users/subscriptions/subscriptions.component';


const routes: Routes = [
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    { path: 'home', component: HomeComponent, canActivate: [AuthGuard] },
    { path: 'login', component: LoginComponent, canActivate: [LoginGuard] },
    { path: 'users', component: UsersComponent, canActivate: [AuthGuard] },
    { path: 'users/purchases/:userId', component: PurchasesComponent, canActivate: [AuthGuard] },
    { path: 'users/subscriptions/:userId', component: SubscriptionsComponent, canActivate: [AuthGuard] }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }

export const routingComponents = [
    HomeComponent,
    LoginComponent,
    UsersComponent,
    PurchasesComponent,
    SubscriptionsComponent
];
