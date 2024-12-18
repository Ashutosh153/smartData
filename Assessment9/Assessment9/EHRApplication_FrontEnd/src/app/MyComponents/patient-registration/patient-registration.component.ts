import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { UserServicesService } from '../../services/user-services.service';
import { ToastrService } from 'ngx-toastr';





@Component({
  selector: 'app-patient-registration',
  standalone: true,
  imports: [CommonModule, RouterLink,ReactiveFormsModule],
  templateUrl: './patient-registration.component.html',
  styleUrl: './patient-registration.component.css'
})

export class PatientRegistrationComponent {

  constructor()
  {
    this.getAllStates()
  }


  isLoading = false;
  User:any
  router=inject(Router)
  TodaysDate: Date = new Date();
  maxDate = this.TodaysDate.toISOString().substring(0, 10);
  
  service=inject(UserServicesService)
  toastr = inject(ToastrService)


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
    city_Id: new FormControl(0, Validators.required),
    state_Id: new FormControl(0, Validators.required),
    bloodGroup:new FormControl('',Validators.required),
    gender:new FormControl('',Validators.required)
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
    
  //  console.log(this.userForm.value)
    if (this.userForm.valid) {
 
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
              FirstName: this.userForm.get('firstName')?.value || '',
              LastName: this.userForm.get('lastName')?.value || '',
              Email: this.userForm.get('email')?.value || '',
              Mobile: (this.userForm.get('mobile')?.value || ''),
              DateOfBirth: this.userForm.get('dob')?.value || '',
              UserType_ID:1,
              BloodGroup:this.userForm.get('bloodGroup')?.value||'',
              Address: this.userForm.get('address')?.value || '',
              PinCode: (this.userForm.get('zipcode')?.value || ''),
              CityId: this.userForm.get('city_Id')?.value || 0,
              StateId: this.userForm.get('state_Id')?.value || 0,
              Gender:this.userForm.get('gender')?.value||'',
              Profile: imagePath,
            };

            this.saveUser(this.User);
          },
        });
      } else {

        debugger

        

        this.User = {
          FirstName: this.userForm.get('firstName')?.value || '',
          LastName: this.userForm.get('lastName')?.value || '',
          Email: this.userForm.get('email')?.value || '',
          Mobile: (this.userForm.get('mobile')?.value || '').toString(),
          DateOfBirth: this.userForm.get('dob')?.value || '',
          UserType_ID: 1,
          BloodGroup:this.userForm.get('bloodGroup')?.value||'',
          Address: this.userForm.get('address')?.value || '',
          PinCode: (this.userForm.get('zipcode')?.value || ''),
          CityId: this.userForm.get('city_Id')?.value || 0,
          StateId: this.userForm.get('state_Id')?.value || 0,
          Gender:this.userForm.get('gender')?.value||'',

          Profile:
            'https://as1.ftcdn.net/jpg/05/79/55/26/1000_F_579552668_sZD51Sjmi89GhGqyF27pZcrqyi7cEYBH.webp',
        };

        console.log('image' + this.User.profileImage);
        console.log(this.User);

        this.saveUser(this.User);
      }
    }
  }
  saveUser(Payload: any) {
   debugger
    this.service.DoRegistration(Payload).subscribe({
      next: (res: any) => {
        if (res.isSuccess == true) {
          this.isLoading = false;
         this.toastr.success(res.message);
           //alert(`${res.message}`)
          console.log('success' + Payload);
          this.router.navigateByUrl('/login/1');
        } else {
          this.isLoading = false;
         this.toastr.error(res.message);
         // alert(`${res.message}`)
          console.log('fail' + Payload);
        }
      },
      error: (e: any) => {
        this.isLoading = false;
        this.toastr.error('something went wrong please try again later');
      },
    });
  }

  allState: any[] = [];

  getAllStates() {
    this.service.DoGetAllStates().subscribe({
      next: (res: any) => {
        this.allState = res.data;
       // console.log(this.allCountry);
      },
      error: (error: any) => {},
    });
  }

  

  allCities: any[] = [];

  loadCities(stateID: number) {
    this.service.DoGetCitiesById(stateID).subscribe((data: any) => {
      this.allCities = data.data;
      console.log(data);
      // Set the selected state in the form
      // if (this.editData) {
      //   this.patientForm.patchValue({
      //     state: this.editData.state
      //   });
      // }
    });
  }

  onChange(stateID: any) {
    
    this.loadCities(stateID);
  }
}

