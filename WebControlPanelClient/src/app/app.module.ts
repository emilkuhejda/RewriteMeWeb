import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AlertComponent } from './_directives/alert/alert.component';
import { DialogComponent } from './_directives/dialog/dialog.component';
import { SidebarComponent } from './_directives/sidebar/sidebar.component';
import { TopbarComponent } from './_directives/topbar/topbar.component';
import { GecoDialogModule } from 'angular-dynamic-dialog';
import { TokenInterceptorService } from './_service/token-interceptor.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { ErrorInterceptorService } from './_service/error-interceptor.service';

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
    providers: [{
        provide: HTTP_INTERCEPTORS,
        useClass: TokenInterceptorService,
        multi: true
    },
    {
        provide: HTTP_INTERCEPTORS,
        useClass: ErrorInterceptorService,
        multi: true
    }],
    bootstrap: [AppComponent],
    entryComponents: [DialogComponent]
})
export class AppModule { }
