import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../_services/user.service';
import { AlertService } from '../_services/alert.service';
import { first } from 'rxjs/operators';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
    registerForm: FormGroup;
    loading: boolean = false;
    submitted: boolean = false;

    constructor(
        private formBuilder: FormBuilder,
        private router: Router,
        private userService: UserService,
        private alertService: AlertService) { }

    ngOnInit() {
        this.registerForm = this.formBuilder.group({
            firstName: ['', Validators.required],
            lastName: ['', Validators.required],
            username: ['', [Validators.required, Validators.email]],
            password: ['', [Validators.required, Validators.minLength(6)]],
            confirmPassword: ['']
        }, { validator: this.repeatPasswordValidate });
    }

    repeatPasswordValidate(group: FormGroup) {
        var password = group.controls.password.value;
        var confirmPassword = group.controls.confirmPassword.value;
        let errors = password === confirmPassword ? null : { passwordsNotEqual: true };

        group.controls.confirmPassword.setErrors(errors);
    }

    get controls() {
        return this.registerForm.controls;
    }

    onSubmit() {
        this.submitted = true;

        if (this.registerForm.invalid)
            return;

        this.loading = true;

        let userData = {
            username: this.controls.username.value,
            password: this.controls.password.value,
            firstName: this.controls.firstName.value,
            lastName: this.controls.lastName.value
        };

        this.userService.register(userData)
            .pipe(first())
            .subscribe(
                data => {
                    this.router.navigate(['/login']);
                },
                error => {
                    this.alertService.error(error);
                    this.loading = false;
                });
    }
}
