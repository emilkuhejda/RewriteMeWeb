import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './admin/dashboard/dashboard.component';
import { AdminComponent } from './admin/admin.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AuthGuard } from './_guards/auth.guard';
import { FilesComponent } from './admin/files/files.component';
import { CreateFileComponent } from './admin/files/create-file/create-file.component';
import { EditFileComponent } from './admin/files/edit-file/edit-file.component';

const routes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    {
        path: 'admin',
        component: AdminComponent,
        canActivate: [AuthGuard],
        children: [
            { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
            { path: 'dashboard', component: DashboardComponent },
            { path: 'files', component: FilesComponent },
            { path: 'files/create', component: CreateFileComponent },
            { path: 'file/:fileId', component: EditFileComponent }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }

export const routingComponents = [
    HomeComponent,
    LoginComponent,
    RegisterComponent,
    AdminComponent,
    DashboardComponent,
    FilesComponent,
    CreateFileComponent
]
