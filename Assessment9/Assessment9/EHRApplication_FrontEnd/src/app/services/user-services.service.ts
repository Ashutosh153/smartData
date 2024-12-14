import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UserServicesService {

  constructor() { }
  public http=inject(HttpClient)

  public DoRegistration(obj:any)
  {
      return this.http.post("https://localhost:7053/api/User/createUserController",obj)
  }

  public DoAddImage(obj:any)
  {
    return this.http.post("https://localhost:7053/api/SaveImage/saveImage",obj)
  }
  public DoGetAllStates()
  {
    return this.http.get("https://localhost:7053/api/User/getAllStates")
  }
public DoGetCitiesById(obj:any)
{
  return this.http.get(`https://localhost:7053/api/User/getAllCities/${obj}`)
}
public DoGetAllSpecilization()
{
  return this.http.get("https://localhost:7053/api/User/getAllSpecilizations")
}
public DoGetOtp(obj:any)
{
  return this.http.post("https://localhost:7053/api/User/DoVerifyCredentials",obj)
}
public DoVerifyOtpAndLogin(obj:any)
{
  return this.http.post("https://localhost:7053/api/User/varifyOtpAndLogin",obj)
}
public DoForgetPassword(obj:any)
{
  return this.http.post(`https://localhost:7053/api/User/forgetPassword?email=${obj}`,obj)
}
public DoGetUserById(obj:any)
{
  return this.http.get(`https://localhost:7053/api/User/getUserByID/${obj}`)
}
public DoUpdatePatient(obj:any)
{
  return this.http.post("https://localhost:7053/api/User/updatePatientController",obj)
}
public DoUpdateProvider(obj:any)
{
  return this.http.post("https://localhost:7053/api/User/updateProviderCommand",obj)
}
public getPractionersApPerRequired(obj:any)
{
  return this.http.get(`https://localhost:7053/api/User/getPractionersAsRequired/${obj}`)
}
}
