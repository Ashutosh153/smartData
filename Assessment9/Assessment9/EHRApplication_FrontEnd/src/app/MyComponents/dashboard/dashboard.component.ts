import { Component, inject, OnInit } from '@angular/core';
import { jwtDecode } from 'jwt-decode';
import { UserServicesService } from '../../services/user-services.service';
import { CommonModule, DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink, ReactiveFormsModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit{

  userId:any=0
  PatientObject:any
  UserObject:any
  bookAppointment:any
  isPatient=false
  isProvider=false
  isNavbarOpen = false;
  practionerform:any
  allPractioners?:any[]=[]
  allSpecilisation:any
  LoginUser:any
  service=inject(UserServicesService)

  ngOnInit(): void {
    const token="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJzYXZldG9rZW5zIiwiaWQiOiIzIiwiZW1haWwiOiJhc2h1dG9zaGd1cHRhNjE2ODZAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMiIsImV4cCI6MTczNDExNjI5NiwiaXNzIjoiaHR0cHM6Ly93d3cueW91dHViZS5jb20vIiwiYXVkIjoiaHR0cHM6Ly93d3cueW91dHViZS5jb20vIn0.NGD2eGG_58s40CyQCCVYf2V0RQHpbixRxSlI-4aMBJM"
    if(token)
    {
      const decodeToken: any = jwtDecode(token);
      this.userId = decodeToken.id;
      
      this.service.DoGetUserById(this.userId).subscribe({
        next:(res:any)=>{
          
          if(res.isSuccess)
          {
            this.LoginUser=res.data[0]
            this.LoginUser.dateOfBirth = new DatePipe('en-US').transform(
              this.LoginUser.dateOfBirth,
              'yyyy-MM-dd'
            );
            if(res.data[0].userType_ID==1)
            {
              
              this.UserObject={
              firstName:res.data[0].firstName,
                lastName:res.data[0].lastName,
                dateOfBirth:this.LoginUser.dateOfBirth,
                userName: res.data[0].userName,
                mobile:res.data[0].mobile,
                gender: res.data[0].gender,
                email: res.data[0].email,
                bloodGroup: res.data[0].bloodGroup,
                address: res.data[0].address,
                cityId: res.data[0].cityId,
                stateId: res.data[0].stateId,
                pinCode: res.data[0].pinCode,
                profile: res.data[0].profile,
                userType_ID: res.data[0].userType_ID
            }

            }

            if(res.data[0].userType_ID==2)
            {
              //debugger
              //console.log(res);
             
              this.isProvider=true
              this.UserObject={
                firstName:res.data[0].firstName,
                lastName:res.data[0].lastName,
                dateOfBirth: this.LoginUser.dateOfBirth,
                userName: res.data[0].userName,
                gender: res.data[0].gender,
                mobile:res.data[0].mobile,
                email: res.data[0].email,
                bloodGroup: res.data[0].bloodGroup,
                address: res.data[0].address,
                cityId: res.data[0].cityId,
                stateId: res.data[0].stateId,
                pinCode: res.data[0].pinCode,
                profile: res.data[0].profile,
                userType_ID: res.data[0].userType_ID,
                qualification: res.data[0].qualification,
                specialisation_ID: res.data[0].specialisation_ID,
                registration_Number: res.data[0].registration_Number,
                visiting_Charge: res.data[0].visiting_Charge,
              }
            
            }
          }
          
          console.log(this.UserObject);
          
          
        }
      })

    }

    this.bookAppointment=new FormGroup(
      {
        firstName:new FormControl('',[Validators.required]),
        lastName:new FormControl('',[Validators.required]), 
        qualification:new FormControl('',[Validators.required]),
        registration_Number:new FormControl('',[Validators.required]),
        specialisationType:new FormControl('',[Validators.required]),
        visiting_Charge:new FormControl(0,[Validators.required]),
        appointmentDate:new FormControl('',[Validators.required]),
        appointmentTime:new FormControl('',[Validators.required])

      }
    )





    this.getPractioners(0)
    debugger
    console.log(this.allPractioners);
    this.getAllSpecilization()
    
  }

getAllSpecilization()
{
  this.service.DoGetAllSpecilization().subscribe({
    next:(res:any)=>{
      this.allSpecilisation=res.data
    }
  })
}
  
  openProfileModal() {
    const modal = document.getElementById('myProfileModal');
    if (modal != null) {
      modal.style.display = 'block';
     
    }
  }

  closeProfilrModal() {
    const modal = document.getElementById('myProfileModal');
    if (modal != null) {
      modal.style.display = 'none';
    }
  }
  

  toggleNavbar() {
    this.isNavbarOpen = !this.isNavbarOpen;
  }

  getPractioners(id:number)
  {
    this.service.getPractionersApPerRequired(id).subscribe({
      next:(res:any)=>{
        if(res.isSuccess)
        {
          this.allPractioners=res.data
          console.log(this.allPractioners);
          
        }
      },
      error:(e:any)=>{
        console.log("error",e);
        
      }
     
    })
  }
  onChange() {
    const selectElement = document.getElementById('specialist_id') as HTMLSelectElement; 
    const id = selectElement.value; 
    this.getPractioners(parseInt(id))
  }

}
