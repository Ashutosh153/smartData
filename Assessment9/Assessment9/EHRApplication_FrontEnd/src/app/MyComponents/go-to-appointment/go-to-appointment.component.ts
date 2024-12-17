import { Component, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserServicesService } from '../../services/user-services.service';
import { CommonModule, DatePipe } from '@angular/common';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-go-to-appointment',
  standalone: true,
  imports: [CommonModule,ReactiveFormsModule],
  templateUrl: './go-to-appointment.component.html',
  styleUrl: './go-to-appointment.component.css'
})
export class GoToAppointmentComponent {

  appointmentID?:any
  patientObject:any
  dateOfBirth:any
  AddSoapNoteObj:any
  service=inject(UserServicesService)
  toaster=inject(ToastrService)
  router=inject(Router)

  constructor(private route:ActivatedRoute)
  {
    this.appointmentID=Number(this.route.snapshot.paramMap.get('id')||'');
    this.service.DoGetPatientDetailsForAppointment(this.appointmentID).subscribe({
      next:(res:any)=>{
       
          if(res.isSuccess)
          {
             this.dateOfBirth= new DatePipe('en-US').transform(
              res.data[0].dob,
                        'yyyy-MM-dd'
                      );
           
            //   var timeDiff = Math.abs(Date.now() - res.data[0].dob);
            //   var age = Math.floor((timeDiff / (1000 * 3600 * 24))/365);
         var date=Date.now()
           
            this.patientObject={
              profile:res.data[0].profile,
              gender:res.data[0].gender,
              dob:this.dateOfBirth,
              patientName:res.data[0].patientName,
              appointmentDate:res.data[0].appointmentDate,
              appointmentTime:res.data[0].appointmentTime,
              chiefComplaint:res.data[0].chiefComplaint
            }
          }
      }
    })

   
  }
   SOAPForm = new FormGroup({  
    Assessment:new FormControl ('', Validators.required),
    Objective:new FormControl ('', [Validators.required ]),
    Subjective:new FormControl ('', [Validators.required]), 
    Plan:new FormControl ('', [Validators.required])
      });
 
      onClickCompleteAppointment()
      {
        console.log(this.SOAPForm.value);
          this.AddSoapNoteObj={
          appointmnetID:this.appointmentID,
          subjective:this.SOAPForm.get('Subjective')?.value,
          objective:this.SOAPForm.get('Objective')?.value,
          assessment:this.SOAPForm.get('Assessment')?.value,
          plan:this.SOAPForm.get('Plan')?.value
         }

         console.log(this.AddSoapNoteObj)

         this.service.DoAddSoapNoteAndCompleteAppointment(this.AddSoapNoteObj).subscribe({
          next:(res:any)=>{
            if(res.isSuccess)
            {
                this.toaster.success(res.message)
                this.router.navigateByUrl("/dashboard")
            }

            this.toaster.warning(res.message)

          }
         

         })
        
        
      }

  
}
