import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { UserServiceService } from '../../services/User/user-service.service';
import {
  DetailsOfUsersService,
  userDetailsModel,
} from '../../services/DetailsOfUserService/details-of-users.service';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import { RouterLink } from '@angular/router';
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
import { jwtDecode } from 'jwt-decode';
import { ToastrService } from 'ngx-toastr';
import { sellingPriceGreaterThanPurchasePrice } from '../add-product/add-product.component';
import Swal from 'sweetalert2';
import { timeout } from 'rxjs';
@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink, ReactiveFormsModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css',
})
export class DashboardComponent implements OnInit {
  user: boolean = false;
  admin: boolean = true;
  editProduct: boolean = false;
  // cartService=inject(CartserviceService)
  UserTypeId_new?: number;
  product?: any;
  oldProductImage?: string;
  userId: number = 0;
  cartCount?: number;
  changePassObj: any;
  toastr = inject(ToastrService);
  showPassword: boolean = false;

  token: any = localStorage.getItem('token');

  userObj: any;
  viewProductObject: any;
  changePasswordForm: FormGroup;
  errorMessage: string = '';

  role: string = 'user';
  isUser = true;
  service = inject(UserServiceService);
  userDetailsService = inject(DetailsOfUsersService);
  products: any[] = [];
  loggedUserValue?: userDetailsModel;
  productForm: any;
  productId?: number;
  noProductAdded: boolean = true;
  TodaysDate: Date = new Date();
  maxDate = this.TodaysDate.toISOString().substring(0, 10);

  constructor(private fb: FormBuilder) {
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

  passwordMatchValidator(group: FormGroup) {
    const password = group.get('password')?.value;
    const confirmPassword = group.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { mismatch: true };
  }

  onSubmitChangePassword() {
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
    this.service.DoChnagePassword(this.changePassObj).subscribe({
      next: (res: any) => {
        if (res.isSuccess) {
          this.toastr.success(res.message);
          this.closeChangePassModel();
        } else {
          this.toastr.error(res.message);
        }
      },
    });
    console.log(this.changePassObj);
  }

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }

  ngOnInit(): void {
    var token: any = localStorage.getItem('token');
    if (token) {
      const decodeToken: any = jwtDecode(token);
      this.userId = decodeToken.id;
      this.service.updateCartQuentity(decodeToken.id);

      this.service.DoGetUserById(this.userId).subscribe({
        next: (res: any) => {
          console.log(res.data);

          this.userObj = {
            firstName: res.data.firstName,
            lastName: res.data.lastName,
            mobile: res.data.mobile,
            profileImage: res.data.profileImage,
            username: res.data.username,
            email: res.data.email,
            dob: res.data.dob,
            address: res.data.address,
          };
          console.log(this.userObj);
        },
      });
    }

    this.service.cartCountValue.subscribe((res: any) => {
      this.cartCount = res;
    });

    this.UserTypeId_new = JSON.parse(localStorage.getItem('userType') || '{}');
    console.log('user tpe id abs ' + this.UserTypeId_new);

    if (this.UserTypeId_new == 1) {
      this.user = true;
      this.admin = false;
    }
    if (this.UserTypeId_new == 2) {
      this.user = false;
      this.admin = true;
    }

    this.DoGetAllProducts();

    this.Initializers();
    this.refreshData();
    this.DoGetUserValues();

    this.checkHaveProductsOrnot();
  }

  Initializers() {
    this.productForm = new FormGroup(
      { 
        productName: new FormControl('', [
          Validators.required,
          Validators.minLength(3),
        ]),
        productImage: new FormControl(''),
        category: new FormControl('', Validators.required),
        brand: new FormControl('', Validators.required),
        sellingPrice: new FormControl(null, [
          Validators.required,
          Validators.min(0),
        ]),
        purchasePrice: new FormControl(null, [
          Validators.required,
          Validators.min(0),
        ]),
        purchaseDate: new FormControl('', Validators.required),
        stock: new FormControl(null, [Validators.required, Validators.min(0)]),
      },
      { validators: sellingPriceGreaterThanPurchasePrice() }
    );
  }
  get productName() {
    return this.productForm.get('productName');
  }
  get productImage() {
    return this.productForm.get('productImage');
  }
  get category() {
    return this.productForm.get('category');
  }
  get brand() {
    return this.productForm.get('brand');
  }
  get sellingPrice() {
    return this.productForm.get('sellingPrice');
  }
  get purchasePrice() {
    return this.productForm.get('purchasePrice');
  }
  get purchaseDate() {
    return this.productForm.get('purchaseDate');
  }
  get stock() {
    return this.productForm.get('stock');
  }

