import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { AppRoutingModule, routingComponents } from './app-routing.module';
import { TokenInterceptorService } from './_services/token-interceptor.service';
import { AlertComponent } from './_directives/alert/alert.component';
import { HomeNavigationComponent } from './_directives/home-navigation/home-navigation.component';
import { AdminNavigationComponent } from './_directives/admin-navigation/admin-navigation.component';
import { ErrorInterceptorService } from './_services/error-interceptor.service';
import { ProgressBarComponent } from './_directives/progress-bar/progress-bar.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FilesComponent } from './admin/files/files.component';
import { CreateFileComponent } from './admin/files/create-file/create-file.component';

@NgModule({
    declarations: [
        AppComponent,
        AlertComponent,
        HomeNavigationComponent,
        AdminNavigationComponent,
        ProgressBarComponent,
        routingComponents,
        FilesComponent,
        CreateFileComponent
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        HttpClientModule,
        ReactiveFormsModule,
        FormsModule,
        NgbModule
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
    bootstrap: [AppComponent]
})
export class AppModule { }
