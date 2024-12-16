import { Component, inject, OnInit } from '@angular/core';
import { jwtDecode } from 'jwt-decode';
import { UserServicesService } from '../../services/user-services.service';
import { CommonModule, DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Toast, ToastrService } from 'ngx-toastr';

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
  bookAppointmentForm:any
  isPatient=false
  isProvider=false
  isNavbarOpen = false;
  practionerform:any
  appointObj:any
  editAppointmentObj:any
  editAppointmentId:any
  practionerIdForAppointment:any
  practionerChargesForAppointment:any
  allPractioners?:any[]=[]
  allAppointments:any[]=[]
  allSpecilisation:any
  LoginUser:any
  TodaysDate: Date = new Date();
  maxDate = this.TodaysDate.toISOString().substring(0, 10);
  service=inject(UserServicesService)
  toaster=inject(ToastrService)

  ngOnInit(): void {
   // const token="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJzYXZldG9rZW5zIiwiaWQiOiIzIiwiZW1haWwiOiJhc2h1dG9zaGd1cHRhNjE2ODZAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMiIsImV4cCI6MTczNDExNjI5NiwiaXNzIjoiaHR0cHM6Ly93d3cueW91dHViZS5jb20vIiwiYXVkIjoiaHR0cHM6Ly93d3cueW91dHViZS5jb20vIn0.NGD2eGG_58s40CyQCCVYf2V0RQHpbixRxSlI-4aMBJM"
   const token="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJzYXZldG9rZW5zIiwiaWQiOiI2IiwiZW1haWwiOiJhc2h1dG9zaGd1cHRhQHlvcG1haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMSIsImV4cCI6MTczNDMyNzI4MiwiaXNzIjoiaHR0cHM6Ly93d3cueW91dHViZS5jb20vIiwiYXVkIjoiaHR0cHM6Ly93d3cueW91dHViZS5jb20vIn0.Py3QCkuA7H-5oJyIyw2RiGS-VuCQ-CIWSjUe0g_nKt8"
   if(token)
    {
      const decodeToken: any = jwtDecode(token);
      this.userId = decodeToken.id;
      
      this.service.DoGetUserById(this.userId).subscribe({
        next:(res:any)=>{
          
          if(res.isSuccess)
          {
           
            this.LoginUser=res.data[0]
            console.log(this.LoginUser)
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
          this.bookAppointmentForm=new FormGroup(
            {
           
            
              appointmentDate:new FormControl('',[Validators.required]),
              appointmentTime:new FormControl('',[Validators.required]),
              chiefComplained:new FormControl('',[Validators.required])
      
            }
          )
          
          console.log(this.UserObject);
          
          
        }
      })

    }

    this.service.DoGetAllAppointmentsById(this.userId).subscribe({
      next:(res:any)=>{
        if(res.isSuccess)
        {
          this.allAppointments=res.data
       
        }
      },
      error:(e:any)=>
      {
        console.log(e)
      }
    })

   





    this.getPractioners(0)
  
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

  openBooAppointmentModal() {
    const modal = document.getElementById('BooAppointmentModel');
    if (modal != null) {
      modal.style.display = 'block';
     
    }
  }

  closeBooAppointmentModal() {
    const modal = document.getElementById('BooAppointmentModel');
    if (modal != null) {
      modal.style.display = 'none';
    }
  }

  openMakePaymentModel() {
    const modal = document.getElementById('MakePaymentModel');
    if (modal != null) {
      modal.style.display = 'block';
     
    }
  }

  closeMakePaymentModel() {
    const modal = document.getElementById('MakePaymentModel');
    if (modal != null) {
      modal.style.display = 'none';
    }
  }
  openEditAppointmentModel() {
    const modal = document.getElementById('EditAppointmentModel');
    if (modal != null) {
      modal.style.display = 'block';
     
    }
  }

  closeEditAppointmentModel() {
    const modal = document.getElementById('EditAppointmentModel');
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
  onChangeSpecilization() {
    const selectElement = document.getElementById('specialist_id') as HTMLSelectElement; 
    const id = selectElement.value; 
    this.getPractioners(parseInt(id))
  }
  onclickBookAppointment(data:any)
  {
  
   this.openBooAppointmentModal()
   this.practionerChargesForAppointment=data.visiting_Charge
    this.practionerIdForAppointment=data.id 
   
  }

  onSubmitCreateAppointment()
  {
   
    this.appointObj={
      providerId:this.practionerIdForAppointment,
      fee:this.practionerChargesForAppointment,

      patientId:this.userId,

      appointmentDate:this.bookAppointmentForm.get("appointmentDate").value,
      appointmentTime:this.bookAppointmentForm.get("appointmentTime").value,
      chiefComplaint:this.bookAppointmentForm.get("chiefComplained").value
    }

    this.service.DoverifyAvailableAppointment(this.appointObj).subscribe({
    next:(res:any)=>{
        if(res.isSuccess)
        {
          this.closeBooAppointmentModal()
          this.openMakePaymentModel()
        }
        else
        {
          this.toaster.error(res.message)
        }
    }
    })

 
  }

  onclickMakePayment()
  {
    this.service.DoCraeteAppointment(this.appointObj).subscribe({
      next:(res:any)=>{
        if(res.isSuccess)
        {
          this.toaster.success(res.message)
        }
        else{
          this.toaster.error(res.message)
        }
      }
    })
  }

  onClickEditAppointment(data:any)
  {
    this.editAppointmentId=data.id

     var apointmentDate = new DatePipe('en-US').transform(
      data.appointmentDate,
       'yyyy-MM-dd'
    );
    debugger
    console.log (apointmentDate)

    this.openEditAppointmentModel()
    this.bookAppointmentForm.get("chiefComplained").setValue(data.chiefComplaint)
    this.bookAppointmentForm.get("appointmentDate").setValue(apointmentDate)
    this.bookAppointmentForm.get("appointmentTime").setValue(data.appointmentTime)
  }

  onClickUpdateAppointment()
  {
    console.log(this.bookAppointmentForm.value)

    this.editAppointmentObj={
      id:this.editAppointmentId,
      appointmentDate:this.bookAppointmentForm.get("appointmentDate").value,
      appointmentTime: this.bookAppointmentForm.get("appointmentTime").value,
      chiefComplaint:this.bookAppointmentForm.get("chiefComplained").value
    }
   
debugger
    console.log(this.editAppointmentObj);
    
  }
 
  

}
