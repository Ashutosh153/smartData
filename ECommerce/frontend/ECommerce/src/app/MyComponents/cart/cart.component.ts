import { Component, inject } from '@angular/core';
import { UserServiceService } from '../../services/User/user-service.service';
import { DetailsOfUsersService } from '../../services/DetailsOfUserService/details-of-users.service';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { jwtDecode } from 'jwt-decode';
import { Router, RouterLink } from '@angular/router';
import {
  CartItem,
  CartserviceService,
} from '../../services/cartservice.service';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, RouterLink, ReactiveFormsModule, CurrencyPipe],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css',
})
export class CartComponent {
  loggedUserValue: any;
  cartItem?: any;
  userId: number = 0;
  service = inject(UserServiceService);
  userDetailsService = inject(DetailsOfUsersService);
  cartService = inject(CartserviceService);
  grandTotal: number = 0;
  cardForm: FormGroup;
  userForm: FormGroup;
  AddressValidate: boolean = false;
  salesDetailsObj: any;
  cardDetailsObj: any;
  router = inject(Router);
  isCartEmpty: boolean = true;
  toastr = inject(ToastrService);

  doPayment: boolean = false;
  constructor(private fb: FormBuilder) {
    this.cardForm = new FormGroup({
      cardNumber: new FormControl(null, [
        Validators.required,
        Validators.pattern(/^\d{16}$/),
      ]),
      expiryDate: new FormControl(null, [
        Validators.required,
        //Validators.pattern(/^\d{2}\/\d{2}$/),
         Validators.pattern(/^(0[1-9]|1[0-2])\/(2[0-9])$/),
      ]), // MM/YY format
      cvv: new FormControl(null, [
        Validators.required,
        Validators.pattern(/^\d{3}$/),
      ]), // 3-digit CVV
    });

    this.userForm = this.fb.group({
      deliveryAddress: ['', Validators.required],
      deliveryZipcode: [
        0,
        [Validators.required, Validators.pattern('^[0-9]{6}$')],
      ],
      country_Id: ['', Validators.required],
      state_Id: ['', Validators.required],
    });

    const token: any = localStorage.getItem('token');
    //const token:string="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJzYXZldG9rZW5zIiwiaWQiOiIxIiwiZW1haWwiOiJhc2h1dG9zaGd1cHRhNjE2ODZAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMSIsImV4cCI6MTczMzIxNDA4NywiaXNzIjoiaHR0cHM6Ly93d3cueW91dHViZS5jb20vIiwiYXVkIjoiaHR0cHM6Ly93d3cueW91dHViZS5jb20vIn0.YnAcN2NYg2YyD2qMCwmzz2RQY_dc_gZzvBLafo2Yfwg"
    if (token) {
      const decodeToken: any = jwtDecode(token);
      const Id = decodeToken.id;
      this.userId = JSON.parse(Id);

      // this.userDetailsService.loggedUser.subscribe({
      //   next:(res:any)=>{
      //     this.loggedUserValue=res
      //     console.log(res)
      //   }
      // })

      this.LoadContent();
    }

    // if (this.cartItem == null) {
    //   this.isCartEmpty = true
    // }
    // else {
    //   console.log(this.cartItem)
    //   this.isCartEmpty = false
    // }
  }
  OnRemoveFromCart(pid: number) {
    var cartObhj = {
      productId: pid,
      userId: this.userId,
    };

    this.service.DoRemoveFromCart(cartObhj).subscribe({
      next: (res: any) => {
        if (res.isSuccess) {
          console.log(res.message);
          this.LoadContent();
        }
      },
      error: (err: any) => {
        console.log(err);
      },
    });
  }

  LoadContent() {
    this.service.DoGetAllCartProducts(this.userId).subscribe({
      next: (res: any) => {
        this.cartItem = res.data;

        if (res.data.length > 0) {
          this.isCartEmpty = false;
        } else {
          this.isCartEmpty = true;
        }
        console.log(res);

        var val = 0;
        this.cartItem.forEach((item: any) => {
          val += item.price * item.quantity;
        });
        this.grandTotal = val;
      },
    });
  }

  makePayment() {
    this.getAllCountry();

    this.service.DoGetUserById(this.userId).subscribe((res: any) => {
      this.loadState(res.data.country_Id);
      console.log(res);
      this.userForm.get('country_Id')?.setValue(res.data.country_Id);
      this.userForm.get('state_Id')?.setValue(res.data.state_Id);
      this.userForm.get('deliveryAddress')?.setValue(res.data.address);
      this.userForm.get('deliveryZipcode')?.setValue(res.data.zipcode);
      this.loadState(res.data.country_Id);
    });

    this.AddressValidate = true;
  }

