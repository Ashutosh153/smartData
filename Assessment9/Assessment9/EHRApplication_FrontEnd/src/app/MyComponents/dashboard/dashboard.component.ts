import { Component, inject, OnInit } from '@angular/core';
import { jwtDecode } from 'jwt-decode';
import { UserServicesService } from '../../services/user-services.service';
import { CommonModule, DatePipe } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Toast, ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';


@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink, ReactiveFormsModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})


export class DashboardComponent implements OnInit{
  timeMin?: string;
  timeMax?: string;
  todayDate?: string;
  earliestTime?: string;
  latestTime?: string;
  isLoading:boolean=false
  
  minTime: string = '';
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
  allPractioners:any[]=[]
  allAppointments:any[]=[]
  allPatients:any[]=[]
  allSpecilisation:any
  showPassword: boolean = false;
  LoginUser:any
  TodaysDate: Date = new Date();
  maxDate = this.TodaysDate.toISOString().substring(0, 10);
  service=inject(UserServicesService)
  toaster=inject(ToastrService)
    router=inject(Router)
  changePasswordForm: any
  errorMessage?: string;
  changePassObj?: any

  
    constructor(private fb: FormBuilder) {
      this.todayDate = new Date().toISOString().split('T')[0];

      // Set the earliest appointment time as current time
      const now = new Date();
      this.earliestTime = now.toTimeString().split(' ')[0];
  
      // Set the latest appointment time as one hour from now
      now.setHours(now.getHours() + 1);
      this.latestTime = now.toTimeString().split(' ')[0];


      this.changePasswordForm = this.fb.group(
        {
          password: [
            '',
            [
              Validators.required,
              Validators.minLength(8),
              Validators.pattern(
                /^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$/
              ),
            ],
          ],
          confirmPassword: ['', Validators.required],
        },
        { validator: this.passwordMatchValidator }
      );
    }
   async  passwordMatchValidator(group: FormGroup) {
      const password = group.get('password')?.value;
      const confirmPassword = group.get('confirmPassword')?.value;
      return password === confirmPassword ? null : { mismatch: true };
    }

    
async  onSubmitChangePassword() {
    if (this.changePasswordForm.invalid) {
      this.errorMessage = 'Please correct the errors before submitting.';
      return;
    }
    this.errorMessage = '';

    // Perform further actions, like sending the form data to the server
    this.changePassObj = {
      Id: this.userId,
      Password: this.changePasswordForm.get('confirmPassword')?.value,
    };
    this.service.DoChangePassword(this.changePassObj).subscribe({
      next: (res: any) => {
        if (res.isSuccess) {
          this.toaster.success(res.message);
          this.closeChangePassModel();
        } else {
          this.toaster.error(res.message);
        }
      },
    });
    console.log(this.changePassObj);
  }

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }

  ngOnInit(): void {
    
    const now = new Date();
    const nextHour = new Date(now.setHours(now.getHours() + 1));
    this.minTime = nextHour.toISOString().substring(11, 16);
   
   // const token="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJzYXZldG9rZW5zIiwiaWQiOiIzIiwiZW1haWwiOiJhc2h1dG9zaGd1cHRhNjE2ODZAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMiIsImV4cCI6MTczNDExNjI5NiwiaXNzIjoiaHR0cHM6Ly93d3cueW91dHViZS5jb20vIiwiYXVkIjoiaHR0cHM6Ly93d3cueW91dHViZS5jb20vIn0.NGD2eGG_58s40CyQCCVYf2V0RQHpbixRxSlI-4aMBJM"
   //const token="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJzYXZldG9rZW5zIiwiaWQiOiI2IiwiZW1haWwiOiJhc2h1dG9zaGd1cHRhQHlvcG1haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMSIsImV4cCI6MTczNDMyNzI4MiwiaXNzIjoiaHR0cHM6Ly93d3cueW91dHViZS5jb20vIiwiYXVkIjoiaHR0cHM6Ly93d3cueW91dHViZS5jb20vIn0.Py3QCkuA7H-5oJyIyw2RiGS-VuCQ-CIWSjUe0g_nKt8"
   const token=localStorage.getItem("token")
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
                userType_ID: res.data[0].userType_ID,
            }

            }

            if(res.data[0].userType_ID==2)
            {
              console.log(res);
             
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
                specialisation:res.data[0].specilisation

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


    this.getAllAppointments()




    this.getPractioners(0)
  
    console.log(this.allPractioners);
    this.getAllSpecilization()

    this.getAllPAtients()
    
  }

