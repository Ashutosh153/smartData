import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export interface CartItem {
  id: number;
  name: string;
  price: number;
  quantity: number;
  image:string;
}

@Injectable({
  providedIn: 'root'
})
export class CartserviceService {

  constructor() { }

  private cartItems: CartItem[] = [];
  private cartSubject = new BehaviorSubject<CartItem[]>(this.cartItems);

  // Observable to track cart changes
  cart$ = this.cartSubject.asObservable();

  // Add item to cart
  addToCart(item: CartItem): void {
    const existingItem = this.cartItems.find((i) => i.id === item.id);
    if (existingItem) {
      existingItem.quantity += item.quantity;
    } else {
      this.cartItems.push(item);
    }
    this.cartSubject.next(this.cartItems);
  }

  // Remove item from cart
  removeFromCart(itemId: number): void {
    this.cartItems = this.cartItems.filter((item) => item.id !== itemId);
    this.cartSubject.next(this.cartItems);
  }

  // Get the total price of the cart
  getTotal(): number {
    return this.cartItems.reduce(
      (total, item) => total + item.price * item.quantity,
      0
    );
  }

  // Get the number of items in the cart
  getItemCount(): number {
    return this.cartItems.reduce((count, item) => count + item.quantity, 0);
  }

}
