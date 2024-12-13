import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserServiceService {


  private CartQuantuitySubject=new BehaviorSubject<number>(0);
  cartCountValue=this.CartQuantuitySubject.asObservable();

  updateCartQuentity(userId:number)
  {
    this.DoGetCartQuentity(userId).subscribe((res:any)=>
    {
      this.CartQuantuitySubject.next(res.data)
    })

  }


  


  constructor() { }

public http=inject(HttpClient);

public DoRegester(obj:any)
{
  return this.http.post("https://localhost:7053/api/User/Regester",obj)
}

public DoGetOtp(obj:any)
{
  return this.http.post("https://localhost:7053/api/User/sendOtp",obj)
}

public DoVerifyOtpAndLogin(obj:any)
{
  
return this.http.post("https://localhost:7053/api/User/varifyOtpAndLogin",obj)
}

public DoForgetPassword(obj:any)
{
  return this.http.post(`https://localhost:7053/api/User/forgetPassword?email=${obj}`,obj)
}

public DoUpdateUser(obj:any)
{
  return this.http.post("https://localhost:7053/api/User/updateUser",obj)
}

public DoAddProduct(obj:any)
{
  return this.http.post("https://localhost:7053/api/User/addProduct",obj)
}

public DoGetAllProducts(obj:any)
{
  return this.http.get(`https://localhost:7053/api/User/getAllProducts/${obj}`)
}

public DoAddImage(obj:any)
{
  return this.http.post("https://localhost:7053/api/User/uploadFile",obj)
}
public DoGetUserById(id:number)
{
  return this.http.get(`https://localhost:7053/api/User/getUserById/${id}`)
}
public DoAddToCart(obj:any)
{
  return this.http.post("https://localhost:7053/api/User/addToCart",obj)
}
public DoGetAllCartProducts(id:number)
{
  return this.http.get(`https://localhost:7053/api/User/getProductsFromCart?Id=${id}`)
}

public DoRemoveFromCart(obj:any)
{
  return this.http.post(`https://localhost:7053/api/User/removeFromCart`,obj)
}
public DoDeleteProduct(obj:any)
{
  return this.http.delete(`https://localhost:7053/api/User/doDeleteAllProduct/${obj}`,obj)
}

public DoGetAllCountries()
{
  return this.http.get(`https://localhost:7053/api/User/getAllCountries`)
}

public DoGetAllStatesByCountry(id:number)
{
  return this.http.get(`https://localhost:7053/api/User/getAllStates/${id}`)
}
public DoUpdateProduct(obj:any)
{
  return this.http.post(`https://localhost:7053/api/User/doEditProduct`,obj)
}
public DoDecrementQuantityFromCart(obj:any)
{
  return this.http.post("https://localhost:7053/api/User/decrementTheQuantity",obj)
}
public DoIncrementQuantityFromCart(obj:any)
{
  return this.http.post("https://localhost:7053/api/User/incrementTheQuantity",obj)
}
public DoValidateCardDetails(obj:any)
{
  return this.http.post("https://localhost:7053/api/User/validateCardDetails",obj)
}
public DoAddSalesDetails(obj:any)
{
  return this.http.post("https://localhost:7053/api/User/addDataToSalesMaster",obj)
}
public DoGetinvoiceDetails(obj:any)
{
  return this.http.post(`https://localhost:7053/api/User/getAllInvoiceData?invoiceId=${obj}`,obj)
}
public DoGetCartQuentity(userId:number)
{
  return this.http.get(`https://localhost:7053/api/User/getCartItemQuantity/${userId}`)
}
public DoChnagePassword(obj:any)
{
  return this.http.post("https://localhost:7053/api/User/chnageUserPassword",obj)
}
}
