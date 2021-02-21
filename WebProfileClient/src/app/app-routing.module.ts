import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './_guards/auth.guard';
import { FilesComponent } from './files/files.component';
import { CreateFileComponent } from './files/create-file/create-file.component';
import { DetailFileComponent } from './files/detail-file/detail-file.component';
import { EditFileComponent } from './files/edit-file/edit-file.component';
import { AccountComponent } from './account/account.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { DetailMessageComponent } from './messages/detail-message/detail-message.component';
import { MessagesComponent } from './messages/messages.component';
import { RecycleBinComponent } from './recycle-bin/recycle-bin.component';
import { DeactivateCreateFileGuard } from './_guards/deactivate-create-file.guard';

const routes: Routes = [
    { path: '', redirectTo: 'files', pathMatch: 'full' },
    { path: 'files', component: FilesComponent, canActivate: [AuthGuard] },
    { path: 'files/create', component: CreateFileComponent, pathMatch: 'full', canActivate: [AuthGuard], canDeactivate: [DeactivateCreateFileGuard] },
    { path: 'files/edit/:fileId', component: EditFileComponent, pathMatch: 'full', canActivate: [AuthGuard] },
    { path: 'files/:fileId', component: DetailFileComponent, pathMatch: 'full', canActivate: [AuthGuard] },
    { path: 'account', component: AccountComponent, pathMatch: 'full', canActivate: [AuthGuard] },
    { path: 'messages', component: MessagesComponent, pathMatch: 'full', canActivate: [AuthGuard] },
    { path: 'messages/:messageId', component: DetailMessageComponent, pathMatch: 'full', canActivate: [AuthGuard] },
    { path: 'recycle-bin', component: RecycleBinComponent, pathMatch: 'full', canActivate: [AuthGuard] },
    { path: '404', component: NotFoundComponent },
    { path: '**', redirectTo: '404' }
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
    DetailFileComponent,
    AccountComponent,
    MessagesComponent,
    DetailMessageComponent,
    RecycleBinComponent,
    NotFoundComponent
]
