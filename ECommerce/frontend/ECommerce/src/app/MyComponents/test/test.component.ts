import { Component } from '@angular/core';

@Component({
  selector: 'app-test',
  standalone: true,
  imports: [],
  templateUrl: './test.component.html',
  styleUrl: './test.component.css'
})
export class TestComponent {
  selectedFile: File | null = null;


 onFileSelected(event: any): void {
    this.selectedFile = event.target.files[0];
  }





  
}
