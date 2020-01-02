import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule, routingComponents } from './app-routing.module';
import { AppComponent } from './app.component';
import { AlertComponent } from './_directives/alert/alert.component';
import { DialogComponent } from './_directives/dialog/dialog.component';
import { SidebarComponent } from './_directives/sidebar/sidebar.component';
import { TopbarComponent } from './_directives/topbar/topbar.component';
import { GecoDialogModule } from 'angular-dynamic-dialog';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TokenInterceptorService } from './_services/token-interceptor.service';
import { ErrorInterceptorService } from './_services/error-interceptor.service';
import { CreateSubscriptionDialogComponent } from './_directives/create-subscription-dialog/create-subscription-dialog.component';
import { TruncatePipe } from './_pipes/truncate.pipe';
import { MatTabsModule } from '@angular/material/tabs';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RecognitionStatePipe } from './_pipes/recognition-state.pipe';
import { ConfirmationDialogComponent } from './_directives/confirmation-dialog/confirmation-dialog.component';
import { SubscriptionOperationPipe } from './_pipes/subscription-operation.pipe';
import { ToTimeStringPipe } from './_pipes/to-time-string.pipe';
import { StorageSettingPipe } from './_pipes/storage-setting.pipe';
import { UploadStatusPipe } from './_pipes/upload-status.pipe';

@NgModule({
    declarations: [
        AppComponent,
        AlertComponent,
        DialogComponent,
        CreateSubscriptionDialogComponent,
        ConfirmationDialogComponent,
        SidebarComponent,
        TopbarComponent,
        TruncatePipe,
        routingComponents,
        RecognitionStatePipe,
        SubscriptionOperationPipe,
        ToTimeStringPipe,
        StorageSettingPipe,
        UploadStatusPipe
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        HttpClientModule,
        ReactiveFormsModule,
        FormsModule,
        GecoDialogModule,
        BrowserAnimationsModule,
        MatTabsModule,
        MatButtonToggleModule
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
    entryComponents: [DialogComponent, CreateSubscriptionDialogComponent, ConfirmationDialogComponent]
})
export class AppModule { }