  incrementQuantity(CId: number, PId: number) {
    const incObj = {
      cartId: CId,
      productId: PId,
    };
    this.service.DoIncrementQuantityFromCart(incObj).subscribe({
      next: (res: any) => {
        if (res.isSuccess) {
          this.LoadContent();
        } else {
          this.toastr.warning(res.message);
          // alert(res.message)
        }
      },
      error: (e: any) => {
        console.log('error: ' + e);
      },
    });

    console.log(CId, PId);
  }

  decrementQuantity(CId: number, PId: number) {
    const incObj = {
      cartId: CId,
      productId: PId,
    };
    this.service.DoDecrementQuantityFromCart(incObj).subscribe({
      next: (res: any) => {
        if (res.isSuccess) {
          this.LoadContent();
        } else {
          this.toastr.warning(res.message);
          // alert(res.message)
        }
      },
      error: (e: any) => {
        console.log('error: ' + e);
      },
    });

    console.log(CId, PId);
  }

  openAddressModal() {
    const modal = document.getElementById('myAddressModel');
    if (modal != null) {
      modal.style.display = 'block';
      this.makePayment();
    }
  }
  closeAddressModal() {
    const modal = document.getElementById('myAddressModel');
    if (modal != null) {
      modal.style.display = 'none';
    }
  }

  openPaymentModal() {
    const modal = document.getElementById('addPaymentModel');
    if (modal != null) {
      modal.style.display = 'block';
      // this.makePayment()
    }
  }
  closePaymentModal() {
    const modal = document.getElementById('addPaymentModel');
    if (modal != null) {
      modal.style.display = 'none';
    }
    // this.LoadContent();
  }

  limitInputLength(event: Event, maxLength: number): void {
    const input = event.target as HTMLInputElement;
    if (input.value.length > maxLength) {
      input.value = input.value.slice(0, maxLength);
    }
  }

  onSubmitAddress() {
    if (this.userForm?.valid) {
      this.closeAddressModal();

      this.salesDetailsObj = {
        userId: this.userId,
        subtotal: this.grandTotal,
        deliveryZipcode: parseInt(this.userForm.get('deliveryZipcode')?.value),
        deliveryAddress: this.userForm.get('deliveryAddress')?.value,
        deliveryState: parseInt(this.userForm.get('state_Id')?.value),
        deliveryCountry: parseInt(this.userForm.get('country_Id')?.value),
      };

      this.openPaymentModal();
      console.log(this.userForm.get('deliveryAddress')?.value);
      console.log(this.salesDetailsObj);
      this.AddressValidate = false;
      this.doPayment = true;
    } else {
      this.closeAddressModal();
      console.log('Form is invalid');
      console.log(this.userForm?.value);
    }
  }

  AddSalesDetails() {
    console.log(this.salesDetailsObj);
    this.service.DoAddSalesDetails(this.salesDetailsObj).subscribe({
      next: (res: any) => {
        if (res.isSuccess) {
          console.log(res.data);
          localStorage.setItem('invoiceId', res.data);
          console.log(res.message);
          this.closePaymentModal();
          this.router.navigateByUrl('/invoice');
        }
      },
    });
  }
  onSubmitValidateCardDetails() {
    if (this.cardForm?.valid) {
      console.log(this.cardForm?.value);
      this.cardDetailsObj = {
        cardNumber: this.cardForm.get('cardNumber')?.value.toString(),
        expiryDate: this.cardForm.get('expiryDate')?.value,
        cvv: this.cardForm.get('cvv')?.value.toString(),
      };

      this.service.DoValidateCardDetails(this.cardDetailsObj).subscribe({
        next: (res: any) => {
          if (res.isSuccess) {
            this.AddSalesDetails();
            this.toastr.success(res.message);
            // alert(res.message)
          } else {
            this.toastr.error(res.message);

            // alert(res.message)
          }
        },
        error: (e: any) => {
          this.toastr.error(e);
        },
      });
      console.log(this.cardDetailsObj);
      this.AddressValidate = false;
      //this.doPayment=false
    } else {
      console.log('Form is invalid');
      console.log(this.userForm?.value);
    }
  }

  allCountry: any[] = [];
  allState: any[] = [];

  getAllCountry() {
    this.service.DoGetAllCountries().subscribe({
      next: (res: any) => {
        this.allCountry = res.data;
        console.log(this.allCountry);
      },
      error: (error: any) => {},
    });
  }

  loadState(countryId: number) {
    this.service.DoGetAllStatesByCountry(countryId).subscribe((data: any) => {
      this.allState = data.data;
      console.log(data);
    });
  }
  onChange(countrId: any) {
    this.loadState(countrId);
  }
}
