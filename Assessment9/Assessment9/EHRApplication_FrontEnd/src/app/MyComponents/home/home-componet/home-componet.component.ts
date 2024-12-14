import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-home-componet',
  standalone: true,
  imports: [RouterLink,CommonModule],
  templateUrl: './home-componet.component.html',
  styleUrl: './home-componet.component.css'
})
export class HomeComponetComponent {
 router=inject(Router)
  onClickPatient()
  { debugger
    this.router.navigateByUrl("login/1");
  }
  onClickDoctor(){
    debugger
    this.router.navigate(["login/2"])
  }

}
