import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { UserServicesService } from '../../services/user-services.service';
import { ToastrService } from 'ngx-toastr';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { jwtDecode } from 'jwt-decode';
import { CommonModule, DatePipe } from '@angular/common';

@Component({
  selector: 'app-update-profile',
  standalone: true,
  imports:  [CommonModule,ReactiveFormsModule,RouterLink],
  templateUrl: './update-profile.component.html',
  styleUrl: './update-profile.component.css'
})
export class UpdateProfileComponent {

  router=inject(Router)
profile:any

oldProfile?:string
id?:number
service=inject(UserServicesService)

  


LoginUser?:any;
toastr=inject(ToastrService)
TodaysDate:Date=new Date
maxDate = this.TodaysDate.toISOString().substring(0, 10);


constructor()
{
  this.getUserValue()
  
  this.getAllStates()
  
    
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
              id:this.id,
              firstName:this.profileForm.get('FirstName')?.value||"",
              lastName:this.profileForm.get('LastName')?.value||"",
               //Email:"",
               mobile:(this.profileForm.get('Mobile')?.value||"").toString(),
               dateOfBirth:this.profileForm.get('DOB')?.value||"",
               //userType_Id:0,
               //Username:"",
               address:this.profileForm.get('Address')?.value||"",
               pinCode:(this.profileForm.get('Zipcode')?.value||"").toString(),
               cityId:this.profileForm.get('city')?.value||0,
               stateId:this.profileForm.get('State')?.value||0,
               bloodGroup:this.profileForm.get('bloodGroup')?.value||'',
               gender:this.profileForm.get('gender')?.value||'',

    
               profile:imagePath, 

            }
           
            this.saveUser(this.profile)
    
           // this.saveUser(this.User)
          }
        })
      }
      else{
        this.profile={
          id:this.id,
          firstName:this.profileForm.get('FirstName')?.value||"",
          lastName:this.profileForm.get('LastName')?.value||"",
           //Email:"",
           mobile:(this.profileForm.get('Mobile')?.value||"").toString(),
           dateOfBirth:this.profileForm.get('DOB')?.value||"",
           //userType_Id:0,
           //Username:"",
           address:this.profileForm.get('Address')?.value||"",
           pinCode:(this.profileForm.get('Zipcode')?.value||"").toString(),
           cityId:this.profileForm.get('city')?.value||0,
           stateId:this.profileForm.get('State')?.value||0,
           bloodGroup:this.profileForm.get('bloodGroup')?.value||'',
           gender:this.profileForm.get('gender')?.value||'',

           profile:this.oldProfile,
          
        }
        this.saveUser(this.profile)
      }




     
    } else {
      console.log('Form is invalid');
    }
  }






  private _profileForm = new FormGroup({
    // UserType_Id: new FormControl(0, Validators.required),
    // Email: new FormControl('', [Validators.required, Validators.email]),
    id: new FormControl(0, Validators.required),
    FirstName: new FormControl('', [Validators.required, Validators.minLength(2)]),
    LastName: new FormControl('', [Validators.required, Validators.minLength(2)]),
    //  Username: new FormControl('', [Validators.required, Validators.minLength(4)]),
    profileImage: new FormControl(''),
    // Password: new FormControl('', [Validators.required, Validators.minLength(6)]),
    Mobile: new FormControl('', [Validators.required, Validators.pattern(/^(\+?\d{1,4}|\d{1,4})?\s?\(?\d{1,4}\)?[\d\s\-]{5,12}$/)]),
    DOB: new FormControl<Date>(new Date, Validators.required),
    Address: new FormControl('', Validators.required),
    Zipcode: new FormControl('', [Validators.required]),
    city: new FormControl(0, Validators.required),
    State: new FormControl(0, Validators.required),
    gender:new FormControl('',Validators.required),
    bloodGroup:new FormControl('',Validators.required)

  });
  public get profileForm() {
    return this._profileForm;
  }
  public set profileForm(value) {
    this._profileForm = value;
  }


getUserValue()
{
  //const token:any=localStorage.getItem('token')
  const token="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJzYXZldG9rZW5zIiwiaWQiOiIzIiwiZW1haWwiOiJhc2h1dG9zaGd1cHRhNjE2ODZAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMiIsImV4cCI6MTczNDExNjI5NiwiaXNzIjoiaHR0cHM6Ly93d3cueW91dHViZS5jb20vIiwiYXVkIjoiaHR0cHM6Ly93d3cueW91dHViZS5jb20vIn0.NGD2eGG_58s40CyQCCVYf2V0RQHpbixRxSlI-4aMBJM"
  console.log("ghsfgghw"+token)
  if(token)
  {
  
    const decodeToken:any=jwtDecode(token);
    const Id=decodeToken.id
    //const userDetail=new userDetailsModel()
debugger
    this.service.DoGetUserById(Id).subscribe({
      
      next:(res:any)=>{
        console.log(res)
        if(res.isSuccess)
        {
          this.LoginUser=res.data[0]
          this.oldProfile=res.data[0].profile
          this.id=res.data[0].id
          console.log(res)
          this.LoginUser=res.data[0]
          this.LoginUser.dateOfBirth = new DatePipe('en-US').transform(
            this.LoginUser.dateOfBirth,
            'yyyy-MM-dd'
          );
           
           this.profileForm.get('id')?.setValue(res.data.id)
          this.profileForm.get('FirstName')?.setValue(res.data[0].firstName)
          this.profileForm.get('LastName')?.setValue(res.data[0].lastName)
          this.profileForm.get('Mobile')?.setValue(res.data[0].mobile)
          this.profileForm.get('DOB')?.setValue(this.LoginUser.dateOfBirth)
          this.profileForm.get('Address')?.setValue(res.data[0].address)
          
          this.profileForm.get('Zipcode')?.setValue(res.data[0].pinCode)
          
          this.profileForm.get('city')?.setValue(res.data[0].cityId)
          
          this.profileForm.get('State')?.setValue(res.data[0].stateId)
          this.profileForm.get('gender')?.setValue(res.data[0].gender)
          this.profileForm.get('bloodGroup')?.setValue(res.data[0].bloodGroup)

          this.loadcities(res.data[0].stateId)
        }

      }
    });
}



}

saveUser(Payload:any)
{
  debugger
  console.log(Payload)
  this.service.DoUpdatePatient(Payload).subscribe((res:any)=>{
 
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




allStates : any [] = []


getAllStates(){
  this.service.DoGetAllStates().subscribe({
    next : (res: any) => {
      this.allStates = res.data
       console.log(this.allStates)
    },
    error : (error: any) =>{
     
    }
    
  })
}


allCities : any [] = []

// allStateByCountryId: any[] = []


loadcities(countryId: number){
  this.service.DoGetCitiesById(countryId).subscribe((data: any)=>{
        this.allCities = data.data;
         console.log(data)
       
      });
}

onChange(countrId : any){

  this.loadcities(countrId)

}





}

