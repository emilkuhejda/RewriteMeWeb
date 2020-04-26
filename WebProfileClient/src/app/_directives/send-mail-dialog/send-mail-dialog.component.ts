import { Component, OnInit, Inject } from '@angular/core';
import { GECO_DATA_DIALOG, GecoDialogRef, GECO_DIALOG_REF } from 'angular-dynamic-dialog';
import { MailService } from 'src/app/_services/mail.service';
import { ErrorResponse } from 'src/app/_models/error-response';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ErrorCode } from 'src/app/_enums/error-code';
import { AlertService } from 'src/app/_services/alert.service';

@Component({
    selector: 'app-send-mail-dialog',
    templateUrl: './send-mail-dialog.component.html',
    styleUrls: ['./send-mail-dialog.component.css']
})
export class SendMailDialogComponent implements OnInit {
    sendMailForm: FormGroup;
    fileItemId: string;
    submitted: boolean;
    loading: boolean;

    constructor(
        @Inject(GECO_DATA_DIALOG) public data: any,
        @Inject(GECO_DIALOG_REF) public dialogRef: GecoDialogRef,
        private formBuilder: FormBuilder,
        private mailService: MailService,
        private alertService: AlertService) {
        this.fileItemId = data.fileItemId;
    }

    ngOnInit() {
        this.sendMailForm = this.formBuilder.group({
            emailAddress: ['', [Validators.required, Validators.email]]
        })
    }

    get controls() {
        return this.sendMailForm.controls;
    }

    onSubmit() {
        this.submitted = true;
        if (this.sendMailForm.invalid)
            return;

        this.loading = true;

        this.mailService.sendEmail(this.controls.emailAddress.value, this.fileItemId).subscribe(
            () => {
                this.alertService.success("The email has been sent successfully");
            },
            (err: ErrorResponse) => {
                let error = err.message;
                if (err.errorCode === ErrorCode.EC101)
                    error = "Sending file was not found";

                if (err.errorCode === ErrorCode.EC202)
                    error = "Email address format is incorrect";

                this.alertService.error(error);
            })
            .add(() => this.close());
    }

    close() {
        this.loading = false;
        this.dialogRef.closeModal();
    }
}
