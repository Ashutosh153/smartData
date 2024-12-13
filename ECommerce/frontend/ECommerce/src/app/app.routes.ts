import { Routes } from '@angular/router';
import { RegestrationComponent } from './MyComponents/regestration/regestration.component';
import { LoginComponent } from './MyComponents/login/login.component';
import { ForgetPasswordComponent } from './MyComponents/forget-password/forget-password.component';
import { DashboardComponent } from './MyComponents/dashboard/dashboard.component';
import { AddProductComponent } from './MyComponents/add-product/add-product.component';
import { ProfileComponent } from './MyComponents/profile/profile.component';
import { TestComponent } from './MyComponents/test/test.component';
import { CartComponent } from './MyComponents/cart/cart.component';
import { ChangePasswordComponent } from './MyComponents/change-password/change-password.component';
import { authGuard } from './guards/auth.guard';
import { InvoiceComponent } from './MyComponents/invoice/invoice.component';

export const routes: Routes = [
    {
        path:'',
        redirectTo:'login',
        pathMatch:'full'
    },
    {
        path:'login',
       component :LoginComponent

    },
    {
        path:"regester",
        component:RegestrationComponent
    },
    {
        path:"dashboard",
        component:DashboardComponent,
        canActivate:[authGuard]
    },
    {
        path:"addProduct",
        component:AddProductComponent,
        canActivate:[authGuard]
    },
    {
        path:"profile",
        component:ProfileComponent,
        canActivate:[authGuard]
    },{
        path:"forgetPassword",
        component:ForgetPasswordComponent
    },
    {
        path:"test",
        component:TestComponent
    },
    {
        path:"cart",
        component:CartComponent,
        canActivate:[authGuard]
    },{
        path:"changePassword",
        component:ChangePasswordComponent
        //canActivate:[authGuard]
    },
    {
        path:'invoice',
        component:InvoiceComponent
    }
    // },{
    //     path:"**",
    //     redirectTo:""
    // }
];
