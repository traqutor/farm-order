import {Component, OnInit} from '@angular/core';
import {AuthService} from "../core/auth/auth.service";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {ActivatedRoute, Params, Router} from "@angular/router";
import {PasswordValidation} from "../shared/validators/match-password.validator";
import {UsersService} from "../users/users.service";

@Component({
  selector: 'app-password-recovery',
  templateUrl: './password-recovery.component.html',
  styleUrls: ['./password-recovery.component.css']
})
export class PasswordRecoveryComponent implements OnInit {


  private passwordRecoveryToken: string;
  passwordRecoveryEmail: string;

  public isOk: boolean;
  public isError: boolean;
  public errorMessage: string;
  public recoveryForm: FormGroup;
  public email: string;

  constructor(
    private auth: AuthService,
    private userService: UsersService,
    private formBuilder: FormBuilder,
    private router: Router,
    private route: ActivatedRoute) {
  }

  ngOnInit() {

    this.route.queryParams
      .subscribe(
        (params: Params) => {
          this.passwordRecoveryToken = params['token'];
          this.passwordRecoveryEmail = params['email'];
          console.log('passwordRecoveryToken', this.passwordRecoveryToken);
          console.log('passwordRecoveryEmail', this.passwordRecoveryEmail);
        });

    this.recoveryForm = this.formBuilder.group({
      password: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', [Validators.required]],
    }, {validator: PasswordValidation.MatchPassword});


  }

  get password() {
    return this.recoveryForm.get('password');
  }

  get confirmPassword() {
    return this.recoveryForm.get('confirmPassword');
  }

  getPasswordErrorMessage() {
    return this.password.hasError('required') ? 'Password is required' :
      this.password.hasError('minlength') ? 'At lest 8 characters long' :
        '';
  }

  getConfirmPasswordErrorMessage() {
    return this.confirmPassword.hasError('required') ? 'Password i required' :
      this.confirmPassword.hasError('MatchPassword') ? 'Password do not match' :
        '';
  }

  onPasswordChange() {
    if (this.recoveryForm.valid) {
      return this.userService.onPasswordRecovery(
        this.passwordRecoveryToken,
        this.passwordRecoveryEmail,
        this.recoveryForm.value.password,
        this.recoveryForm.value.confirmPassword)
        .subscribe(() => {
          this.isOk = true;
          this.isError = false;
          this.router.navigateByUrl('/login');
        }, (error) => {
          this.isError = true;
          this.errorMessage = error.error;
          console.error('Password change ERROR: ', error.error);
        });
    }
  }

}
