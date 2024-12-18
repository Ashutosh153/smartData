import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-home-componet',
  standalone: true,
  imports: [RouterLink,CommonModule],
  templateUrl: './home-componet.component.html',
  styleUrl: './home-componet.component.css'
})
export class HomeComponetComponent implements OnInit {
  ngOnInit(): void {
    localStorage.removeItem("token")
  }
 router=inject(Router)
  onClickPatient()
  { 
    this.router.navigateByUrl("login/1");
  }
  onClickDoctor(){
    
    this.router.navigateByUrl("login/2")
  }

}
