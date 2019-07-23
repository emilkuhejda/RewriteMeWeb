import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { GecoDialogModule } from 'angular-dynamic-dialog';

import { AppRoutingModule, routingComponents } from './app-routing.module';
import { AppComponent } from './app.component';
import { SidebarComponent } from './_directives/sidebar/sidebar.component';
import { TopbarComponent } from './_directives/topbar/topbar.component';
import { AlertComponent } from './_directives/alert/alert.component';
import { TokenInterceptorService } from './_services/token-interceptor.service';
import { ErrorInterceptorService } from './_services/error-interceptor.service';
import { RecognitionStatePipe } from './_pipes/recognition-state.pipe';
import { ExportAsModule } from 'ngx-export-as';
import { ExportDialogComponent } from './_directives/export-dialog/export-dialog.component';
import { DialogComponent } from './_directives/dialog/dialog.component';
import { ProgressBarComponent } from './_directives/progress-bar/progress-bar.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
    declarations: [
        AppComponent,
        AlertComponent,
        SidebarComponent,
        TopbarComponent,
        RecognitionStatePipe,
        routingComponents,
        ExportDialogComponent,
        DialogComponent,
        ProgressBarComponent
    ],
    imports: [
        BrowserModule,
        HttpClientModule,
        AppRoutingModule,
        ReactiveFormsModule,
        FormsModule,
        ExportAsModule,
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
    entryComponents: [ExportDialogComponent, DialogComponent]
})
export class AppModule { }
