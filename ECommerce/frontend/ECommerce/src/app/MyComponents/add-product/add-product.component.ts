import { CommonModule } from '@angular/common';
import { Component, Inject, inject } from '@angular/core';
import {
  AbstractControl,
  FormControl,
  FormControlName,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { UserServiceService } from '../../services/User/user-service.service';
import { routes } from '../../app.routes';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { ToastrService } from 'ngx-toastr';

export function sellingPriceGreaterThanPurchasePrice(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const productForm = control as FormGroup;

    const sellingPrice = productForm.get('sellingPrice')?.value;
    const purchasePrice = productForm.get('purchasePrice')?.value;

    // If sellingPrice is greater than purchasePrice, return null (valid)
    if (sellingPrice > purchasePrice) {
      return null;
    }

    // Otherwise, return an error object indicating the validation failed
    return { sellingPriceLessThanPurchasePrice: true };
  };
}

export interface Product {
  productName: string; // Should be a string, validated with minLength and required
  productImage: string; // Should be a string, presumably for image URL or file path
  category: string; // Should be a string, validated as required
  brand: string; // Should be a string, validated as required
  sellingPrice: number; // Should be a number, validated as required and min value of 0
  purchasePrice: number; // Should be a number, validated as required and min value of 0
  purchaseDate: string; // Should be a string, validated as required (likely a date string)
  stock: number;
  IsCreatedCy: number; // Should be a number, validated as required and min value of 0
}

@Component({
  selector: 'app-add-product',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './add-product.component.html',
  styleUrl: './add-product.component.css',
})
export class AddProductComponent {
  service = inject(UserServiceService);
  router = inject(Router);
  product?: Product;
  userId: number = 0;
  token: any;
  toster = inject(ToastrService);
  TodaysDate: Date = new Date();
  maxDate = this.TodaysDate.toISOString().substring(0, 10);

  constructor() {
    this.token = localStorage.getItem('token');
    if (this.token) {
      const decodeToken: any = jwtDecode(this.token);
      this.userId = decodeToken.id;
    }
  }

  productForm = new FormGroup(
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

  imageFile: File | null = null;

  onFileChange(event: any): void {
    const file = event.target.files[0]; // Get the selected file
    if (file) {
      this.imageFile = file;
    }
  }
  // sellingPriceGreaterThanPurchasePrice(
  //   group: FormGroup
  // ): ValidationErrors | null {
  //   const sellingPrice = group.get('sellingPrice')?.value;
  //   const purchasePrice = group.get('purchasePrice')?.value;

  //   if (sellingPrice && purchasePrice && sellingPrice <= purchasePrice) {
  //     return { priceNotValid: true }; // Return error if sellingPrice <= purchasePrice
  //   }

  //   return null; // Valid if sellingPrice > purchasePrice
  // }
  onSubmit(): void {
    if (this.productForm.valid) {
      const formData = new FormData();

      // Loop through all controls and append data
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
            const imagePath: string = res.data;
            console.log('res image' + res.data);
            console.log('path get ' + imagePath);
            // this.productForm.patchValue({
            //   productImage: imagePath // Update form with image path
            // });

            this.product = {
              brand: this.productForm.get('brand')?.value || '',
              category: this.productForm.get('category')?.value || '',
              stock: this.productForm.get('stock')?.value || 0,
              productName: this.productForm.get('productName')?.value || '',
              purchaseDate: this.productForm.get('purchaseDate')?.value || '',
              purchasePrice: this.productForm.get('purchasePrice')?.value || 0,
              sellingPrice: this.productForm.get('sellingPrice')?.value || 0,
              IsCreatedCy: this.userId,
              productImage: imagePath,
            };

            //  this.productForm.get('productImage')?.setValue(imagePath)
            console.log('product image 1 ' + this.productImage);

            this.SaveProduct(this.product);
          },
          error: (e: any) => {
            console.log(e);
            this.toster.error('file Format not supported');
          },
        });
      } else {
        this.product = {
          brand: this.productForm.get('brand')?.value || '',
          category: this.productForm.get('category')?.value || '',
          stock: this.productForm.get('stock')?.value || 0,
          productName: this.productForm.get('productName')?.value || '',
          purchaseDate: this.productForm.get('purchaseDate')?.value || '',
          purchasePrice: this.productForm.get('purchasePrice')?.value || 0,
          sellingPrice: this.productForm.get('sellingPrice')?.value || 0,
          IsCreatedCy: this.userId,
          productImage:
            'https://localhost:7053/Uploads/3c26cac0-b311-4a64-8b8f-58b3ff0e0570.png',
        };

        this.SaveProduct(this.product);
      }
    }
  }

  limitInputLength(event: Event, maxLength: number): void {
    const input = event.target as HTMLInputElement;
    if (input.value.length > maxLength) {
      input.value = input.value.slice(0, maxLength);
    }
  }

  SaveProduct(payload: Product) {
    this.service.DoAddProduct(payload).subscribe((res: any) => {
      if (res.isSuccess) {
        console.log('Save all ');

        console.log('success');
        this.toster.success(res.message);
        this.router.navigateByUrl('dashboard');
      } else {
        this.toster.error(res.message);
      }
    });
  }
}
