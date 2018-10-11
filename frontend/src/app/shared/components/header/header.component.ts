import {Component, HostBinding, OnInit, ViewChild} from '@angular/core';
import {BreakpointObserver, Breakpoints} from '@angular/cdk/layout';
import {OverlayContainer} from "@angular/cdk/overlay";
import {MatSnackBar} from '@angular/material';
import {Observable} from 'rxjs';
import {map} from 'rxjs/operators';

import {AuthService} from '../../../core/auth/auth.service';
import {DialogService} from '../../dialogs/dialog.service';
import {SharedService} from '../../shared.service';
import {User} from "../../models/user";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  user$;
  auth$;
  user: User;

  @ViewChild('drawer') sidenav: any;
  @HostBinding('class') componentCssClass;

  constructor(private breakpointObserver: BreakpointObserver,
              public authService: AuthService,
              private dialogService: DialogService,
              private sharedService: SharedService,
              public overlayContainer: OverlayContainer,
              private snackBar: MatSnackBar) {
  }

  ngOnInit() {
    const user = this.authService.getUser();
    const authenticated = this.authService.isUserAuthenticated();
    this.authService.setAuth(authenticated);
    this.authService.setUser(user);
    this.user$ = this.authService.currentUser;
    this.auth$ = this.authService.isAuthenticated;

    this.user$.subscribe((user: User) => {
      this.user = user;
      this.onSetTheme( this.user && this.user.customer && this.user.customer.cssFilePath ? this.user.customer.cssFilePath : 'default-theme');
    });

    this.isHandset$.subscribe(res => {
    });

  }

  onSetTheme(theme) {
    this.overlayContainer.getContainerElement().classList.add(theme);
    this.componentCssClass = theme;
  }

  isHandset$: Observable<boolean> = this.breakpointObserver.observe([
    Breakpoints.XSmall,
    Breakpoints.Small,
    Breakpoints.Handset
  ])
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
