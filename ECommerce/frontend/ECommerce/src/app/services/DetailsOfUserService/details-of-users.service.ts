import { inject, Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';
import { BehaviorSubject, Subject } from 'rxjs';
import { UserServiceService } from '../User/user-service.service';



export class userDetailsModel{
  UserType_Id:number=0;
  Email:string="";
  id:number=0;
  FirstName:string="";
  LastName:string="";
  Username:string="";
  Password:string="";
  Mobile:string="";
  DOB:Date=new Date;
  Address:string="";
  Zipcode:string="";
  Country:number=0;
  State:number=0;
  profile:string="";
}

@Injectable({
  providedIn: 'root'
})

export class DetailsOfUsersService {

  service=inject(UserServiceService)


  loggedUser:BehaviorSubject<userDetailsModel>=new BehaviorSubject<userDetailsModel>(this.getLoggedUser());

private getLoggedUser():userDetailsModel
{
  const token:any=localStorage.getItem('token')
  //  const token:any="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJzYXZldG9rZW5zIiwiaWQiOiIxIiwiZW1haWwiOiJhc2h1dG9zaGd1cHRhNjE2ODZAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMSIsImV4cCI6MTczMzIxNDA4NywiaXNzIjoiaHR0cHM6Ly93d3cueW91dHViZS5jb20vIiwiYXVkIjoiaHR0cHM6Ly93d3cueW91dHViZS5jb20vIn0.YnAcN2NYg2YyD2qMCwmzz2RQY_dc_gZzvBLafo2Yfwg"
  if(token)
  {
  
    const decodeToken:any=jwtDecode(token);
    const Id=decodeToken.id
    const userDetail=new userDetailsModel()

    this.service.DoGetUserById(Id).subscribe({
      next:(res:any)=>{
        if(res.isSuccess)
        {
          userDetail.UserType_Id=res.data.userType_Id;
        
          userDetail.Email=res.data.email ;
          userDetail.id=res.data.id;
          userDetail. FirstName=res.data.firstName;
          userDetail. LastName=res.data.lastName;
          userDetail. Username=res.data.username;
          userDetail. Password=res.data.password;
          userDetail.Mobile=res.data.mobile;
          userDetail.DOB=res.data.dob;
          userDetail.Address=res.data.address;
          userDetail.Zipcode=res.data.zipcode;
          userDetail.Country=res.data.country_Id;
          userDetail.State=res.data.state_Id;
          userDetail.profile=res.data.profileImage;
        }
      }
    });


    

    
 
    return userDetail

  }
  return new userDetailsModel();
}


  public resetLoginUser():void{
    const newLoginUser=this.getLoggedUser();
    this.loggedUser.next(newLoginUser);
  }

 



}
