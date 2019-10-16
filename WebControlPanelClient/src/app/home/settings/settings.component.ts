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
    cleanUpFormSubmitted: boolean;

    selectedStorage: string;
    loadingStorageSettings: boolean;
    loadingCleanUpForm: boolean;

    constructor(
        private formBuilder: FormBuilder,
        private utilsService: UtilsService,
        private settingsService: SettingsService,
        private alertService: AlertService) { }

    ngOnInit() {
        this.cleanUpForm = this.formBuilder.group({
            deleteBeforeInDays: ['', [Validators.required]],
            cleanUpSettings: ['', [Validators.required]],
            forceCleanUp: [],
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
            () => { },
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
}
