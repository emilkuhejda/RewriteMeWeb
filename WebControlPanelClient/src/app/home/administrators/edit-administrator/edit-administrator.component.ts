import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AdministratorService } from 'src/app/_services/administrator.service';
import { AlertService } from 'src/app/_services/alert.service';
import { first } from 'rxjs/operators';
import { ErrorResponse } from 'src/app/_models/error-response';
import { MustMatch } from 'src/app/_validators/must-match';

@Component({
    selector: 'app-edit-administrator',
    templateUrl: './edit-administrator.component.html',
    styleUrls: ['./edit-administrator.component.css']
})
export class EditAdministratorComponent implements OnInit {
    editForm: FormGroup;
    administratorId: string;
    loading: boolean = false;
    submitted: boolean = false;

    constructor(
        private formBuilder: FormBuilder,
        private router: Router,
        private route: ActivatedRoute,
        private administratorService: AdministratorService,
        private alertService: AlertService) { }

    ngOnInit() {
        this.editForm = this.formBuilder.group({
            firstName: ['', Validators.required],
            lastName: ['', Validators.required],
            username: ['', [Validators.required, Validators.email]],
            password: ['', [Validators.minLength(6)]],
            confirmPassword: ['']
        }, { validator: MustMatch('password', 'confirmPassword') });

        this.route.paramMap.subscribe(paramMap => {
            this.administratorId = paramMap.get("administratorId");
            this.administratorService.get(this.administratorId).subscribe(
                (administrator) => {
                    this.controls.username.setValue(administrator.username);
                    this.controls.firstName.setValue(administrator.firstName);
                    this.controls.lastName.setValue(administrator.lastName);
                },
                (err: ErrorResponse) => {
                    this.alertService.error(err.message);
                });
        });
    }

    get controls() {
        return this.editForm.controls;
    }

    onSubmit() {
        this.submitted = true;

        if (this.editForm.invalid)
            return;

        this.loading = true;

        let userData = {
            id: this.administratorId,
            username: this.controls.username.value,
            password: this.controls.password.value,
            firstName: this.controls.firstName.value,
            lastName: this.controls.lastName.value
        };

        this.administratorService.update(userData)
            .pipe(first())
            .subscribe(
                () => {
                    this.router.navigate(['/administrators']);
                },
                (err: ErrorResponse) => {
                    let error = err.status === 409 ? "User already exists" : err.message;
                    this.alertService.error(error);
                    this.loading = false;
                });
    }
}
