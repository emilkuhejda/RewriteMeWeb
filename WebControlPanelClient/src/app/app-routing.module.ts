import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './_guards/auth.guard';
import { LoginGuard } from './_guards/login.guard';
import { UsersComponent } from './home/users/users.component';
import { PurchasesComponent } from './home/users/purchases/purchases.component';
import { SubscriptionsComponent } from './home/users/subscriptions/subscriptions.component';
import { AdministratorsComponent } from './home/administrators/administrators.component';
import { CreateAdministratorComponent } from './home/administrators/create-administrator/create-administrator.component';
import { EditAdministratorComponent } from './home/administrators/edit-administrator/edit-administrator.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { ContactFormsComponent } from './home/contact-forms/contact-forms.component';
import { DetailContactFormComponent } from './home/contact-forms/detail-contact-form/detail-contact-form.component';
import { DetailPurchaseComponent } from './home/users/purchases/detail-purchase/detail-purchase.component';

const routes: Routes = [
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    { path: 'home', component: HomeComponent, canActivate: [AuthGuard] },
    { path: 'login', component: LoginComponent, canActivate: [LoginGuard] },
    { path: 'users', component: UsersComponent, canActivate: [AuthGuard] },
    { path: 'users/purchases/:userId', component: PurchasesComponent, canActivate: [AuthGuard] },
    { path: 'users/purchases/:userId/:purchaseId', component: DetailPurchaseComponent, canActivate: [AuthGuard] },
    { path: 'users/subscriptions/:userId', component: SubscriptionsComponent, canActivate: [AuthGuard] },
    { path: 'administrators', component: AdministratorsComponent, canActivate: [AuthGuard] },
    { path: 'administrators/create', component: CreateAdministratorComponent, canActivate: [AuthGuard] },
    { path: 'administrators/edit/:administratorId', component: EditAdministratorComponent, canActivate: [AuthGuard] },
    { path: 'contact-forms', component: ContactFormsComponent, canActivate: [AuthGuard] },
    { path: 'contact-forms/:contactFormId', component: DetailContactFormComponent, canActivate: [AuthGuard] },
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
    LoginComponent,
    UsersComponent,
    PurchasesComponent,
    DetailPurchaseComponent,
    SubscriptionsComponent,
    AdministratorsComponent,
    CreateAdministratorComponent,
    EditAdministratorComponent,
    ContactFormsComponent,
    DetailContactFormComponent,
    NotFoundComponent
];
