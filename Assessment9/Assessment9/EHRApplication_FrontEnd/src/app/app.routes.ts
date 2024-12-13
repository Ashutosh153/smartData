import { Routes } from '@angular/router';
import { HomeComponetComponent } from './MyComponents/home/home-componet/home-componet.component';
import { LoginComponentComponent } from './MyComponents/Login/login-component/login-component.component';
import { PatientRegistrationComponent } from './MyComponents/patient-registration/patient-registration.component';

export const routes: Routes = [
    {
        path:"home",
        component:HomeComponetComponent
        
    },
    {
        path:"",
        component:LoginComponentComponent
    },
    {
        path:"patReg",
        component:PatientRegistrationComponent
    }
];
