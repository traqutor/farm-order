import { Component, OnInit, ViewChild } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthService } from '../../../core/auth/auth.service';

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
              public authService: AuthService) {
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
}
