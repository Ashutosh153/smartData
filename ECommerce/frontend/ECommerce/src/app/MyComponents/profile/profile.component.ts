import { Component, inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { DetailsOfUsersService, userDetailsModel } from '../../services/DetailsOfUserService/details-of-users.service';
import { CommonModule } from '@angular/common';
import { jwtDecode } from 'jwt-decode';
import { UserServiceService } from '../../services/User/user-service.service';
import { Router, RouterLink } from '@angular/router';
import { routes } from '../../app.routes';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule,ReactiveFormsModule,RouterLink],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent  {

  router=inject(Router)
profile:any

oldProfile?:string
id?:number
service=inject(UserServiceService)

  
userDeatilsService=inject(DetailsOfUsersService);

LoginUser?:userDetailsModel;
toastr=inject(ToastrService)
TodaysDate:Date=new Date
maxDate = this.TodaysDate.toISOString().substring(0, 10);


constructor()
{
  this.getUserValue()
  
  this.getAllCountry()
  
    
  }




  imageFile: File | null = null;

  onFileChange(event: any): void {
    const file = event.target.files[0]; // Get the selected file
    if (file) {
      this.imageFile = file;
    }
  }

  
  limitInputLength(event: Event, maxLength: number): void {
    const input = event.target as HTMLInputElement;
    if (input.value.length > maxLength) {
      input.value = input.value.slice(0, maxLength); 
    }
  }
  onSubmit(): void {

    

    if (this.profileForm.valid) {

      const formData=new FormData
    Object.keys(this.profileForm.controls).forEach(key => {
      const value = this.profileForm.get(key)?.value;
      
      if (key === 'profileImage' && this.imageFile) {
        formData.append('file', this.imageFile); // Correct key for file
      } else if (value) {
        formData.append(key, value);
      }
    });
    debugger
    if(this.imageFile)
      {
    
        this.service.DoAddImage(formData).subscribe({
          next:(res:any)=>
          {
            
            const imagePath=res.data
          
            console.log(res.data)
    
            this.profile={
              Id:this.id,
              FirstName:this.profileForm.get('FirstName')?.value||"",
              LastName:this.profileForm.get('LastName')?.value||"",
               Email:"",
              Mobile:(this.profileForm.get('Mobile')?.value||"").toString(),
              DOB:this.profileForm.get('DOB')?.value||"",
               userType_Id:0,
               Username:"",
              Address:this.profileForm.get('Address')?.value||"",
              Zipcode:(this.profileForm.get('Zipcode')?.value||"").toString(),
              Country_Id:this.profileForm.get('Country')?.value||0,
              State_Id:this.profileForm.get('State')?.value||0,
    
              Profile:imagePath, 

            }
           
            this.saveUser(this.profile)
    
           // this.saveUser(this.User)
          }
        })
      }
      else{
        this.profile={
          Id:this.id,
          FirstName:this.profileForm.get('FirstName')?.value||"",
          LastName:this.profileForm.get('LastName')?.value||"",
          Email:"",
          Mobile:(this.profileForm.get('Mobile')?.value||"").toString(),
          DOB:this.profileForm.get('DOB')?.value||"",
          userType_Id:0,
          Username:"",
          Address:this.profileForm.get('Address')?.value||"",
          Zipcode:(this.profileForm.get('Zipcode')?.value||"").toString(),
          Country_Id:this.profileForm.get('Country')?.value||0,
          State_Id:this.profileForm.get('State')?.value||0,
          Profile:this.oldProfile,
          
        }
        this.saveUser(this.profile)
      }




     
    } else {
      console.log('Form is invalid');
    }
  }






  profileForm = new FormGroup({
   // UserType_Id: new FormControl(0, Validators.required),
   // Email: new FormControl('', [Validators.required, Validators.email]),
    id: new FormControl(0, Validators.required),
    FirstName: new FormControl('', [Validators.required, Validators.minLength(2)]),
    LastName: new FormControl('', [Validators.required, Validators.minLength(2)]),
  //  Username: new FormControl('', [Validators.required, Validators.minLength(4)]),
    profileImage:new FormControl(''),
    // Password: new FormControl('', [Validators.required, Validators.minLength(6)]),
    Mobile: new FormControl('', [Validators.required, Validators.pattern(/^(\+?\d{1,4}|\d{1,4})?\s?\(?\d{1,4}\)?[\d\s\-]{5,12}$/)]),
    DOB: new FormControl<Date>(new Date, Validators.required),
    Address: new FormControl('', Validators.required),
    Zipcode: new FormControl('', [Validators.required]),
    Country: new FormControl(0, Validators.required),
    State: new FormControl(0, Validators.required)
  });


getUserValue()
{
  const token:any=localStorage.getItem('token')
  console.log("ghsfgghw"+token)
 // const token:string="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJzYXZldG9rZW5zIiwiaWQiOiIxIiwiZW1haWwiOiJhc2h1dG9zaGd1cHRhNjE2ODZAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMSIsImV4cCI6MTczMzIxNDA4NywiaXNzIjoiaHR0cHM6Ly93d3cueW91dHViZS5jb20vIiwiYXVkIjoiaHR0cHM6Ly93d3cueW91dHViZS5jb20vIn0.YnAcN2NYg2YyD2qMCwmzz2RQY_dc_gZzvBLafo2Yfwg"
  if(token)
  {
  
    const decodeToken:any=jwtDecode(token);
    const Id=decodeToken.id
    const userDetail=new userDetailsModel()

    this.service.DoGetUserById(Id).subscribe({
      next:(res:any)=>{
        if(res.isSuccess)
        {
          this.LoginUser=res.data
          this.oldProfile=res.data.profileImage
          this.id=res.data.id
          console.log(res)
          this.LoginUser=res.data
          //this.profileForm.get('UserType_Id')?.setValue(res.data.userType_Id)
          // this.profileForm.get('Email')?.setValue(res.data.email)
          
           this.profileForm.get('id')?.setValue(res.data.id)
          this.profileForm.get('FirstName')?.setValue(res.data.firstName)
          this.profileForm.get('LastName')?.setValue(res.data.lastName)
        //  this.profileForm.get('Username')?.setValue(res.data.username)
         // this.profileForm.get('Password')?.setValue(res.data.password)
          this.profileForm.get('Mobile')?.setValue(res.data.mobile)
          this.profileForm.get('DOB')?.setValue(res.data.dob)
          this.profileForm.get('Address')?.setValue(res.data.address)
          
          this.profileForm.get('Zipcode')?.setValue(res.data.zipcode)
          
          this.profileForm.get('Country')?.setValue(res.data.country_Id)
          
          this.profileForm.get('State')?.setValue(res.data.state_Id)

          this.loadState(res.data.country_Id)
        }

      }
    });
}



}

saveUser(Payload:any)
{
  debugger
  console.log(Payload)
  this.service.DoUpdateUser(Payload).subscribe((res:any)=>{
 
    if(res.isSuccess==true)
    {
      this.toastr.success(res.message)
      // alert(`${res.message}`)
      console.log("success"+Payload)
      this.router.navigateByUrl("dashboard")
    }
    else{
      this.toastr.error(res.message)
      // alert(`${res.message}`)
      console.log("fail"+Payload)
    }
     })
}




allCountry : any [] = []


getAllCountry(){
  this.service.DoGetAllCountries().subscribe({
    next : (res: any) => {
      this.allCountry = res.data
       console.log(this.allCountry)
    },
    error : (error: any) =>{
     
    }
    
  })
}


allState : any [] = []

allStateByCountryId: any[] = []


loadState(countryId: number){
  this.service.DoGetAllStatesByCountry(countryId).subscribe((data: any)=>{
        this.allState = data.data;
         console.log(data)
        // Set the selected state in the form
        // if (this.editData) {
        //   this.patientForm.patchValue({
        //     state: this.editData.state
        //   });
        // }
      });
}

onChange(countrId : any){

  this.loadState(countrId)

}





}
