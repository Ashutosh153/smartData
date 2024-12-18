import { Component, inject, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { UserServicesService } from '../../services/user-services.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-view-complete-appointments',
  standalone: true,
  imports: [RouterLink,CommonModule],
  templateUrl: './view-complete-appointments.component.html',
  styleUrl: './view-complete-appointments.component.css'
})
export class ViewCompleteAppointmentsComponent implements OnInit {
  ngOnInit(): void {
    this.getAllSoapnotes()
  }

  allSoapNotesDetails:any[]=[]
  service=inject(UserServicesService)
  isProvider:boolean=true
  isPatient:boolean=true

 
  getAllSoapnotes()
  {
  const  token:any=localStorage.getItem('token')
if(token)
{const decodeToken:any=jwtDecode(token);
    const Id=decodeToken.id
    const userType=decodeToken.userType
    if(userType==1)
    {
      this.isProvider=false
      this.isPatient=true
    }
    if(userType==2)
    {
      this.isProvider=true
      this.isPatient=false
    }

    this.service.DoGetAllSoapNotesDetails(Id).subscribe({
      next:(res:any)=>{
        if(res.isSuccess)
        {
          this.allSoapNotesDetails=res.data
        }
      },
      error:(e:any)=>{
        console.log(e);
        
      }
    })



}
  }
   
}
