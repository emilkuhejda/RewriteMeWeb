import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MsalService } from '../_services/msal.service';
import { CommonVariables } from '../_config/common-variables';
import { ContactFormService } from '../_services/contact-form.service';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
    contactForm: FormGroup;
    submitted: boolean;
    loading: boolean;
    successMessage: string;
    errorMessage: string;

    constructor(
        private formBuilder: FormBuilder,
        private contactFormService: ContactFormService,
        private msalService: MsalService) { }

    ngOnInit() {
        this.contactForm = this.formBuilder.group({
            name: ['', Validators.required],
            email: ['', [Validators.required, Validators.email]],
            message: ['', Validators.required]
        });
    }

    navigateToProfile(): void {
        if (!this.msalService.isLoggedIn()) {
            location.reload(true);
        }

        window.open(CommonVariables.ApiUrl + CommonVariables.Profile, '_blank');
    }

    get controls() {
        return this.contactForm.controls;
    }

    onSubmit() {
        this.submitted = true;
        this.successMessage = "";
        this.errorMessage = "";

        if (this.contactForm.invalid)
            return;

        this.loading = true;

        let contactFormData = {
            name: this.controls.name.value,
            email: this.controls.email.value,
            message: this.controls.message.value
        };

        this.contactFormService.create(contactFormData).subscribe(
            () => {
                this.successMessage = "Contact form was successfully sent.";
            },
            () => {
                this.errorMessage = "Contact form was not successfully sent. Please, try it later."
            }
        ).add(
            () => { this.loading = false }
        );
    }

    login(): void {
        this.msalService.login();
    }

    logout(): void {
        this.msalService.logout();
    }

    isUserLoggedIn(): boolean {
        return this.msalService.isLoggedIn();
    }
}
