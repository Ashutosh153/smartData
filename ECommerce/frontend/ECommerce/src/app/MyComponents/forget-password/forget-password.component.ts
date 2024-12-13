import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserServiceService } from '../../services/User/user-service.service';
import { Router, RouterLink } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-forget-password',
  standalone: true,
  imports: [CommonModule,ReactiveFormsModule,RouterLink],
  templateUrl: './forget-password.component.html',
  styleUrl: './forget-password.component.css'
})
export class ForgetPasswordComponent {
  service=inject(UserServiceService)
  router=inject(Router)
  isLoading:boolean=false
  toastr=inject(ToastrService)

 forgotPasswordForm = new FormGroup({
    email:new FormControl ('', [Validators.required, Validators.email]) // Email is required and must be a valid email
  });


  onSubmit()
  { 
    this.isLoading=true
    console.log(this.forgotPasswordForm.get('email')?.value)
    var email=this.forgotPasswordForm.get('email')?.value
    console.log("email value is "+email)
    this.service.DoForgetPassword(email).subscribe({
      next:((res:any)=>{
        if(res.isSuccess)
        { this.isLoading=false 
            this.toastr.success(res.message)
           // alert(res.message)
          this.router.navigateByUrl("");
        }
        else{
          this.isLoading=false
          this.toastr.error(res.message)
          
        }
      }
     ),
     error:(e:any)=>{
      this.isLoading=false
      console.log(e)}
      
    })
    
  }


}






