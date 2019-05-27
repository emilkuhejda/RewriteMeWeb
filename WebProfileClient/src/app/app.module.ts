import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';

import { AppRoutingModule, routingComponents } from './app-routing.module';
import { AppComponent } from './app.component';
import { SidebarComponent } from './_directives/sidebar/sidebar.component';
import { TopbarComponent } from './_directives/topbar/topbar.component';
import { AlertComponent } from './_directives/alert/alert.component';
import { TokenInterceptorService } from './_services/token-interceptor.service';
import { ErrorInterceptorService } from './_services/error-interceptor.service';
import { RecognitionStatePipe } from './_pipes/recognition-state.pipe';

@NgModule({
    declarations: [
        AppComponent,
        AlertComponent,
        SidebarComponent,
        TopbarComponent,
        RecognitionStatePipe,
        routingComponents
    ],
    imports: [
        BrowserModule,
        HttpClientModule,
        AppRoutingModule
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
