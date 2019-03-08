import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { GecoDialogModule } from 'angular-dynamic-dialog';

import { AppComponent } from './app.component';
import { AppRoutingModule, routingComponents } from './app-routing.module';
import { TokenInterceptorService } from './_services/token-interceptor.service';
import { AlertComponent } from './_directives/alert/alert.component';
import { HomeNavigationComponent } from './_directives/home-navigation/home-navigation.component';
import { AdminNavigationComponent } from './_directives/admin-navigation/admin-navigation.component';
import { ErrorInterceptorService } from './_services/error-interceptor.service';
import { ProgressBarComponent } from './_directives/progress-bar/progress-bar.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { DialogComponent } from './_directives/dialog/dialog.component';
import { EditFileComponent } from './admin/files/edit-file/edit-file.component';
import { RecognitionStatePipe } from './_pipes/recognition-state.pipe';

@NgModule({
    declarations: [
        AppComponent,
        AlertComponent,
        HomeNavigationComponent,
        AdminNavigationComponent,
        ProgressBarComponent,
        DialogComponent,
        routingComponents,
        EditFileComponent,
        RecognitionStatePipe
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        HttpClientModule,
        ReactiveFormsModule,
        FormsModule,
        NgbModule,
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
