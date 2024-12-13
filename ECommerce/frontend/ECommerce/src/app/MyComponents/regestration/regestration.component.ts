import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { UserServiceService } from '../../services/User/user-service.service';
import { responceObject } from '../CommonClasses';
import { Router, RouterLink } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

export interface user {
  firstName: string;
  lastName: string;
  email: string;
  mobile: string;
  dob: string;
  userType_Id: number;
  profileImage: string;
  address: string;
  zipcode: string;
  country_Id: number;
  state_Id: number;
}

@Component({
  selector: 'app-regestration',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './regestration.component.html',
  styleUrl: './regestration.component.css',
})
export class RegestrationComponent {
  constructor() {
    this.getAllCountry();
  }

  service = inject(UserServiceService);
  User?: user;
  router = inject(Router);
  isLoading = false;
  TodaysDate: Date = new Date();
  toastr = inject(ToastrService);
  maxDate = this.TodaysDate.toISOString().substring(0, 10);

  userForm = new FormGroup({
    firstName: new FormControl('', [
      Validators.required,
      Validators.minLength(3),
    ]),
    lastName: new FormControl('', [
      Validators.required,
      Validators.minLength(3),
    ]),
    email: new FormControl('', [Validators.required, Validators.email]),
    mobile: new FormControl('', [
      Validators.required,
      Validators.pattern(/^\d{10}$/),
    ]), // Mobile validation
    dob: new FormControl('', Validators.required),
    userType_Id: new FormControl(0, Validators.required),
    profileImage: new FormControl(''),
    address: new FormControl(''),
    zipcode: new FormControl('', [Validators.required]), // Zipcode validation
    country_Id: new FormControl(0, Validators.required),
    state_Id: new FormControl(0, Validators.required),
  });

  limitInputLength(event: Event, maxLength: number): void {
    const input = event.target as HTMLInputElement;
    if (input.value.length > maxLength) {
      input.value = input.value.slice(0, maxLength);
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
    if (this.userForm.valid) {
      debugger;
      this.isLoading = true;

      const formData = new FormData();
      Object.keys(this.userForm.controls).forEach((key) => {
        const value = this.userForm.get(key)?.value;

        if (key === 'profileImage' && this.imageFile) {
          formData.append('file', this.imageFile); // Correct key for file
        } else if (value) {
          formData.append(key, value);
        }
      });

      if (this.imageFile) {
        this.service.DoAddImage(formData).subscribe({
          next: (res: any) => {
            const imagePath = res.data;

            this.User = {
              firstName: this.userForm.get('firstName')?.value || '',
              lastName: this.userForm.get('lastName')?.value || '',
              email: this.userForm.get('email')?.value || '',
              mobile: (this.userForm.get('mobile')?.value || '').toString(),
              dob: this.userForm.get('dob')?.value || '',
              userType_Id: this.userForm.get('userType_Id')?.value || 0,

              address: this.userForm.get('address')?.value || '',
              zipcode: (this.userForm.get('zipcode')?.value || '').toString(),
              country_Id: this.userForm.get('country_Id')?.value || 0,
              state_Id: this.userForm.get('state_Id')?.value || 0,

              profileImage: imagePath,
            };

            this.saveUser(this.User);
          },
        });
      } else {
        this.User = {
          firstName: this.userForm.get('firstName')?.value || '',
          lastName: this.userForm.get('lastName')?.value || '',
          email: this.userForm.get('email')?.value || '',
          mobile: (this.userForm.get('mobile')?.value || '').toString(),
          dob: this.userForm.get('dob')?.value || '',
          userType_Id: this.userForm.get('userType_Id')?.value || 0,

          address: this.userForm.get('address')?.value || '',
          zipcode: (this.userForm.get('zipcode')?.value || '').toString(),
          country_Id: this.userForm.get('country_Id')?.value || 0,
          state_Id: this.userForm.get('state_Id')?.value || 0,

          profileImage:
            'https://as1.ftcdn.net/jpg/05/79/55/26/1000_F_579552668_sZD51Sjmi89GhGqyF27pZcrqyi7cEYBH.webp',
        };

        console.log('image' + this.User.profileImage);
        console.log(this.User);

        this.saveUser(this.User);
      }
    }
  }
  saveUser(Payload: user) {
    this.service.DoRegester(Payload).subscribe({
      next: (res: any) => {
        if (res.isSuccess == true) {
          this.isLoading = false;
          this.toastr.success(res.message);
          // alert(`${res.message}`)
          console.log('success' + Payload);
          this.router.navigateByUrl('');
        } else {
          this.isLoading = false;
          this.toastr.error(res.message);
          //alert(`${res.message}`)
          console.log('fail' + Payload);
        }
      },
      error: (e: any) => {
        this.isLoading = false;
        this.toastr.error('something went wrong please try again later');
      },
    });
  }

  allCountry: any[] = [];

  getAllCountry() {
    this.service.DoGetAllCountries().subscribe({
      next: (res: any) => {
        this.allCountry = res.data;
        console.log(this.allCountry);
      },
      error: (error: any) => {},
    });
  }

  allState: any[] = [];

  allStateByCountryId: any[] = [];

  loadState(countryId: number) {
    this.service.DoGetAllStatesByCountry(countryId).subscribe((data: any) => {
      this.allState = data.data;
      console.log(data);
      // Set the selected state in the form
      // if (this.editData) {
      //   this.patientForm.patchValue({
      //     state: this.editData.state
      //   });
      // }
    });
  }

  onChange(countrId: any) {
    this.loadState(countrId);
  }
}
