import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ContactFormService } from '../_services/contact-form.service';
import { DynamicScriptLoaderService } from '../_services/dynamic-script-loader.service';
import { BaseComponent } from '../base/base.component';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css']
})
export class HomeComponent extends BaseComponent {
    contactForm: FormGroup;
    submitted: boolean;
    loading: boolean;
    successMessage: string;
    errorMessage: string;

    constructor(
        private formBuilder: FormBuilder,
        private contactFormService: ContactFormService,
        protected dynamicScriptLoaderService: DynamicScriptLoaderService) {
        super(dynamicScriptLoaderService);
    }

    ngOnInit() {
        this.contactForm = this.formBuilder.group({
            name: ['', Validators.required],
            email: ['', [Validators.required, Validators.email]],
            message: ['', Validators.required]
        });

        this.loadScripts();
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
}
