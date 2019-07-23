import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './_guards/auth.guard';
import { FilesComponent } from './files/files.component';
import { CreateFileComponent } from './files/create-file/create-file.component';
import { DetailFileComponent } from './files/detail-file/detail-file.component';
import { EditFileComponent } from './files/edit-file/edit-file.component';

const routes: Routes = [
    { path: '', redirectTo: 'files', pathMatch: 'full' },
    { path: 'files', component: FilesComponent, canActivate: [AuthGuard] },
    { path: 'files/create', component: CreateFileComponent, pathMatch: 'full', canActivate: [AuthGuard] },
    { path: 'files/edit/:fileId', component: EditFileComponent, pathMatch: 'full', canActivate: [AuthGuard] },
    { path: 'files/:fileId', component: DetailFileComponent, pathMatch: 'full', canActivate: [AuthGuard] }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }

export const routingComponents = [
    FilesComponent,
    CreateFileComponent,
    EditFileComponent,
    DetailFileComponent
]
