import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AlertComponent } from './__directives/alert/alert.component';
import { DialogComponent } from './__directives/dialog/dialog.component';
import { SidebarComponent } from './__directives/sidebar/sidebar.component';
import { TopbarComponent } from './__directives/topbar/topbar.component';
import { GecoDialogModule } from 'angular-dynamic-dialog';

@NgModule({
    declarations: [
        AppComponent,
        AlertComponent,
        DialogComponent,
        SidebarComponent,
        TopbarComponent
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        GecoDialogModule
    ],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule { }
