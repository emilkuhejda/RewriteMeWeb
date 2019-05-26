import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './_guards/auth.guard';
import { FilesComponent } from './files/files.component';
import { CreateFileComponent } from './files/create-file/create-file.component';

const routes: Routes = [
    { path: '', redirectTo: 'files', pathMatch: 'full', canActivate: [AuthGuard] },
    { path: 'files', component: FilesComponent, canActivate: [AuthGuard] },
    { path: 'files/create', component: CreateFileComponent, pathMatch: 'full', canActivate: [AuthGuard] }

];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }

export const routingComponents = [
    FilesComponent,
    CreateFileComponent
]