  onViewProduct(data: any) {
    this.openViewProductModel();

    this.viewProductObject = {
      productName: data.productName,
      productCode: data.productCode,
      brand: data.brand,
      category: data.category,
      id: data.id,
      productImage: data.productImage,
      purchaseDate: data.purchaseDate,
      purchasePrice: data.purchasePrice,
      sellingPrice: data.sellingPrice,
      stock: data.stock,
    };
    console.log(this.viewProductObject);
  }

  onEdit(data: any) {
    this.openEditProductModal();

    //this.editProduct=true
    console.log(data);
    console.log(this.productForm.get('productName'));

    this.productForm.get('productName')?.setValue(data.productName);
    this.productForm.get('category')?.setValue(data.category);
    this.productForm.get('sellingPrice')?.setValue(data.sellingPrice);
    this.productForm.get('purchasePrice')?.setValue(data.purchasePrice);
    this.productForm.get('purchaseDate')?.setValue(data.purchaseDate);
    this.productForm.get('stock')?.setValue(data.stock);
    this.productForm.get('brand')?.setValue(data.brand);
    this.oldProductImage = data.productImage;

    this.productId = data.id;
  }

  onDelete(id: number) {
    Swal.fire({
      title: 'Are you sure?',
      text: `Do you want to delete the product?`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes',
      cancelButtonText: 'Cancel',
      reverseButtons: true,
    }).then((result: any) => {
      if (result.isConfirmed) {
        this.service.DoDeleteProduct(id).subscribe({
          next: (res: any) => {
            if (res.isSuccess) {
              console.log('success');
              Swal.fire(res.message, '', 'success');
              //alert(res.message)

              this.DoGetAllProducts();
            } else this.DoGetAllProducts();
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

    //   if(confirm("are you sure,you want to delete"))
    //   {this.service.DoDeleteProduct(id).subscribe({
    //     next:((res:any)=>{
    //       if(res.isSuccess)
    //       {

    //         console.log("success")
    //         this.toastr.success(res.message)
    //         //alert(res.message)

    //         this.DoGetAllProducts()

    //       }
    //       else
    //       this.DoGetAllProducts()
    //     }),
    //     error:(e:any)=>{
    //       console.log(e)

    //     }
    //   })
    // }
    //   console.log(id)
  }

  onAddToCart(id: number) {
    console.log(this.loggedUserValue?.id);
    console.log(this.loggedUserValue?.FirstName);
    var cartObj = {
      userId: this.loggedUserValue?.id,
      productId: id,
      quantity: 1,
    };

    this.service.DoAddToCart(cartObj).subscribe((res: any) => {
      console.log(res);

      const token = localStorage.getItem('token');

      if (token) {
        const decodeToken: any = jwtDecode(token);
        // this.userId=decodeToken.id
        this.service.updateCartQuentity(decodeToken.id);
      }
    });
    this.DoGetAllProducts();
  }

  DoGetAllProducts() {
    this.service.DoGetAllProducts(this.userId).subscribe((res: any) => {
      debugger;
      if (res.isSuccess) {
        this.products = res.data;
        this.checkHaveProductsOrnot();
      } else {
        this.products = [];
        //   this.toastr.warning(res.message,"",{timeOut:1000})
        this.checkHaveProductsOrnot();
      }
    });
  }

  DoGetUserValues() {
    this.userDetailsService.loggedUser.subscribe({
      next: (res: userDetailsModel) => {
        this.loggedUserValue = res;
        console.log('abc ', this.loggedUserValue);
        console.log(res.UserType_Id);
        this.UserTypeId_new = this.loggedUserValue?.UserType_Id;
        console.log('role id ', this.UserTypeId_new);
      },
    });
  }

  openChangePassModel() {
    const modal = document.getElementById('MyChangePassModel');
    if (modal != null) {
      modal.style.display = 'block';
      //this.makePayment()
    }
  }

  closeChangePassModel() {
    const modal = document.getElementById('MyChangePassModel');
    if (modal != null) {
      modal.style.display = 'none';
    }
  }

  openEditProductModal() {
    const modal = document.getElementById('myModal');
    if (modal != null) {
      modal.style.display = 'block';
      //this.makePayment()
    }
  }

  closeEditProductModal() {
    const modal = document.getElementById('myModal');
    if (modal != null) {
      modal.style.display = 'none';
    }
  }

  openProfileModal() {
    const modal = document.getElementById('myProfileModal');
    if (modal != null) {
      modal.style.display = 'block';
      //this.makePayment()
    }
  }

  closeProfilrModal() {
    const modal = document.getElementById('myProfileModal');
    if (modal != null) {
      modal.style.display = 'none';
    }
  }

  openViewProductModel() {
    const modal = document.getElementById('MyViewProductModel');
    if (modal != null) {
      modal.style.display = 'block';
      //this.makePayment()
    }
  }

  closeViewProductModel() {
    const modal = document.getElementById('MyViewProductModel');
    if (modal != null) {
      modal.style.display = 'none';
    }
  }

  imageFile: File | null = null;

  onFileChange(event: any): void {
    const file = event.target.files[0]; // Get the selected file
    if (file) {
      this.imageFile = file;
    }
  }

  onSubmit() {
    if (this.productForm.valid) {
      const formData = new FormData();
      Object.keys(this.productForm.controls).forEach((key) => {
        const value = this.productForm.get(key)?.value;

        if (key === 'productImage' && this.imageFile) {
          formData.append('file', this.imageFile); // Correct key for file
        } else if (value) {
          formData.append(key, value);
        }
      });

      if (this.imageFile) {
        this.service.DoAddImage(formData).subscribe({
          next: (res: any) => {
            const imagePath = res.data;

            console.log(res.data);

            this.product = {
              productName: this.productForm.get('productName')?.value || '',
              category: this.productForm.get('category')?.value || '',
              sellingPrice: this.productForm.get('sellingPrice')?.value || '',
              purchasePrice: this.productForm.get('purchasePrice')?.value || '',
              purchaseDate: this.productForm.get('purchaseDate')?.value || '',
              stock: this.productForm.get('stock')?.value || 0,
              brand: this.productForm.get('brand')?.value || '',

              productImage: imagePath,
              productId: this.productId,
            };

            this.UpdateProduct(this.product);

            //this.saveUser(this.profile)

            // this.saveUser(this.User)
          },
        });
      } else {
        this.product = {
          productName: this.productForm.get('productName')?.value || '',
          category: this.productForm.get('category')?.value || '',
          sellingPrice: this.productForm.get('sellingPrice')?.value || '',
          purchasePrice: this.productForm.get('purchasePrice')?.value || '',
          purchaseDate: this.productForm.get('purchaseDate')?.value || '',
          stock: this.productForm.get('stock')?.value || 0,
          brand: this.productForm.get('brand')?.value || '',

          productImage: this.oldProductImage,
          productId: this.productId,
        };
        this.UpdateProduct(this.product);
      }
    } else {
      console.log('Form is invalid');
    }
  }

  UpdateProduct(Payload: any) {
    console.log(Payload);
    this.service.DoUpdateProduct(Payload).subscribe((res: any) => {
      if (res.isSuccess == true) {
        this.toastr.success(res.message);
        // alert(`${res.message}`)
        console.log('success' + Payload);

        this.DoGetAllProducts();
        this.closeEditProductModal();
      } else {
        this.toastr.error(res.message);
        // alert(`${res.message}`)
        console.log('fail' + Payload);
      }
    });
    this.editProduct = false;
  }

  refreshData() {
    this.userDetailsService.loggedUser.subscribe({
      next: (res: any) => {
        this.loggedUserValue = res;
        console.log('abc ', this.loggedUserValue);
        //  this.userId=this.loggedUserValue?.id
        console.log(this.userId);
        this.UserTypeId_new = this.loggedUserValue?.UserType_Id;
        console.log('role id ', this.UserTypeId_new);
      },
    });
  }

  checkHaveProductsOrnot() {
    debugger;
    console.log(this.products);
    if (this.products.length > 0) {
      this.noProductAdded = false;
    } else {
      this.noProductAdded = true;
    }
  }

  limitInputLength(event: Event, maxLength: number): void {
    const input = event.target as HTMLInputElement;
    if (input.value.length > maxLength) {
      input.value = input.value.slice(0, maxLength);
    }
  }
}
