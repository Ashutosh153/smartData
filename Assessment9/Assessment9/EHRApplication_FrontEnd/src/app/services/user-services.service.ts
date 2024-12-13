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
}
