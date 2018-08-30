import { Component, OnInit, ViewChild } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthService } from '../../../core/auth/auth.service';
import { DialogService } from '../../dialogs/dialog.service';
import { SharedService } from '../../shared.service';
import { MatSnackBar } from '@angular/material';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  user$;
  auth$;
  @ViewChild('drawer') sidenav: any;


  constructor(private breakpointObserver: BreakpointObserver,
              public authService: AuthService,
              private dialogService: DialogService,
              private sharedService: SharedService,
              private snackBar: MatSnackBar) {
  }

  ngOnInit() {
    const user = this.authService.getUser();
    const authenticated = this.authService.isUserAuthenticated();
    this.authService.setAuth(authenticated);
    this.authService.setUser(user);
    this.user$ = this.authService.currentUser;
    this.auth$ = this.authService.isAuthenticated;
  }

  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches)
    );

  changePassword() {
    this.dialogService.changePassword('change')
      .subscribe(dialogRes => {
        if (dialogRes) {
          const credentials = Object.assign({}, dialogRes);
          credentials.newPassword = credentials.password;
          delete credentials.password;
          this.sharedService.resetPassword(credentials)
            .subscribe(() => {
              this.snackBar.open('Password have been changed!', '', {
                duration: 2000,
              });
            }, (err) => {
              this.dialogService.alert(JSON.stringify(err.modelState));
            });
        }
      });
  }
}
