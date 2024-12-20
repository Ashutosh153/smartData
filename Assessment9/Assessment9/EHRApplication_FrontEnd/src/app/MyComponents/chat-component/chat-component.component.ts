import { Component, OnInit } from '@angular/core';
import { inject } from '@angular/core';
import { initializeApp } from 'firebase/app';
import { getDatabase, ref, set, onValue, child, push } from 'firebase/database';
import { firebaseConfig } from '../../../environment/firebase-config';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { jwtDecode } from 'jwt-decode';
import { ActivatedRoute } from '@angular/router';

const app = initializeApp(firebaseConfig);
const db = getDatabase(app);

@Component({
  selector: 'app-chat-component',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './chat-component.component.html',
  styleUrl: './chat-component.component.css',
})
export class ChatComponentComponent implements OnInit {
  senderId: string = '';
  receiverId: string = '';
  message: string = '';
  messages: any[] = [];

  constructor(private router: ActivatedRoute) {
    // this.receiverId=(this.router.snapshot.paramMap.get('id')||'')
  }
  ngOnInit() {
    this.receiverId = this.router.snapshot.paramMap.get('id') || '';
this.getSender();
    // Initialize listening to messages for a specific receiver
    // this.listenForMessages(this.receiverId);

    this.listenForMessages(this.senderId, this.receiverId);
  }

  getSender() {
    const token: any = localStorage.getItem('token');

    if (token) {
      const decodeToken: any = jwtDecode(token);
      const Id = decodeToken.id;
      this.senderId = decodeToken.id;
    }
  }


  // Send message to both sender-receiver and receiver-sender paths
  sendMessage(receiverId: string, message: string): void {
    const messageId = push(ref(db, `messages/${this.senderId}_${receiverId}`)).key;
    if (messageId) {
      const messageRefSenderReceiver = ref(db, `messages/${this.senderId}_${receiverId}/${messageId}`);
      set(messageRefSenderReceiver, {
        senderId: this.senderId,
        receiverId: receiverId,
        message: message,
        timestamp: Date.now(),
      });

      const messageRefReceiverSender = ref(db, `messages/${receiverId}_${this.senderId}/${messageId}`);
      set(messageRefReceiverSender, {
        senderId: this.senderId,
        receiverId: receiverId,
        message: message,
        timestamp: Date.now(),
      });
    }

  }

  // Listen for messages from both sender-receiver and receiver-sender paths
  listenForMessages(senderId: string, receiverId: string): void {
    const messagesRefSenderReceiver = ref(db, `messages/${senderId}_${receiverId}`);
    const messagesRefReceiverSender = ref(db, `messages/${receiverId}_${senderId}`);
    
    // Listen for messages from the sender-receiver path
    onValue(messagesRefSenderReceiver, (snapshot) => {
      const data = snapshot.val();
      if (data) {
        // Combine the messages and sort them by timestamp
        this.messages = Object.values(data).sort(
          (a: any, b: any) => a.timestamp - b.timestamp
        );
        console.log('Messages (Sender -> Receiver): ', this.messages);
      }
    });

    // Listen for messages from the receiver-sender path
    onValue(messagesRefReceiverSender, (snapshot) => {
      const data = snapshot.val();
      if (data) {
        // Combine the messages and sort them by timestamp
        this.messages = Object.values(data).sort(
          (a: any, b: any) => a.timestamp - b.timestamp
        );
        console.log('Messages (Receiver -> Sender): ', this.messages);
      }
    });
  }



  //  // Inject necessary dependencies (if needed)
  //  receiverId: string = '';
  //  message: string = '';
  //  messages: any[] = [];
  //  senderId:string=''
  //  getSender()
  //  {
  //       const token:any=localStorage.getItem('token')
  //       //const token="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJzYXZldG9rZW5zIiwiaWQiOiIzIiwiZW1haWwiOiJhc2h1dG9zaGd1cHRhNjE2ODZAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMiIsImV4cCI6MTczNDExNjI5NiwiaXNzIjoiaHR0cHM6Ly93d3cueW91dHViZS5jb20vIiwiYXVkIjoiaHR0cHM6Ly93d3cueW91dHViZS5jb20vIn0.NGD2eGG_58s40CyQCCVYf2V0RQHpbixRxSlI-4aMBJM"
  //       console.log("ghsfgghw"+token)
  //       if(token)
  //       {

  //         const decodeToken:any=jwtDecode(token);
  //         const Id=decodeToken.id
  //         this.senderId=decodeToken.id

  //       }
  //  }
  //  // Send message to receiver
  //  sendMessage(receiverId: string, message: string): void {
  //  const messageRef = ref(db, 'messages/' + receiverId);
  //  const newMessageRef = push(messageRef);
  //  set(newMessageRef, {
  //  senderId: this.senderId, // Set senderId dynamically
  //  message: message,
  //  timestamp: Date.now(),
  //  });
  //  console.log(newMessageRef)
  //  }
  //  // Listen for new messages for the receiver
  //  listenForMessages(receiverId: string): void {
  //  const messageRef = ref(db, 'messages/' + receiverId);
  //  onValue(messageRef, (snapshot) => {
  //  const data = snapshot.val();
  //  if (data) {
  //  this.messages = Object.values(data).sort((a: any, b: any) => a.timestamp - b.timestamp);
  //  }
  //  });
  //  }
}
