import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AdministratorService } from 'src/app/_services/administrator.service';
import { AlertService } from 'src/app/_services/alert.service';
import { first } from 'rxjs/operators';
import { ErrorResponse } from 'src/app/_models/error-response';
import { MustMatch } from 'src/app/_validators/must-match';
import { Location } from '@angular/common';

@Component({
    selector: 'app-create-administrator',
    templateUrl: './create-administrator.component.html',
    styleUrls: ['./create-administrator.component.css']
})
export class CreateAdministratorComponent implements OnInit {
    createForm: FormGroup;
    loading: boolean = false;
    submitted: boolean = false;

    constructor(
        private location: Location,
        private formBuilder: FormBuilder,
        private router: Router,
        private administratorService: AdministratorService,
        private alertService: AlertService) { }

    ngOnInit() {
        this.createForm = this.formBuilder.group({
            username: ['', [Validators.required, Validators.email]],
            firstName: ['', Validators.required],
            lastName: ['', Validators.required],
            password: ['', [Validators.required, Validators.minLength(6)]],
            confirmPassword: ['']
        }, { validator: MustMatch('password', 'confirmPassword') });
    }

    goBack() {
        this.location.back();
    }

    get controls() {
        return this.createForm.controls;
    }

    onSubmit() {
        this.submitted = true;

        if (this.createForm.invalid)
            return;

        this.loading = true;

        let userData = {
            username: this.controls.username.value,
            password: this.controls.password.value,
            firstName: this.controls.firstName.value,
            lastName: this.controls.lastName.value
        };

        this.administratorService.create(userData)
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
