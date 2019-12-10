import { Injectable, OnDestroy } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { Observable, Subject } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthService } from '../authentication/auth.service';

@Injectable({
  providedIn: 'root',
})
export class SignalRService implements OnDestroy {
  private hubConnection: signalR.HubConnection;
  public signalRConnected = false;
  public data: [];
  public interval;
  token: '';
  private subject = new Subject<any>();
  constructor(private authService: AuthService) {

  }

  public GetStatus(): Observable<string> {
    return this.subject.asObservable();
  }

  public startConnection() {
    const d = new Date();
    console.log('trying to connect to signal-r' + d.toString());
    const token = this.authService.getLocalToken();
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.signalrBaseUrl}/chat`, { accessTokenFactory: () => token })
      .build();

    Object.defineProperty(WebSocket, 'OPEN', { value: 1 });

    this.hubConnection
      .start()
      .then(() => {
        this.signalRConnected = true;
        console.log('Connection started');
        this.subject.next('connected');
        this.connected();
        this.addSendDataListener();
      })
      .catch((err) => {
        console.log('Error while starting connection: ' + JSON.stringify(err));
        this.signalRConnected = false;
        this.reconnect();
        this.subject.next('disconnected');
      });

    this.hubConnection.onclose((err) => {
      this.signalRConnected = false;
      console.log('SignalR connection Closed' + JSON.stringify(err));
      this.reconnect();
      this.subject.next('disconnected');
    });

  }
  private reconnect() {
    if (!this.interval) {
      this.interval = setInterval(() => { this.startConnection(); }, 30000);
    }
  }

  private connected() {
    if (this.interval) {
      clearInterval(this.interval);
      this.interval = undefined;
    }
  }

  public addSendDataListener = () => {
    this.hubConnection.on('Send', (data) => {
      this.data = data;
      console.log(data);
    });
  }

  ngOnDestroy() {
    if (this.interval) {
      clearInterval(this.interval);
    }
  }
}
