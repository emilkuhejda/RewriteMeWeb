import { Component, OnInit } from '@angular/core';
import { UtilsService } from 'src/app/_services/utils.service';
import { SettingsService } from 'src/app/_services/settings.service';
import { AlertService } from 'src/app/_services/alert.service';
import { ErrorResponse } from 'src/app/_models/error-response';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
    selector: 'app-settings',
    templateUrl: './settings.component.html',
    styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {
    cleanUpForm: FormGroup;
    resetForm: FormGroup;
    cleanUpFormSubmitted: boolean;
    resetFormSubmitted: boolean;
    loadingCleanUpForm: boolean;
    loadingResetForm: boolean;
    loadingStorageSettings: boolean;
    selectedStorage: string;

    constructor(
        private formBuilder: FormBuilder,
        private utilsService: UtilsService,
        private settingsService: SettingsService,
        private alertService: AlertService) { }

    ngOnInit() {
        this.cleanUpForm = this.formBuilder.group({
            deleteBeforeInDays: ['', [Validators.required]],
            cleanUpSettings: ['', [Validators.required]],
            forceCleanUp: [false],
            password: ['', [Validators.required]]
        });

        this.resetForm = this.formBuilder.group({
            password: ['', [Validators.required]]
        });

        this.initializeStorageSetting();
    }

    private initializeStorageSetting() {
        this.loadingStorageSettings = true;

        this.settingsService.getStorageSetting().subscribe(
            storageSetting => {
                this.selectedStorage = storageSetting.toString();
            },
            (err: ErrorResponse) => {
                this.alertService.error(err.message);
            })
            .add(() => {
                this.loadingStorageSettings = false;
            });
    }

    public onValueChange(value: string) {
        this.settingsService.changeStorage(value).subscribe(
            () => {
                this.initializeStorageSetting();
            },
            (err: ErrorResponse) => {
                this.alertService.error(err.message);
            });
    }

    get cleanUpControls() {
        return this.cleanUpForm.controls;
    }

    onCleanUpFormSubmit() {
        this.alertService.clear();
        this.cleanUpFormSubmitted = true;

        if (this.cleanUpForm.invalid)
            return;

        this.loadingCleanUpForm = true;

        let formData = {
            deleteBeforeInDays: this.cleanUpControls.deleteBeforeInDays.value,
            cleanUpSettings: this.cleanUpControls.cleanUpSettings.value,
            forceCleanUp: this.cleanUpControls.forceCleanUp.value,
            password: this.cleanUpControls.password.value
        };

        this.settingsService.cleanUp(formData).subscribe(
            () => {
                this.cleanUpControls.deleteBeforeInDays.setValue('');
                this.cleanUpControls.cleanUpSettings.setValue('');
                this.cleanUpControls.forceCleanUp.setValue(false);
                this.cleanUpControls.password.setValue('');

                this.cleanUpFormSubmitted = false;

                this.alertService.success("Clean up has been successfully started.");
            },
            (err: ErrorResponse) => {
                let error = err.message;

                if (err.status === 400)
                    error = "Password is not correct!";

                this.alertService.error(error);
            })
            .add(() => {
                this.loadingCleanUpForm = false;
            });
    }

    get resetControls() {
        return this.resetForm.controls;
    }

    resetFormSubmit() {
        this.alertService.clear();
        this.resetFormSubmitted = true;

        if (this.resetForm.invalid)
            return;

        this.loadingResetForm = true;

        let formData = {
            password: this.resetControls.password.value
        };

        this.utilsService.resetDatabase(formData).subscribe(
            () => {
                this.resetControls.password.setValue('');
                this.resetFormSubmitted = false;
                this.alertService.success('Database was successfully reseted.');
            },
            (err: ErrorResponse) => {
                let error = err.message;

                if (err.status === 400)
                    error = "Password is not correct!";

                this.alertService.error(error);
            })
            .add(() => {
                this.loadingResetForm = false;
            });
    }
}
