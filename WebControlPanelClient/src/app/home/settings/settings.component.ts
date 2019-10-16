import { Component, OnInit } from '@angular/core';
import { UtilsService } from 'src/app/_services/utils.service';
import { SettingsService } from 'src/app/_services/settings.service';
import { AlertService } from 'src/app/_services/alert.service';
import { ErrorResponse } from 'src/app/_models/error-response';

@Component({
    selector: 'app-settings',
    templateUrl: './settings.component.html',
    styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {
    selectedStorage: string;
    isLoadingStorageSettings: boolean;

    constructor(
        private utilsService: UtilsService,
        private settingsService: SettingsService,
        private alertService: AlertService) { }

    ngOnInit() {
        this.initializeStorageSetting();
    }

    private initializeStorageSetting() {
        this.isLoadingStorageSettings = true;

        this.settingsService.getStorageSetting().subscribe(
            storageSetting => {
                this.selectedStorage = storageSetting.toString();
            },
            (err: ErrorResponse) => {
                this.alertService.error(err.message);
            })
            .add(() => {
                this.isLoadingStorageSettings = false;
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

    cleanUp() {
        console.log("cleanup");
    }
}
