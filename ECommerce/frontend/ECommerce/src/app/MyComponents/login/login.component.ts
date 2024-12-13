import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserServiceService } from '../../services/User/user-service.service';
import { DetailsOfUsersService } from '../../services/DetailsOfUserService/details-of-users.service';
import { Router, RouterLink } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { timeout } from 'rxjs';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule,ReactiveFormsModule,RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {
  ngOnInit(): void {
    localStorage.removeItem("token")
    localStorage.removeItem("invoiceId")
  }

service=inject(UserServiceService)
toaster = inject(ToastrService)
 router=inject(Router)
 isLoading=false
 validateCrededntials=true
 validateOtp=false
 
  

 
   
    userForm = new FormGroup({  
      userName:new FormControl ('', Validators.required),
      password:new FormControl ('', [Validators.required, Validators.minLength(6)]),
      otp:new FormControl ('', [Validators.required, Validators.pattern(/^\d{6}$/)]), 
    });

    sendOtp()
    {
this.isLoading=true
      console.log(this.userForm.value)
      const getOtpObj={
        userName:this.userForm.value.userName,
        password:this.userForm.value.password
      }
      console.log(getOtpObj)

      this.service.DoGetOtp(getOtpObj).subscribe((res:any)=>{
        if(res.isSuccess)
        {
          console.log("success")
          this.isLoading=false
          // alert(res.message)
          this.toaster.success("Otp send successfully")
          this.validateCrededntials=false
          this.validateOtp=true
        }
      else{
            console.log("fail")
            this.isLoading=false
          this.toaster.error(res.message)
            //alert(res.message)
        } 

      })
    }  
    limitInputLength(event: Event, maxLength: number): void {
      const input = event.target as HTMLInputElement;
      if (input.value.length > maxLength) {
        input.value = input.value.slice(0, maxLength); 
        
      }
    }
    

    onSubmit()
    {
   
      const validateOtpObj={
        userName:this.userForm.value.userName,
        otp:this.userForm.value.otp?.toString()
      }
      this.service.DoVerifyOtpAndLogin(validateOtpObj).subscribe((res:any)=>{
        if(res.isSuccess)
        {
         
          console.log("otp varified successfully")
          this.toaster.success(res.message,"",{timeOut:1000, closeButton:true})
          //alert(res.message)
          localStorage.setItem("token",res.data.token )
          localStorage.setItem("userType",res.data.id)
          console.log(res)

          this.router.navigateByUrl("dashboard")
        
        }
        else{
          
          console.log("fail")
          this.toaster.error(res.message)
          //alert(res.message)
        }
      })


    }
  

}

