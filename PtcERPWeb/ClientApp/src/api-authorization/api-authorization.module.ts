import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginMenuComponent } from './login-menu/login-menu.component';
import { LoginComponent } from './login/login.component';
import { LogoutComponent } from './logout/logout.component';
import { RouterModule } from '@angular/router';
import { ApplicationPaths } from './api-authorization.constants';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthorizeInterceptor } from './authorize.interceptor';
import { LoadingBarHttpClientModule } from '@ngx-loading-bar/http-client';
import { LoadingBarRouterModule } from '@ngx-loading-bar/router';
import { LoadingBarModule } from '@ngx-loading-bar/core';

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,
    LoadingBarHttpClientModule,
    LoadingBarRouterModule,
    LoadingBarModule,

    RouterModule.forChild(
      [
        { path: ApplicationPaths.Register, component: LoginComponent },
        { path: ApplicationPaths.Profile, component: LoginComponent },
        { path: ApplicationPaths.Login, component: LoginComponent },
        { path: ApplicationPaths.LoginFailed, component: LoginComponent },
        { path: ApplicationPaths.LoginCallback, component: LoginComponent },
        { path: ApplicationPaths.LogOut, component: LogoutComponent },
        { path: ApplicationPaths.LoggedOut, component: LogoutComponent },
        { path: ApplicationPaths.LogOutCallback, component: LogoutComponent }
      ]
    )
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true }
  ],
  declarations: [LoginMenuComponent, LoginComponent, LogoutComponent],
  exports: [LoginMenuComponent, LoginComponent, LogoutComponent, LoadingBarModule]
})
export class ApiAuthorizationModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: ApiAuthorizationModule,
      providers: [
        {
          provide: HTTP_INTERCEPTORS,
          useClass: AuthorizeInterceptor,
          multi: true
        }
      ]
    };
  }
}
