import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { LoaderServiceService } from '../../services/loader-service.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-loader',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './loader.component.html',
  styleUrl: './loader.component.css'
})
export class LoaderComponent  implements OnInit, OnDestroy {
  isLoading = false;
  private subscription!: Subscription;

  constructor(private loaderService:LoaderServiceService) {}

  ngOnInit(): void {
    this.subscription = this.loaderService.isLoading.subscribe(
      (Loading) => {
        this.isLoading = Loading;
      }
    );
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}