import { Routes } from '@angular/router';
import { HomeComponetComponent } from './MyComponents/home/home-componet/home-componet.component';
import { LoginComponentComponent } from './MyComponents/Login/login-component/login-component.component';
import { PatientRegistrationComponent } from './MyComponents/patient-registration/patient-registration.component';
import { PractionerRegestrationComponent } from './MyComponents/practioner-regestration/practioner-regestration.component';
import { ForgetPasswordComponent } from './MyComponents/forget-password/forget-password.component';
import { DashboardComponent } from './MyComponents/dashboard/dashboard.component';
import { UpdateProfileComponent } from './MyComponents/update-profile/update-profile.component';
import { UpdatePractitionerProfileComponent } from './MyComponents/update-practitioner-profile/update-practitioner-profile.component';
import { GoToAppointmentComponent } from './MyComponents/go-to-appointment/go-to-appointment.component';
import { authGuard } from './guards/auth.guard';
import { ViewCompleteAppointmentsComponent } from './MyComponents/view-complete-appointments/view-complete-appointments.component';

export const routes: Routes = [
    {
        path:"",
        component:HomeComponetComponent
        
    },
    {
        path:"login/:id",
        component:LoginComponentComponent
    },
    {
        path:"PatientRegestration",
        component:PatientRegistrationComponent
    },
    {
        path:"PractionerRegestration",
        component:PractionerRegestrationComponent
    },
    {
        path:"forgetPassword",
        component:ForgetPasswordComponent
    },
    {
        path:"dashboard",
        component:DashboardComponent,
        canActivate:[authGuard]
    },
    {
        path:"updateProfile",
        component:UpdateProfileComponent,
        canActivate:[authGuard]
    },{
        path:"updatePractProfile",
        component:UpdatePractitionerProfileComponent,
        canActivate:[authGuard]
    },{
        path:"goToAppointment/:id",
        component:GoToAppointmentComponent,
        canActivate:[authGuard]
    },
    {
        path:"viewCompleteAppointment",
        component:ViewCompleteAppointmentsComponent,
        canActivate:[authGuard]
    }
];
