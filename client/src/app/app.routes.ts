import { Routes } from '@angular/router';
import { Login } from '../features/user/login/login';
import { RegisterTenant } from '../features/user/register-tenant/register-tenant';
import { authGuard } from '../core/guards/auth-guard';
import { InviteMember } from '../features/user/invite-member/invite-member';
import { Home } from '../home/home/home';
import { LandingPage } from '../home/landing-page/landing-page';
import { guestGuard } from '../core/guards/guest-guard';
import { CustomerList } from '../features/customers/customer-list/customer-list';
import { CustomerForm } from '../features/customers/customer-form/customer-form';
import { ServerError } from '../shared/errors/server-error/server-error';
import { NotFound } from '../shared/errors/not-found/not-found';

export const routes: Routes = [
  {path: '',component: LandingPage,canActivate: [guestGuard],runGuardsAndResolvers: 'always'},
 {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [guestGuard],
    children: [
      {
        path: 'landing',
        component: LandingPage
      },
      {
        path: 'login',
        component: Login
      },
      {
        path: 'register',
        component: RegisterTenant
      }
    ]
  },
  
  // αφου εχει κανει login
   {
    path: '',
    component: Home,
    runGuardsAndResolvers: 'always',
    canActivate: [authGuard],
    children: [
      {
        path: 'home',
        component: Home
      },
      { path: 'InviteMember',  component: InviteMember},
      { path: 'customer', component: CustomerList },
      { path: 'customer/new', component: CustomerForm },
      { path: 'customer/:id/edit', component: CustomerForm },
      { path: 'customer/:id', component: CustomerList },
    ]
   }, 
  {path:'server-error',component:ServerError},
  {path:'**',component:NotFound}
];