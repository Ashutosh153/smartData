import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { UserServiceService } from '../../services/User/user-service.service';
import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import jsPDF from 'jspdf';

@Component({
  selector: 'app-invoice',
  standalone: true,
  imports: [CommonModule,CurrencyPipe, DatePipe],
  templateUrl: './invoice.component.html',
  styleUrl: './invoice.component.css'
})
export class InvoiceComponent {

  service=inject(UserServiceService)
  invoiceDetails:any
  router=inject(Router)

constructor(){
 const  invoiceId=localStorage.getItem("invoiceId");
//const invoiceId='ORD-001'
this.service.DoGetinvoiceDetails(invoiceId).subscribe({
  next:((res:any)=>{
    this.invoiceDetails=res.data
    console.log(res.data)
  })
})

}

downloadPDF(): void {

  const doc = new jsPDF('l', 'mm', 'a4');

  // Get the HTML content to be printed in the PDF (only invoice details, excluding buttons)
  const content = document.getElementById('invoice-details')?.innerHTML;

  if (content) {
    console.log("page size: ",doc.internal.pageSize)
    // Use jsPDF's html method to render content
    doc.html(content, {
      x: 10,
      y: 10,
      callback: (doc) => {
        // Save the PDF file after rendering
        doc.save('invoice.pdf');
      },
      width: 180, // Optional: This adjusts the width of the content in the PDF
      windowWidth: 850, // Optional: This adjusts the window width for better scaling
      html2canvas: {
        scale: 0.3, // Reduce the scaling to fit the content in one page
        logging: false,
        letterRendering: true, // Improve font rendering
        useCORS: true // Allow cross-origin resources like fonts or images
      },
   
    });
    
  
  }
}
}
