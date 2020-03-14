import { Component, OnInit } from '@angular/core';
import { UtilsService } from 'src/app/_services/utils.service';
import { SettingsService } from 'src/app/_services/settings.service';
import { AlertService } from 'src/app/_services/alert.service';
import { ErrorResponse } from 'src/app/_models/error-response';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { StorageSetting } from 'src/app/_enums/storage-setting';
import { AzureStorageService } from 'src/app/_service/azure-storage.service';

@Component({
    selector: 'app-settings',
    templateUrl: './settings.component.html',
    styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {
    cleanUpForm: FormGroup;
    databaseForm: FormGroup;
    cleanUpFormSubmitted: boolean;
    databaseFormSubmitted: boolean;
    loadingCleanUpForm: boolean;
    loadingDatabaseForm: boolean;
    loadingStorageSettings: boolean;
    loadingChunksStorageSettings: boolean;
    loadingDatabaseBackupSettings: boolean;
    loadingNotificationsSettings: boolean;
    selectedStorage: string;
    selectedChunksStorage: string;
    isEnabledNotifications: string;
    isEnabledDatabaseBackup: string;

    constructor(
        private formBuilder: FormBuilder,
        private utilsService: UtilsService,
        private settingsService: SettingsService,
        private azureStorageService: AzureStorageService,
        private alertService: AlertService) { }

    ngOnInit() {
        this.cleanUpForm = this.formBuilder.group({
            deleteBeforeInDays: ['', [Validators.required]],
            cleanUpSettings: ['', [Validators.required]],
            forceCleanUp: [false],
            password: ['', [Validators.required]]
        });

        this.databaseForm = this.formBuilder.group({
            password: ['', [Validators.required]]
        });

        this.initializeStorageSetting();
        this.initializeChunksStorageSetting();
        this.initializeDatabaseBackupSetting();
        this.initializeNotificationsSetting();
    }

    private initializeStorageSetting() {
        this.loadingStorageSettings = true;

        this.settingsService.getStorageSetting().subscribe(
            storageSetting => {
                this.selectedStorage = StorageSetting[storageSetting].toString();
            },
            (err: ErrorResponse) => {
                this.alertService.error(err.message);
            })
            .add(() => {
                this.loadingStorageSettings = false;
            });
    }

    private initializeChunksStorageSetting() {
        this.loadingChunksStorageSettings = true;

        this.settingsService.getChunksStorageSetting().subscribe(
            storageSetting => {
                this.selectedChunksStorage = StorageSetting[storageSetting].toString();
            },
            (err: ErrorResponse) => {
                this.alertService.error(err.message);
            })
            .add(() => {
                this.loadingChunksStorageSettings = false;
            });
    }

    private initializeDatabaseBackupSetting() {
        this.loadingDatabaseBackupSettings = true;

        this.settingsService.getDatabaseBackupSetting().subscribe(
            isEnabled => {
                this.isEnabledDatabaseBackup = String(isEnabled);
            },
            (err: ErrorResponse) => {
                this.alertService.error(err.message);
            })
            .add(() => {
                this.loadingDatabaseBackupSettings = false;
            });
    }

    private initializeNotificationsSetting() {
        this.loadingNotificationsSettings = true;

        this.settingsService.getNotificationsSetting().subscribe(
            isEnabled => {
                this.isEnabledNotifications = String(isEnabled);
            },
            (err: ErrorResponse) => {
                this.alertService.error(err.message);
            })
            .add(() => {
                this.loadingNotificationsSettings = false;
            });
    }

    public onStorageSettingValueChange(value: string) {
        this.settingsService.changeStorage(value).subscribe(
            () => {
                this.initializeStorageSetting();
            },
            (err: ErrorResponse) => {
                this.alertService.error(err.message);
            });
    }

    public onChunksStorageSettingValueChange(value: string) {
        this.settingsService.changeChunksStorage(value).subscribe(
            () => {
                this.initializeChunksStorageSetting();
            },
            (err: ErrorResponse) => {
                this.alertService.error(err.message);
            });
    }

    public onDatabaseBackupSettingValueChange(value: boolean) {
        this.settingsService.changeDatabaseBackupSettings(value).subscribe(
            () => {
                this.initializeDatabaseBackupSetting();
            },
            (err: ErrorResponse) => {
                this.alertService.error(err.message);
            });
    }

    public onNotificationsSettingValueChange(value: boolean) {
        this.settingsService.changeNotificationsSetting(value).subscribe(
            () => {
                this.initializeNotificationsSetting();
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
            deleteBeforeInDays: Number(this.cleanUpControls.deleteBeforeInDays.value),
            cleanUpSettings: Number(this.cleanUpControls.cleanUpSettings.value),
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

    get databaseFormControls() {
        return this.databaseForm.controls;
    }

    resetSubmit() {
        this.alertService.clear();
        this.databaseFormSubmitted = true;

        if (this.databaseForm.invalid)
            return;

        this.loadingDatabaseForm = true;

        let formData = {
            password: this.databaseFormControls.password.value
        };

        this.utilsService.resetDatabase(formData).subscribe(
            () => {
                this.databaseFormControls.password.setValue('');
                this.databaseFormSubmitted = false;
                this.alertService.success("Database was successfully reseted.");
            },
            (err: ErrorResponse) => {
                let error = err.message;

                if (err.status === 400)
                    error = "Password is not correct!";

                this.alertService.error(error);
            })
            .add(() => {
                this.loadingDatabaseForm = false;
            });
    }

    deleteSubmit() {
        this.alertService.clear();
        this.databaseFormSubmitted = true;

        if (this.databaseForm.invalid)
            return;

        this.loadingDatabaseForm = true;

        let formData = {
            password: this.databaseFormControls.password.value
        };

        this.utilsService.deleteDatabase(formData).subscribe(
            () => {
                this.databaseFormControls.password.setValue('');
                this.databaseFormSubmitted = false;
                this.alertService.success("Database was successfully deleted.");
            },
            (err: ErrorResponse) => {
                let error = err.message;

                if (err.status === 400)
                    error = "Password is not correct!";

                this.alertService.error(error);
            })
            .add(() => {
                this.loadingDatabaseForm = false;
            });
    }

    cleanUpOutdatedFiles() {
        this.settingsService.cleanOutdatedChunks().subscribe(
            () => {
                this.alertService.success("Outdated files was deleted.");
            },
            (err: ErrorResponse) => {
                this.alertService.error(err.message);
            });
    }

    migrate() {
        this.alertService.clear();

        this.azureStorageService.migrate().subscribe(
            () => {
                this.alertService.success('Job was queued.');
            },
            (err: ErrorResponse) => {
                let error = err.message;
                if (err.status === 400)
                    error = 'Some background jobs are in progress.';

                this.alertService.error(error);
            }
        );
    }
}