getAllPAtients()
{
  this.service.DoGetOtherTypeUser(this.userId).subscribe({
    next:(res:any)=>{
     console.log(res);
     
      this.allPatients=res.data
    }
  })
}

  getAllAppointments()
{
    this.service.DoGetAllAppointmentsById(this.userId).subscribe({
      next:(res:any)=>{
        if(res.isSuccess)
        { 
         
          console.log(res)
          this.allAppointments=res.data
       
        }
      },
      error:(e:any)=>
      {
        console.log(e)
      }
    })

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
    this.bookAppointmentForm.reset();
    this.closeaddAppointmentModelByDoctor()
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
        this.bookAppointmentForm.reset();
    const modal = document.getElementById('MakePaymentModel');
    if (modal != null) {
      modal.style.display = 'block';
     
    }
  }

  closeMakePaymentModel() {
       this.bookAppointmentForm.reset();
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
  
  openaddAppointmentModel() {
    const modal = document.getElementById('addAppointment');
    if (modal != null) {
      modal.style.display = 'block';
     
    }
  }

  closeaddAppointmentModel() {
    const modal = document.getElementById('addAppointment');
    if (modal != null) {
      modal.style.display = 'none';
    }
  }
  
  openaddAppointmentModelByDoctor() {
    const modal = document.getElementById('addAppointmentByDoctor');
    if (modal != null) {
      modal.style.display = 'block';
     
    }
  }

  closeaddAppointmentModelByDoctor() {
    const modal = document.getElementById('addAppointmentByDoctor');
    if (modal != null) {
      modal.style.display = 'none';
    }
  }
  openChangePassModel() {
    const modal = document.getElementById('MyChangePassModel');
    if (modal != null) {
      modal.style.display = 'block';
      //this.makePayment()
    }
  }

  closeChangePassModel() {
    this.changePasswordForm.reset()
    const modal = document.getElementById('MyChangePassModel');
    if (modal != null) {
      modal.style.display = 'none';
    }
  }
  toggleNavbar() {
    this.isNavbarOpen = !this.isNavbarOpen;
  }

 async getPractioners(id:number)
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
 async onChangeSpecilization() {
    const selectElement = document.getElementById('specialist_id') as HTMLSelectElement; 
    const id = selectElement.value; 
    this.getPractioners(parseInt(id))
  }
  onclickBookAppointment(data:any)
  {
  
   this.openBooAppointmentModal()
   this.closeaddAppointmentModel()
   this.practionerChargesForAppointment=data.visiting_Charge
    this.practionerIdForAppointment=data.id 
   
  }

  onSubmitCreateAppointment()
  {
    
    this.closeBooAppointmentModal()
    

    if(this.isProvider)
    {
      this.appointObj={
        providerId:this.userId,
        fee:this.UserObject.visiting_Charge,
  
        patientId:this.practionerIdForAppointment,
  
        appointmentDate:this.bookAppointmentForm.get("appointmentDate").value,
        appointmentTime:this.bookAppointmentForm.get("appointmentTime").value,
        chiefComplaint:this.bookAppointmentForm.get("chiefComplained").value
      }
    }
  
    if(!this.isProvider)
    {
      this.appointObj={
        providerId:this.practionerIdForAppointment,
        fee:this.practionerChargesForAppointment,
        patientId:this.userId,
       appointmentDate:this.bookAppointmentForm.get("appointmentDate").value,
        appointmentTime:this.bookAppointmentForm.get("appointmentTime").value,
        chiefComplaint:this.bookAppointmentForm.get("chiefComplained").value
      }

    }
      
  

    this.service.DoverifyAvailableAppointment(this.appointObj).subscribe({
      
    next:(res:any)=>{
        if(res.isSuccess)
        {
          if(!this.isProvider){
            this.closeBooAppointmentModal()

            this.service.DoCreateOrder(parseFloat(this.practionerChargesForAppointment)).subscribe({
              next:(res:any)=>{
                const options = {
                  key: 'rzp_test_j1n3HfglIVc3GS',  
                  amount: res.amount,
                  currency: res.currency,
                  order_id: res.id,
                  handler: (response: any) => {
                    this.appointObj={
                      providerId:this.practionerIdForAppointment,
                      fee:this.practionerChargesForAppointment,
                      patientId:this.userId,
                     appointmentDate:this.bookAppointmentForm.get("appointmentDate").value,
                      appointmentTime:this.bookAppointmentForm.get("appointmentTime").value,
                      chiefComplaint:this.bookAppointmentForm.get("chiefComplained").value,
                    
                      PaymentId:response.razorpay_payment_id,
                      OrderId:res.id
                    }
                    
                    this.isLoading=true
                    this.service.DoAddAppointmentAfterPayment(this.appointObj).subscribe({
                      next:(res:any)=>{
                        console.log(res.data)
                        if(res.isSuccess)
                          {
                            this.isLoading=false
                            this.getAllAppointments()
                            this.toaster.success(res.message)
                           // this.closeMakePaymentModel()
                          }
                          else{
                            this.isLoading=false
                            this.toaster.error(res.message)
                          }
        
                      },
                      error:(e:any)=>{
                        this.isLoading=false
                      }
                    })

                   
                  },


                };
                if (window.Razorpay) {
                  const razorpay = new Razorpay(options);
                  razorpay.open();
                } else {
                  console.error('Razorpay SDK is not loaded properly');
                }
                          


              }
            })
         
            
      



           // this.openMakePaymentModel()
           //this.bookAppointmentForm.reset();

          }
          else if(this.isProvider)
          {
            // method creating new appointment
            this.onclickMakePayment()
          }

         
        }
        else
        {
          this.toaster.error(res.message)
        }
    }
    })

    this.getAllAppointments()

  }

  onClickGoToAppointment(data:any)
  { 
      this.router.navigateByUrl(`/goToAppointment/${data.id}`)
  }

  onclickMakePayment()
  {
    this.isLoading=true
    this.service.DoCraeteAppointment(this.appointObj).subscribe({
      next:(res:any)=>{
        if(res.isSuccess)
        {
          this.isLoading=false
          this.getAllAppointments()
          this.toaster.success(res.message)
          this.closeMakePaymentModel()
        }
        else{
          this.isLoading=false
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

    this.service.DoUpdateAppointment(this.editAppointmentObj).subscribe({
      next:(res:any)=>{
        if(res.isSuccess)
        {
          this.toaster.success(res.message)
          this.closeEditAppointmentModel()
          this.getAllAppointments()
        }
        else{
          this.toaster.error(res.message)
        }
      }
    })
   

    console.log(this.editAppointmentObj);
    
  }
 
  onClickCancelAppointment(data:any)
  {
    
    Swal.fire({
      title: 'Are you sure?',
      text: `Do you want to cancel the appointment?`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes',
      cancelButtonText: 'No',
      reverseButtons: true,
    }).then((result: any) => {
      if (result.isConfirmed) {
        this.service.DoCancelAppointment(data.id).subscribe({
          next: (res: any) => {
            if (res.isSuccess) {
              console.log('success');
              Swal.fire(res.message, '', 'success');
              //alert(res.message)

              this.getAllAppointments();
            } else this.getAllAppointments();
          },
          error: (e: any) => {
            console.log(e);
          },
        });
      } else {
        // If user cancels, do nothing
        console.log('error occured while deleting product');
      }
    });
  }
  
  
  // isDisabledRow(appointment: any): boolean {

  //   const formattedAppointmentDate = new Date(appointment.appointmentDate).toISOString().split('T')[0];
  //   return appointment.appointmentStatus !== 'Scheduled' || (formattedAppointmentDate < this.maxDate);
  // }
  isDisabledRow(appointment: any): boolean {
    // Get the appointment date in local time without time components
    const appointmentDate = new Date(appointment.appointmentDate);
    const formattedAppointmentDate = new Date(
      appointmentDate.getFullYear(),
      appointmentDate.getMonth(),
      appointmentDate.getDate()
    );
  
    // Get today's date in local time without time components
    const today = new Date();
    const localToday = new Date(
      today.getFullYear(),
      today.getMonth(),
      today.getDate()
    );
  
    // Check if the appointment date is in the past or status is not 'Scheduled'
    return (
      appointment.appointmentStatus !== 'Scheduled' ||
      formattedAppointmentDate < localToday
    );
  }

  
 // Custom time validator to restrict times to one hour ahead
 timeValidator(control: any) {
  const selectedTime = new Date(`1970-01-01T${control.value}:00`);
  const minTime = new Date(`1970-01-01T${this.timeMin}:00`);
  const maxTime = new Date(`1970-01-01T${this.timeMax}:00`);
  if (selectedTime < minTime || selectedTime > maxTime) {
    return { timeInvalid: true };
  }
  return null;
}

onDateChange() {
  const selectedDate = new Date(this.bookAppointmentForm.get('appointmentDate')?.value);
  if (selectedDate.toISOString().split('T')[0] === this.todayDate) {
    this.timeMin = new Date().toTimeString().split(' ')[0]; // Reset to current time
    this.timeMax = new Date(new Date().setHours(new Date().getHours() + 1)).toTimeString().split(' ')[0]; // Set max to one hour from now
  } else {
    const now = new Date();
    now.setHours(now.getHours() + 1);
    this.timeMin = now.toTimeString().split(' ')[0];
    this.timeMax = new Date(new Date().setHours(new Date().getHours() + 1)).toTimeString().split(' ')[0];
  }
}
goToChat(data:any)
{
this.router.navigateByUrl(`/chat/${data}`)

}

}


