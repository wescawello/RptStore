import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

// Import Containers
import { DefaultLayoutComponent } from './containers/default-layout/default-layout.component';

import { P404Component } from './views/error/404.component';
import { P500Component } from './views/error/500.component';
import { LoginComponent } from './views/login/login.component';
import { RegisterComponent } from './views/register/register.component';
import { AuthorizeGuard } from '../api-authorization/authorize.guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full',
  },
  {
    path: 'home',
    redirectTo: 'dashboard',
    pathMatch: 'full',
  },
  {
    path: '404',
    component: P404Component,
    data: {
      title: 'Page 404'
    }
  },
  {
    path: '500',
    component: P500Component,
    data: {
      title: 'Page 500'
    }
  },
  {
    path: 'login',
    component: LoginComponent,
    data: {
      title: 'Login Page'
    }
  },
  {
    path: 'register',
    component: RegisterComponent,
    data: {
      title: 'Register Page'
    }
  },
  
  {
    path: '',
    component: DefaultLayoutComponent,
    data: {
      title: 'Home'
    },
    //canActivate: [AuthorizeGuard],
    children: [
      {
        path: 'base',
        loadChildren: () => import('./views/base/base.module').then(m => m.BaseModule)
      },
      {
        path: 'buttons',
        loadChildren: () => import('./views/buttons/buttons.module').then(m => m.ButtonsModule)
      },
      {
        path: 'charts',
        loadChildren: () => import('./views/chartjs/chartjs.module').then(m => m.ChartJSModule)
      },
      {
        path: 'dashboard',
        loadChildren: () => import('./views/dashboard/dashboard.module').then(m => m.DashboardModule)
      },
      {
        path: 'icons',
        loadChildren: () => import('./views/icons/icons.module').then(m => m.IconsModule)
      },
      {
        path: 'notifications',
        loadChildren: () => import('./views/notifications/notifications.module').then(m => m.NotificationsModule)
      },
      {
        path: 'theme',
        loadChildren: () => import('./views/theme/theme.module').then(m => m.ThemeModule)
      },
      {
        path: 'widgets',
        loadChildren: () => import('./views/widgets/widgets.module').then(m => m.WidgetsModule)
      },
      {
        path: 'mjm',
        canActivate: [AuthorizeGuard],
        children: [
          {
            path: "vandor",
            data: { title: "廠商" },
            canLoad: [AuthorizeGuard],
            loadChildren: () => import('./major-maintenance/vandor/main.module').then(m => m.MainModule)
          },
          {
            path: "device",
            data: { title: "設備" },
            canLoad: [AuthorizeGuard],
            loadChildren: () => import('./major-maintenance/device/main.module').then(m => m.MainModule)
          },
          {
            path: "formate",
            data: { title: "規格" },
            canLoad: [AuthorizeGuard],
            loadChildren: () => import('./major-maintenance/formate/main.module').then(m => m.FormateModule)
          }, {
            path: "stock-unit",
            data: { title: "倉別" },
            canLoad: [AuthorizeGuard],
            loadChildren: () => import('./major-maintenance/stock-unit/main.module').then(m => m.StockUnitModule)
        
          }, {
            path: "customer-info",
            data: { title: "客戶" },
            canLoad: [AuthorizeGuard],
            loadChildren: () => import('./major-maintenance/customer-info/main.module').then(m => m.CustomerInfoModule)

          }

        ]
      },
      {
        path: 'work',
        canActivate: [AuthorizeGuard],
        children: [
          {
          path: "pick-order",
          canLoad: [AuthorizeGuard],
          loadChildren: () => import('./work/pick-order/main.module').then(m => m.PickOrderModule)
          },
          {
          path: "stock-order",
          canLoad: [AuthorizeGuard],
          loadChildren: () => import('./work/stock-order/main.module').then(m => m.StockOrderModule)
          },
          {
            path: "sale-order",
            canLoad: [AuthorizeGuard],
            loadChildren: () => import('./work/sale-order/main.module').then(m => m.SaleOrderModule)
          },
          {
            path: "return-order",
            canLoad: [AuthorizeGuard],
            loadChildren: () => import('./work/return-order/main.module').then(m => m.ReturnOrderModule)
          },
          {
            path: "purchase",
            canLoad: [AuthorizeGuard],
            loadChildren: () => import('./work/purchase').then(m => m.PurchaseModule)
          },
          {
            path: "infos",
            canLoad: [AuthorizeGuard],
            loadChildren: () => import('./work/infos/main.module').then(m => m.InfoModule)
          },
          {
            path: "cc",
            data: { title: "零件" },
            canLoad: [AuthorizeGuard],
            loadChildren: () => import('./work/equipment-cost/equipment-cost.module').then(m => m.EquipmentCostModule)
          }, 

          {
            path: "ccc",
            data: { title: "零件" },
            canLoad: [AuthorizeGuard],
            loadChildren: () => import('./work/cequipment-cost/equipment-cost.module').then(m => m.EquipmentCostModule)
          }

        ]

      }
    ]

  },
  { path: '**', component: P404Component }
];

@NgModule({
  imports: [ RouterModule.forRoot(routes) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}
